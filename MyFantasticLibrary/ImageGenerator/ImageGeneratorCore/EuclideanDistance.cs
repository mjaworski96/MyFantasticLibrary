using System;

namespace ImageGeneratorCore
{
    /// <summary>
    /// Calculate Euclidean distance.
    /// </summary>
    internal class EuclideanDistance : IDistance
    {
        /// <summary>
        /// Calculates Euclidean Distance.
        /// </summary>
        /// <param name="x1">X position of first point.</param>
        /// <param name="y1">Y position of first point.</param>
        /// <param name="x2">X position of second point.</param>
        /// <param name="y2">Y position of second point.</param>
        /// <returns>Euclidean distance between two points.</returns>
        public double GetDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
    }
}
