﻿using System;
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
        
    }
}
