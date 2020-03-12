using System.Drawing;

namespace Business.Abstraction
{
    public interface IOcrEngine
    {
        string ReadText(string base64value);
        string ReadText(Bitmap imgsource);
        string ReadHText(string base64value);
        string ReadHText(Bitmap imgsource);
    }

    
}
