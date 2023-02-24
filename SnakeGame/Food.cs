using System;

namespace SnakeGame
{
    public abstract class Food : Point
    {
        protected Food((int x, int y) coord) : base(coord.x, coord.y)
        {
            Draw();
        }
        
        public abstract int NutritionalValue {get;}
        
    }

    // Class for food
    public class SimpleFood : Food
    {
        public SimpleFood() : base(Generate())
        {
        }
        
        public SimpleFood(int x, int y) : base((x, y))
        {
        }

        // Generate a new position for food
        private static (int x, int y) Generate()
        {
            var random = new Random();
            
            // Checking that the food is in an even position on the x coordinate
            int randomX = random.Next(2, Console.WindowWidth - 2); 
            return (x: randomX % 2 == 1 ? ++randomX : randomX, y: random.Next(2, Console.WindowHeight - 2));
        }
        
        // Draw the food
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('☼');
        }

        public override int NutritionalValue => 1;
    }

    public class SnakeHeadFood : Food
    {
        public SnakeHeadFood(SnakeHeadPoint head) : base((head.X, head.Y))
        {
        }
        
        // Draw the food
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('†');
        }
        
        public override int NutritionalValue => 10;
    }
}