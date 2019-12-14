using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstraction;
using DomainModels.Enums;
using Tesseract;

namespace Business.Implementation
{
    public class TesseractInstance: IOcrEngine, IDisposable
    {
        private readonly Dictionary<SupportedLangs, string> _supportedLangDictionary =
            new Dictionary<SupportedLangs, string>()
            {
                {SupportedLangs.Ru, "rus"},
                {SupportedLangs.En, "eng"},
            };

        private readonly ConcurrentDictionary<string, TesseractEngine> _supportedTSEDictionary = new ConcurrentDictionary<string, TesseractEngine>();

        private readonly string path = String.Empty;

        public TesseractInstance()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "Tesseract";
        }
        

        public string ReadText(string base64, SupportedLangs lang)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            using (MemoryStream stream = new System.IO.MemoryStream(imageBytes))
            using (Bitmap bitImage = new Bitmap(Image.FromStream(stream)))
            {
                return this.ReadText(bitImage, lang);
            }
        }


        public string ReadText(Bitmap imgsource, SupportedLangs lang)
        {
            string langValue = String.Empty;
            var ocrtext = string.Empty;

            if (_supportedLangDictionary.TryGetValue(lang, out langValue))
            {
                var engine = _supportedTSEDictionary.GetOrAdd(langValue,
                    s => { return new TesseractEngine(path, langValue, EngineMode.Default); });

                using (var img = PixConverter.ToPix(imgsource))
                {
                    using (var page = engine.Process(img))
                    {
                        ocrtext = page.GetText();
                    }
                }

            }
            else
            {
                throw new ArgumentOutOfRangeException("unsupported lang");
            }

            return ocrtext;
        }

        public void Dispose()
        {
            foreach (var tesseractEngine in _supportedTSEDictionary)
            {
                if (!tesseractEngine.Value.IsDisposed)
                {
                    tesseractEngine.Value.Dispose();
                }
            }
        }
    }
}
