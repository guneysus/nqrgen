using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace nqrgen
{
    public class QR
    {
        private readonly string data;
        private readonly Options options;

        protected QR(string data, Options options)
        {
            this.data = data;
            this.options = options;
        }

        public static QR New(string data, Options options)
        {
            return new QR(data, options);
        }

        public void Process()
        {
            if (!options.Directory.Exists) options.Directory.Create();

            ErrorCorrectionLevel correction = default;

            switch (options.Correction)
            {
                case ErrorCorrection.Low:
                    correction = ErrorCorrectionLevel.L;
                    break;
                case ErrorCorrection.Medium:
                    correction = ErrorCorrectionLevel.M;
                    break;
                case ErrorCorrection.Quite:
                    correction = ErrorCorrectionLevel.Q;
                    break;
                case ErrorCorrection.High:
                    correction = ErrorCorrectionLevel.H;
                    break;
                default:
                    correction = ErrorCorrectionLevel.M;
                    break;
            }

            BarcodeWriterPixelData writer = createBarcodeWriterPixelData(correction, options.Size, options.Margin);

            PixelData pixelData = createPixelData(writer, data);
            using Bitmap bitmap = createBitmap(pixelData);
            BitmapData bitmapData = createBitmapData(bitmap, pixelData);

            try
            {
                Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            var filePath = Path.Join(options.Directory.FullName, data) + ".png";
            using FileStream fileStream = File.OpenWrite(filePath);

            bitmap.Save(fileStream, ImageFormat.Png);
        }


        public static PixelData createPixelData(IBarcodeWriterPixelData qrWriter, string data)
        {
            PixelData pixelData = qrWriter.Write(data);
            return pixelData;
        }

        public static Bitmap createBitmap(PixelData pixelData)
        {
            Bitmap bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            return bitmap;
        }

        public static BitmapData createBitmapData(Bitmap bitmap, PixelData pixelData)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            return bitmapData;
        }

        public static BarcodeWriterPixelData createBarcodeWriterPixelData(ErrorCorrectionLevel correction, int size = 30, int margin = 0)
        {

            var qrCodeWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = size,
                    Width = size,
                    Margin = margin,
                    ErrorCorrection = correction
                },
            };

            return qrCodeWriter;
        }

        public static BarcodeWriter createBarcodeWriterForConsole()
        {
            var qrCodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 1,
                    Width = 1,
                    Margin = 1,
                    ErrorCorrection = ErrorCorrectionLevel.L
                },
                Renderer = new CustomBitmapRenderer()
            };

            return qrCodeWriter;
        }
    }

}
