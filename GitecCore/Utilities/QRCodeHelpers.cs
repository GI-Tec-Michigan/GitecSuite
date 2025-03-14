using ZXing;
using ZXing.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;

namespace Gitec.Utilities
{
    public static class QRCodeHelpers
    {
        /// <summary>
        /// Generates a QR code and saves it as a PNG file.
        /// </summary>
        /// <param name="text">The text to encode in the QR code.</param>
        /// <param name="filePath">The file path where the QR code image should be saved.</param>
        /// <param name="size">Size of the QR code (default: 250px).</param>
        public static void GenerateQRCode(string text, string filePath, int size = 250)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Text cannot be null or empty.", nameof(text));

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            var qrBitmap = GenerateQRCodeImage(text, size);
            qrBitmap.Save(filePath, new PngEncoder());
        }

        /// <summary>
        /// Generates a QR code and returns it as a byte array.
        /// </summary>
        /// <param name="text">The text to encode.</param>
        /// <param name="size">Size of the QR code (default: 250px).</param>
        /// <returns>Byte array representing a PNG QR code image.</returns>
        public static byte[] GenerateQRCodeBytes(string text, int size = 250)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Text cannot be null or empty.", nameof(text));

            var qrBitmap = GenerateQRCodeImage(text, size);
            using var ms = new MemoryStream();
            qrBitmap.Save(ms, new PngEncoder());
            return ms.ToArray();
        }

        /// <summary>
        /// Generates a QR code and returns it as an Image object.
        /// </summary>
        private static Image<Rgba32> GenerateQRCodeImage(string text, int size)
        {
            var qrWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = size,
                    Width = size,
                    Margin = 1
                }
            };

            var pixelData = qrWriter.Write(text);
            var image = Image.LoadPixelData<Rgba32>(pixelData.Pixels, pixelData.Width, pixelData.Height);
            return image;
        }
    }
}

/*
        Generate and Save a QR Code to a File
        string text = "https://popsiclesunday.com"; // URL or any text
        string filePath = "qrcode.png";
        try
        {
            QRCodeHelpers.GenerateQRCode(text, filePath);
            Console.WriteLine($"QR Code saved to: {Path.GetFullPath(filePath)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating QR Code: {ex.Message}");
        }
        
        Generate QR Code as Byte Array (For APIs, Web Apps, etc.)
        string text = "Hello, QR Code!";
        try
        {
            byte[] qrBytes = QRCodeHelpers.GenerateQRCodeBytes(text);
            File.WriteAllBytes("qrcode_bytes.png", qrBytes);
            Console.WriteLine("QR Code generated and saved as a byte array.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating QR Code: {ex.Message}");
        }


*/