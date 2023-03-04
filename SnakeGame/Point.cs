using System;

namespace SnakeGame
{
    // Class for a point on the game field
    public abstract class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        protected abstract char Symbol { get; }

        protected Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public bool IsEquals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public void CopyCoordinatesFrom(Point point)
        {
            X = point.X;
            Y = point.Y;
        }
        
        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(Symbol);
        }

        public void Remove()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(" ");
        }
    }
}