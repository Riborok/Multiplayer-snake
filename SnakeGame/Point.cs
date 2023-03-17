using System;

namespace SnakeGame
{
    // Class for a point on the game field
    public abstract class Point
    {
        // Coordinates of the point
        public (int x, int y) Coords { get; set; }

        // Symbol for drawing the point and its color
        protected abstract char Symbol { get; }
        public ConsoleColor Color { get; }
             
        protected Point((int x, int y) coords, ConsoleColor color)
        {
            Coords = coords;
            Color = color;
        }

        // Copy the coordinates from another point
        public void CopyCoordinatesFrom(Point other)
        {
            Coords = other.Coords;
        }
        
        // Draw the point on the console
        public void Draw()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(Coords.x, Coords.y);
            Console.Write(Symbol);
        }

        // Remove the point from the console
        public void Remove()
        {
            Console.SetCursorPosition(Coords.x, Coords.y);
            Console.Write(" ");
        }
    }
}