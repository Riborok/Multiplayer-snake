namespace SnakeGame
{
    // Class for a point on the game field
    public abstract class Point
    {
        // Coordinates of the point
        public int X { get; set; }
        public int Y { get; set; }

        // Symbol for drawing the point and its color
        public abstract char Symbol { get; }
        
        // Point color on the canvas
        public Color Color { get; }
             
        protected Point(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }

        // Copy the coordinates from another point
        public void CopyCoordinatesFrom(Point other)
        {
            X = other.X;
            Y = other.Y;
        }
        
    }
}