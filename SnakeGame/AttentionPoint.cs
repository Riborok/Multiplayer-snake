namespace SnakeGame
{
    // This class represents a point that draws attention
    public class AttentionPoint : IDrawablePoint
    {
        public int X { get; }
        public int Y { get; }
        public char Symbol => '!';
        public Color Color => Color.Red;

        public AttentionPoint(ICoordinates coordinates)
        {
            X = coordinates.X;
            Y = coordinates.Y;
        }
    }
}