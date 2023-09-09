using System;
using System.Globalization;
using UnityEngine;

namespace Models
{
    public readonly struct BoardPosition : IEquatable<BoardPosition>, IFormattable
    {
        public readonly int X;
        public readonly int Y;

        private readonly bool nonDefault;

        public BoardPosition(int x, int y)
        {
            X = x;
            Y = y;

            nonDefault = true;
        }

        public int MagnitudeSqr => (X * X) + (Y * Y);

        public float Magnitude => Mathf.Sqrt((X * X) + (Y * Y));

        public float DistanceSqrTo(Vector2 floatPosition)
        {
            float x = X - floatPosition.x;
            float y = Y - floatPosition.y;

            return (x * x) + (y * y);
        }

        public float DistanceTo(Vector2 floatPosition) => Mathf.Sqrt(DistanceSqrTo(floatPosition));

        public BoardPosition RotatedClockwise() => new(Y, -X);

        public Vector2 ToVector2() => new(X, Y);

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
