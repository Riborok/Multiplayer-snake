namespace SnakeGame
{
    // Class for a point on the game field
    public interface IPoint
    {
        // Coordinates of the point
        public int X { get; }
        public int Y { get; }

        // Symbol for drawing the point and its color
        public abstract char Symbol { get; }
        
        // Point color on the canvas
        public Color Color { get; }
    }
}