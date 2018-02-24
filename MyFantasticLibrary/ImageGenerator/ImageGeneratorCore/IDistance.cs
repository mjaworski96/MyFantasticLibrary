namespace ImageGeneratorCore
{
    /// <summary>
    /// Provides calculating distance.
    /// </summary>
    internal interface IDistance
    {
        /// <summary>
        /// Calculates distance.
        /// </summary>
        /// <param name="x1">X position of first point.</param>
        /// <param name="y1">Y position of first point.</param>
        /// <param name="x2">X position of second point.</param>
        /// <param name="y2">Y position of second point.</param>
        /// <returns>Distance between two points.</returns>
        double GetDistance(int x1, int y1, int x2, int y2);
    }
}
