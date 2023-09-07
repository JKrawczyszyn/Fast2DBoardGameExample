namespace Models
{
    public static class BoardPositionExtensions
    {
        public static BoardPosition RotateClockwise(this BoardPosition position) => new(position.Y, -position.X);
    }
}
