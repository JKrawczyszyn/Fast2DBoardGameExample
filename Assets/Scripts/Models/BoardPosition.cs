using System;
using System.Globalization;
using UnityEngine;

namespace Models
{
    public readonly struct BoardPosition : IEquatable<BoardPosition>, IFormattable
    {
        /// <summary>
        /// Column
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Row
        /// </summary>
        public readonly int Y;

        private readonly bool nonDefault;

        public BoardPosition(int x, int y)
        {
            X = x;
            Y = y;
            nonDefault = true;
        }

        public BoardPosition(Vector2Int v) : this(v.x, v.y)
        {
        }

        public float Magnitude => Mathf.Sqrt((X * X) + (Y * Y));

        public int SqrMagnitude => (X * X) + (Y * Y);

        public static float Distance(in BoardPosition a, in BoardPosition b)
        {
            float num1 = a.X - b.X;
            float num2 = a.Y - b.Y;

            return (float)Math.Sqrt((num1 * (double)num1) + (num2 * (double)num2));
        }

        public static BoardPosition Min(in BoardPosition lhs, in BoardPosition rhs) =>
            new(Mathf.Min(lhs.X, rhs.X), Mathf.Min(lhs.Y, rhs.Y));

        public static BoardPosition Max(in BoardPosition lhs, in BoardPosition rhs) =>
            new(Mathf.Max(lhs.X, rhs.X), Mathf.Max(lhs.Y, rhs.Y));

        public static BoardPosition Scale(in BoardPosition a, in BoardPosition b) => new(a.X * b.X, a.Y * b.Y);

        public BoardPosition Scale(in BoardPosition scale) => new(X * scale.X, Y * scale.Y);

        public BoardPosition Clamp(BoardPosition min, BoardPosition max)
        {
            int x = Math.Max(min.X, X);
            x = Math.Min(max.X, x);
            int y = Math.Max(min.Y, Y);
            y = Math.Min(max.Y, y);

            return new BoardPosition(x, y);
        }

        public static BoardPosition operator -(BoardPosition v) => new(-v.X, -v.Y);

        public static BoardPosition operator +(BoardPosition a, BoardPosition b) => new(a.X + b.X, a.Y + b.Y);

        public static BoardPosition operator -(BoardPosition a, BoardPosition b) => new(a.X - b.X, a.Y - b.Y);

        public static BoardPosition operator *(BoardPosition a, BoardPosition b) => new(a.X * b.X, a.Y * b.Y);

        public static BoardPosition operator *(int a, BoardPosition b) => new(a * b.X, a * b.Y);

        public static BoardPosition operator *(BoardPosition a, int b) => new(a.X * b, a.Y * b);

        public static BoardPosition operator /(BoardPosition a, int b) => new(a.X / b, a.Y / b);

        public static bool operator ==(BoardPosition lhs, BoardPosition rhs) => lhs.Equals(rhs);

        public static bool operator !=(BoardPosition lhs, BoardPosition rhs) => !(lhs == rhs);

        public override bool Equals(object other) => other is BoardPosition other1 && Equals(other1);

        public bool Equals(BoardPosition other) => nonDefault == other.nonDefault && X == other.X && Y == other.Y;

        public override int GetHashCode() => HashCode.Combine(X, Y, nonDefault);

        public override string ToString() => ToString(null, null);

        public string ToString(string format) => ToString(format, null);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;

            return $"({X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)})";
        }

        public static BoardPosition Zero { get; } = new(0, 0);
        public static BoardPosition One { get; } = new(1, 1);
        public static BoardPosition Up { get; } = new(0, 1);
        public static BoardPosition Down { get; } = new(0, -1);
        public static BoardPosition Left { get; } = new(-1, 0);
        public static BoardPosition Right { get; } = new(1, 0);
        public static BoardPosition Invalid { get; } = new(int.MinValue, int.MinValue);

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }
    }
}
