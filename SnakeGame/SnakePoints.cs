using System;

namespace SnakeGame
{
    public class SnakePoint : Point
    {
        public SnakePoint(int x, int y) : base(x, y)
        {
        }
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('*');   
        }
    }
}