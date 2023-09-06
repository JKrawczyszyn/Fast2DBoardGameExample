using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Utilities
{
    public static class TypeExtensions
    {
        public static bool Between<T>(this T value, T min, T max, bool inclusive = true) where T : IComparable
        {
            if (inclusive)
            {
                return Comparer<T>.Default.Compare(value, min) >= 0
                    && Comparer<T>.Default.Compare(value, max) <= 0;
            }

            return Comparer<T>.Default.Compare(value, min) > 0
                && Comparer<T>.Default.Compare(value, max) < 0;
        }

        public static T Clamp<T>(this T value, T min, T max) where T : IComparable
        {
            if (Comparer<T>.Default.Compare(value, min) < 0)
                return min;

            if (Comparer<T>.Default.Compare(value, max) > 0)
                return max;

            return value;
        }

        public static BoardPosition RotateClockwise(this BoardPosition position) => new(position.Y, -position.X);

        // Used so we can use fire and forget tasks and still catch exceptions.
        public static async void Forget(this Task task)
        {
            await task;
        }
    }
}
