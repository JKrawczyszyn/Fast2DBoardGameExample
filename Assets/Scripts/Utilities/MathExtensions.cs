using System;
using System.Collections.Generic;

namespace Utilities
{
    public static class MathExtensions
    {
        public static bool Between(this int value, int min, int max, bool inclusive = true)
        {
            if (inclusive)
                return value >= min && value <= max;

            return value > min && value < max;
        }

        public static T Clamp<T>(this T value, T min, T max) where T : IComparable
        {
            if (Comparer<T>.Default.Compare(value, min) < 0)
                return min;

            if (Comparer<T>.Default.Compare(value, max) > 0)
                return max;

            return value;
        }
    }
}
