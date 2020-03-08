using System;
using System.Drawing;
using System.IO;
using Business.Abstraction;
using Tesseract;

namespace Business.Implementation
{
    public class TesseractInstance: IOcrEngine
    {
        private Func<MyWorkerScope<TesseractEngine>> currentScope = null;
        
        internal TesseractInstance(Func<MyWorkerScope<TesseractEngine>> currentScope)
        {
            this.currentScope = currentScope;
        }

        public string ReadText(string base64)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            using (MemoryStream stream = new System.IO.MemoryStream(imageBytes))
            using (Bitmap bitImage = new Bitmap(Image.FromStream(stream)))
            {
                return this.ReadText(bitImage);
            }
        }


        public string ReadText(Bitmap imgsource)
        {
            var ocrtext = string.Empty;

            using (var img = PixConverter.ToPix(imgsource))
            {
                using (MyWorkerScope<TesseractEngine> scope = this.currentScope())
                {
                    using (var page = scope.GetEngine().Process(img))
                    {
                        ocrtext = page.GetText();
                    }
                }
            }

            return ocrtext;
        }

        public string ReadHText(string base64)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            using (MemoryStream stream = new System.IO.MemoryStream(imageBytes))
            using (Bitmap bitImage = new Bitmap(Image.FromStream(stream)))
            {
                return this.ReadHText(bitImage);
            }
        }

        public string ReadHText(Bitmap imgsource)
        {
            var ocrtext = string.Empty;

            using (var img = PixConverter.ToPix(imgsource))
            {
                using (MyWorkerScope<TesseractEngine> scope = this.currentScope())
                {
                    using (var page = scope.GetEngine().Process(img))
                    {
                        ocrtext = page.GetHOCRText(0);
                    }
                }
            }

            return ocrtext;
        }

    }
}
