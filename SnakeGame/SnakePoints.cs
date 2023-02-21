using System;

namespace SnakeGame
{
    public class SnakeBodyPoint : Point
    {
        public SnakeBodyPoint(int x, int y) : base((x, y))
        {
        }
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('■');   
        }
    }
    
    public class SnakeHEADPoint : Point
    {
        public SnakeHEADPoint(int x, int y) : base((x, y))
        {
        }
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('☻');   
        }
    }
    
    
}