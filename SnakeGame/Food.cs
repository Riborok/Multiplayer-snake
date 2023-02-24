using System;

namespace SnakeGame
{
    public abstract class Food : Point
    {
        protected Food(int x, int y) : base(x, y)
        {
            Draw();
        }
        
        public abstract int NutritionalValue {get;}
        
    }

    // Class for food
    public class SimpleFood : Food
    {
        
        public SimpleFood((int x, int y) randomCoord) : base(randomCoord.x, randomCoord.y)
        {
        }
        
        public SimpleFood(int x, int y) : base(x, y)
        {
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
        public SnakeHeadFood(SnakeHeadPoint head) : base(head.X, head.Y)
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