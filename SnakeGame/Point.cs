using System;

namespace SnakeGame
{
    // Class for a point on the game field
    public abstract class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public bool IsEquals(Point other)
        {
            return X == other.X && Y == other.Y;
        }
        
        public abstract void Draw();

        public void Remove()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(" ");
        }
    }
}