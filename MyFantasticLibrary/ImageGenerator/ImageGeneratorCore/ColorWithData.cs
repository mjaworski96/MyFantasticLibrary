using System.Drawing;

namespace ImageGeneratorCore
{
    /// <summary>
    /// Color with it's postion
    /// </summary>
    public struct ColorWithPositon
    {
        /// <summary>
        /// X position of color.
        /// </summary>
        public int x;
        /// <summary>
        /// Y positon of color.
        /// </summary>
        public int y;
        /// <summary>
        /// Color.
        /// </summary>
        public Color color;
        /// <summary>
        /// Initializes new instance o ColorWithPosition.
        /// </summary>
        /// <param name="x">X position of Color</param>
        /// <param name="y">Y positon of Color</param>
        /// <param name="color">Color.</param>
        public ColorWithPositon(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
    }
}
