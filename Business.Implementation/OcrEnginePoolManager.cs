using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Abstraction;
using DomainModels.Enums;
using Tesseract;

namespace Business.Implementation
{
    public class OcrEnginePoolManager: IOcrEnginePoolManager,IDisposable
    {
        private readonly Dictionary<SupportedLangs, string> _supportedLangDictionary =
            new Dictionary<SupportedLangs, string>()
            {
                {SupportedLangs.Ru, "rus"},
                {SupportedLangs.En, "eng"},
            };

        private readonly ConcurrentDictionary<string, ConcurrentQueue<TesseractEngine>> _supportedTSEDictionary 
            = new ConcurrentDictionary<string, ConcurrentQueue<TesseractEngine>>();
        
        public static int midpointCount = 0;
        /// <summary>
        /// max number of parallel ocr engines
        /// </summary>
        private static int maxCount = Environment.ProcessorCount;

        private readonly string path = String.Empty;

        public OcrEnginePoolManager()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "Tesseract/tessdata";

            foreach (var lang in _supportedLangDictionary)
            {
                var queue = _supportedTSEDictionary.GetOrAdd(lang.Value,s =>new ConcurrentQueue<TesseractEngine>());

                for (var i = 0; i < maxCount; i++)
                {
                    queue.Enqueue(new TesseractEngine(path, lang.Value, EngineMode.Default));
                }
            }
        }

        private void cleanItem(SupportedLangs lang, TesseractEngine currentValue)
        {
            string langValue = String.Empty;
            if (_supportedLangDictionary.TryGetValue(lang, out langValue))
            {
                _supportedTSEDictionary[langValue].Enqueue(currentValue);
                Interlocked.Decrement(ref midpointCount);
            }
        }

        protected TesseractEngine GetEngine(SupportedLangs lang)
        {
            string langValue = String.Empty;
            if (_supportedLangDictionary.TryGetValue(lang, out langValue))
            {
                TesseractEngine currentValue;
                if (maxCount >= Interlocked.Increment(ref midpointCount) && _supportedTSEDictionary[langValue].TryDequeue(out currentValue))
                {
                    return currentValue;
                }
                else
                {
                    Interlocked.Decrement(ref midpointCount);
                    Thread.Sleep(10);
                    return GetEngine(lang);
                }
            }
            throw new ArgumentOutOfRangeException("unsupported lang");

        }

        public IOcrEngine GetEngineForLang(SupportedLangs lang)
        {
            return new TesseractInstance(() =>
                new MyWorkerScope<TesseractEngine>(() => GetEngine(lang),
                    engine => cleanItem(lang, engine)));

        }



        public void Dispose()
        {
            foreach (var tesseractEngines in _supportedTSEDictionary)
            {
                foreach (var engine in tesseractEngines.Value)
                {
                    if (!engine.IsDisposed)
                    {
                        engine.Dispose();
                    }
                }
            }
        }
    }
    internal class MyWorkerScope<T> : System.IDisposable where T:IDisposable
    {

        Lazy<T> currentValue;
        private Func<T> createEngine;
        Action<T> cleanItem;

        public T GetEngine()
        {
            return currentValue.Value;
        }

        public MyWorkerScope(Func<T> currentValue, Action<T> cleanItem)
        {
            this.currentValue = new Lazy<T>(currentValue);
            this.cleanItem = cleanItem;
        }

        public void Dispose()
        {
            if (currentValue.IsValueCreated)
            {
                cleanItem(this.currentValue.Value);
            }
        }
    }
}
