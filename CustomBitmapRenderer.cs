using System;
using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace nqrgen
{
    public class CustomBitmapRenderer : BitmapRenderer
    {
        public override Bitmap Render(BitMatrix matrix, BarcodeFormat format, string content, EncodingOptions options)
        {

            for (int i = 0; i < matrix.Width; i++)
            {
                for (int j = 0; j < matrix.Height; j++)
                {
                    if (matrix[j, i])
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.Write("  ");
                }
                Console.ResetColor();
                Console.WriteLine();
            }

            Console.ResetColor();

            Console.WriteLine(content);
            Console.WriteLine();

            return null;
        }
    }

}
