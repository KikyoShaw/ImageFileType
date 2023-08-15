using System.IO;
using System.Linq;

namespace ImageFileType
{
    public enum ImageFormat
    {
        Unknown,
        Bmp,
        Jpeg,
        Gif,
        Tiff,
        Png,
        WebP,
    }

    internal class ImageFormatChecker
    {
        // 文件头魔数: https://gist.github.com/leommoore/f9e57ba2aa4bf197ebc5
        private static readonly byte[] Bmp = { 66, 77 };
        private static readonly byte[] Jpeg = { 255, 216, 255 };
        private static readonly byte[] Gif = { 71, 73, 70, 56 };
        private static readonly byte[] Tiff1 = { 77, 77, 00, 42 }; //TIFF format (Motorola - big endian)
        private static readonly byte[] Tiff2 = { 73, 73, 42, 00 }; //TIFF format (Intel - little endian)
        private static readonly byte[] Png = { 137, 80, 78, 71 };
        private static readonly byte[] WebPRiff = { 82, 73, 70, 70 }; // 'RIFF'
        private static readonly byte[] WebPWebP = { 87, 69, 66, 80 }; // 'WEBP'
        private const int BufferLength = 12; // webp 需要这么长

        public static ImageFormat GetImageFormat(Stream stream)
        {
            var bytes = new byte[BufferLength];
            var read = stream.Read(bytes, 0, bytes.Length);
            return read < BufferLength ? ImageFormat.Unknown : GetImageFormat(bytes);
        }

        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            if (bytes.Length < BufferLength)
                return ImageFormat.Unknown;

            if (bytes.Take(Bmp.Length).SequenceEqual(Bmp))
                return ImageFormat.Bmp;
            if (bytes.Take(Jpeg.Length).SequenceEqual(Jpeg))
                return ImageFormat.Jpeg;
            if (bytes.Take(Gif.Length).SequenceEqual(Gif))
                return ImageFormat.Gif;
            if (bytes.Take(Tiff1.Length).SequenceEqual(Tiff1)
                || bytes.Take(Tiff2.Length).SequenceEqual(Tiff2))
                return ImageFormat.Tiff;
            if (bytes.Take(Png.Length).SequenceEqual(Png))
                return ImageFormat.Png;
            if (bytes.Take(WebPRiff.Length).SequenceEqual(WebPRiff)
                && bytes.Skip(8).Take(WebPWebP.Length).SequenceEqual(WebPWebP))
                return ImageFormat.WebP;

            return ImageFormat.Unknown;

        }
    }
}
