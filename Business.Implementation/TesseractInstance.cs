using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace Business.Implementation
{
    public class TesseractInstance
    {
        private static TesseractInstance instance = new TesseractInstance();
        private TesseractEngine engine;

        private TesseractInstance()
        {
            string path = @"./tessdata";
            engine = new TesseractEngine(path, "rus", EngineMode.Default);
        }

        public static TesseractInstance getInstance()
        {
            return instance;
        }

        public string ReadText(Bitmap imgsource)
        {
            return engine.Process(PixConverter.ToPix(imgsource)).GetText().ToString();
        }
    
    }
}
