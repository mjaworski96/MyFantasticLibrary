using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageGeneratorCore
{
    /// <summary>
    /// Generates new image by interpolating visible colors from input image.
    /// </summary>
    public class Generator
    {
        /// <summary>
        /// Distance calculator.
        /// </summary>
        private IDistance distance = new EuclideanDistance();

        /// <summary>
        /// Generates new image by interpolating input image.
        /// </summary>
        /// <param name="imageIn">Path to input image.</param>
        /// <param name="imageOut">Path to output image.</param>
        /// <param name="format">Format of output image.</param>

        public void Generate(string imageIn, string imageOut, ImageFormat format)
        {
            Image inputImage = Image.Load(imageIn);
            Image outputImage = Image.CreateNew(inputImage.Width, inputImage.Height);
            List<ColorWithPositon> knownColors = inputImage.KnownColors;

            for (int x = 0; x < inputImage.Width; x++)
            {
                for (int y = 0; y < inputImage.Height; y++)
                {
                    Color color;
                    double dist = 0;
                    foreach (var colorWithPosition in knownColors)
                    {
                        double d = distance.GetDistance(x, y, colorWithPosition.x, colorWithPosition.y);
                        dist += d;
                    }
                    color = AddColors(knownColors, x, y);

                    outputImage.SetPixel(x, y, color);
                }
            }

            outputImage.Save(imageOut, format);
        }
        /// <summary>
        /// Interpolates colors at x,y positon.
        /// </summary>
        /// <param name="colors">Known colors.</param>
        /// <param name="x">X position of new color.</param>
        /// <param name="y">Y position of new color.</param>
        /// <returns>Color calculated by interpolation to nearest unique known colors.</returns>
        private Color AddColors(List<ColorWithPositon> colors, int x, int y)
        {
            double r = 0, g = 0, b = 0, a = 0;
            Dictionary<Color, double> nearestColors = new Dictionary<Color, double>();
            foreach (var color in colors)
            {
                Color c = color.color;
                double distance = this.distance.GetDistance(color.x, color.y, x, y);
                if (nearestColors.ContainsKey(c))
                {
                    if (nearestColors[c] > distance)
                        nearestColors[c] = distance;
                }
                else
                    nearestColors.Add(c, distance);

            }
            double sumOfDistances = 0;
            foreach (var dist in nearestColors)
            {
                sumOfDistances += dist.Value;
            }
            foreach (var color in nearestColors)
            {
                double modifier = color.Value / sumOfDistances;
                Color c = color.Key;
                r += (c.R * modifier);
                g += (c.G * modifier);
                b += (c.B * modifier);
                a += (c.A * modifier);

            }
            return Color.FromArgb((int)a, (int)r, (int)g, (int)b);
        }
    }
}
