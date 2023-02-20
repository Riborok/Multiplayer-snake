using System;

namespace SnakeGame
{
    
    // Class for food
    public class Food : Point
    {
        public Food() : base(Generate())
        {
            Draw();
        }
        
        public Food(int x, int y) : base((x, y))
        {
            Draw();
        }

        // Generate a new position for food
        private static (int x, int y) Generate()
        {
            Random random = new Random();
            return (x: random.Next(2, Console.WindowWidth - 2), y: random.Next(2, Console.WindowHeight - 2));
        }
        
        // Draw the food
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('$');
        }
    }
}