using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageGeneratorCore
{
    /// <summary>
    /// Represents image.
    /// </summary>
    internal class Image
    {
        /// <summary>
        /// Bitmap of image.
        /// </summary>
        private Bitmap _bitmap;

        /// <summary>
        /// Initializes new instance of Image.
        /// </summary>
        /// <param name="bitmap"></param>
        private Image(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }
        /// <summary>
        /// Loads image from file.
        /// </summary>
        /// <param name="path">Input filename.</param>
        /// <returns>Loaded image.</returns>
        public static Image Load(string path)
        {
            return new Image(new Bitmap(path));
        }
        /// <summary>
        /// Saves image to file.
        /// </summary>
        /// <param name="path">Output filename.</param>
        /// <param name="format">Output image format.</param>
        public void Save(string path, ImageFormat format)
        {
            _bitmap.Save(path, format);
        }
        private bool HasOnlyVisibleHeighbour(int x, int y)
        {
            if (x > 0 && GetPixel(x - 1, y).A == 0)
                return false;
            if (x < Width - 2 && GetPixel(x + 1, y).A == 0)
                return false;
            if (y > 0 && GetPixel(x, y - 1).A == 0)
                return false;
            if (y < Height - 2 && GetPixel(x, y + 1).A == 0)
                return false;

            return true;
        }
        /// <summary>
        /// All visible color (A > 0).
        /// </summary>
        public List<ColorWithPositon> KnownColors
        {
            get
            {
                List<ColorWithPositon> colors = new List<ColorWithPositon>();

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        Color c = GetPixel(x, y);
                        if (c.A != 0 && !HasOnlyVisibleHeighbour(x, y))
                            colors.Add(new ColorWithPositon(x, y, c));
                    }
                }

                return colors;
            }
        }
        /// <summary>
        /// Returns pixel at selected positon.
        /// </summary>
        /// <param name="x">X position of pixel.</param>
        /// <param name="y">Y position of pixel.</param>
        /// <returns>Color of pixel.</returns>
        public Color GetPixel(int x, int y)
        {
            return _bitmap.GetPixel(x, y);
        }
        /// <summary>
        /// Sets pixel color.
        /// </summary>
        /// <param name="x">X position of pixel.</param>
        /// <param name="y">Y position of pixel.</param>
        /// <param name="color">New color of pixel.</param>
        public void SetPixel(int x, int y, Color color)
        {
            _bitmap.SetPixel(x, y, color);
        }
        /// <summary>
        /// Creating blank image.
        /// </summary>
        /// <param name="width">With of new image.</param>
        /// <param name="height">Height of new image.</param>
        /// <returns>Blank image.</returns>
        public static Image CreateNew(int width, int height)
        {
            return new Image(new Bitmap(width, height));
        }
        /// <summary>
        /// Width of image.
        /// </summary>
        public int Width
        {
            get
            {
                return _bitmap.Width;
            }
        }
        /// <summary>
        /// Height of image.
        /// </summary>
        public int Height
        {
            get
            {
                return _bitmap.Height;
            }
        }
    }
}
