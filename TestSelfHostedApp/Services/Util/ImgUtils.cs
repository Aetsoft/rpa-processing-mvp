using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpaSelfHostedApp.Services.Util
{
    public class ImgUtils
    {
        public static Bitmap toBitmap(string base64image)
        {
            System.IO.MemoryStream streamBitmap = null;
            try
            {
                Byte[] bitmapData = Convert.FromBase64String(base64image);
                streamBitmap = new System.IO.MemoryStream(bitmapData);

                Bitmap bitmap = new Bitmap((Bitmap)Image.FromStream(streamBitmap));
                return bitmap;
            }
            finally
            {
                streamBitmap.Close();
            }
        }
        
        
    }
}
