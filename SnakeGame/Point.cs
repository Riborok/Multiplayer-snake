using System;

namespace SnakeGame
{
    // Class for a point on the game field
    public abstract class Point
    {
        // Coordinates of the point
        public int X { get; set; }
        public int Y { get; set; }

        // Symbol for drawing the point and its color
        protected abstract char Symbol { get; }
        public ConsoleColor Color { get; }
             
        protected Point(int x, int y, ConsoleColor color)
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
        
        // Draw the point on the console
        public void Draw()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(X, Y);
            Console.Write(Symbol);
        }

        // Remove the point from the console
        public void Remove()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(" ");
        }
    }
}