using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpaSelfHostedApp.Services.Util
{
    public static class ImgUtils
    {
        public static Bitmap toBitmap(string base64image)
        {
            System.IO.MemoryStream streamBitmap = null;
            try
            {
                Byte[] bitmapData = Convert.FromBase64String(base64image);
                streamBitmap = new System.IO.MemoryStream(bitmapData);

                Bitmap bitmap = (Bitmap)Image.FromStream(streamBitmap);
                return bitmap;
            }
            finally
            {
                streamBitmap?.Close();
                streamBitmap?.Dispose();
            }
        }

        /// <summary>
        /// Get rectangle area from original image. Non-thread safe
        /// </summary>
        /// <param name="localImage"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static Bitmap GetRectFromBitmap(this Bitmap localImage, int X, int Y, int Width, int Height)
        {
            Rectangle rect = new Rectangle(X, Y, Width, Height);
            int totalWidth = rect.Left + rect.Width; //think -the same as Right property

            int allowableWidth = localImage.Width - rect.Left;
            int finalWidth = 0;

            if (totalWidth > allowableWidth)
            {
                finalWidth = allowableWidth;
            }
            else
            {
                finalWidth = totalWidth;
            }

            rect.Width = finalWidth;

            int totalHeight = rect.Top + rect.Height; //think same as Bottom property
            int allowableHeight = localImage.Height - rect.Top;
            int finalHeight = 0;

            if (totalHeight > allowableHeight)
            {
                finalHeight = allowableHeight;
            }
            else
            {
                finalHeight = totalHeight;
            }

            rect.Height = finalHeight;

            return localImage.Clone(rect, localImage.PixelFormat);
        }
        
    }
}
