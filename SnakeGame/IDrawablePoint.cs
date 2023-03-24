namespace SnakeGame
{
    // Interface for objects with coordinates
    public interface ICoordinates
    {
        
        public int X { get; }
        public int Y { get; }
    }
    
    // Interface for objects that can be drawn on a canvas
    public interface IDrawable
    {
        
        public char Symbol { get; }
        public Color Color { get; }
    }
    
    // Interface for points, which have coordinates and can be drawn on a canvas
    public interface IDrawablePoint : ICoordinates, IDrawable
    {
    }
}