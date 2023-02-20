using System;

namespace SnakeGame
{
    
    // Class for food
    class Food : Point
    {
        public Food() : base(0, 0)
        {
            Generate();
            Draw();
        }

        // Generate a new position for food
        private void Generate()
        {
            Random random = new Random();
            X = random.Next(2, Console.WindowWidth - 2);
            Y = random.Next(2, Console.WindowHeight - 2);
        }
        
        // Draw the food
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('$');
        }
    }
}