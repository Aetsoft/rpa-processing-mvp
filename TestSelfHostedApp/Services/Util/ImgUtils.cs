using System;
using System.Drawing;

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
                return localImage.Clone(rect, localImage.PixelFormat);     
        }
        
    }
}
