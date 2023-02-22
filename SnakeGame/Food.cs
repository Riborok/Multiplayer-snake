using System;

namespace SnakeGame
{
    public abstract class Food : Point
    {
        public Food((int x, int y) coord) : base(coord)
        {
        }
        
        public abstract int NutritionalValue { get;}
        
    }

    // Class for food
    public class SimpleFood : Food
    {
        public SimpleFood() : base(Generate())
        {
            Draw();
        }
        
        public SimpleFood(int x, int y) : base((x, y))
        {
            Draw();
        }

        // Generate a new position for food
        private static (int x, int y) Generate()
        {
            Random random = new Random();
            
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

        public override int NutritionalValue {get => 1;}
        
    }

    public class SnakeHeadFood : Food
    {
        public SnakeHeadFood(SnakeHEADPoint head) : base((head.X, head.Y))
        {
            Draw();
        }
        
        // Draw the food
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('†');
        }
        
        public override int NutritionalValue {get => 5;}
    }
}