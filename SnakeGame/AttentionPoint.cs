namespace SnakeGame
{
    // This class represents a point that draws attention
    public class AttentionPoint : IDrawablePoint
    {
        public int X { get; }
        public int Y { get; }
        public char Symbol => '!';
        public Color Color { get; }

        public AttentionPoint(IDrawablePoint drawablePoint)
        {
            X = drawablePoint.X;
            Y = drawablePoint.Y;
            Color = drawablePoint.Color;
        }
    }
}