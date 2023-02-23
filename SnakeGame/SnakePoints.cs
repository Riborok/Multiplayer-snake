using System;
using System.Collections.Generic;

namespace SnakeGame
{
    public abstract class SnakePart: Point
    {
        protected SnakePart(int x, int y) : base(x,y)
        {
        }
    }
    
    public class SnakeBodyPoint : SnakePart
    {
        public SnakeBodyPoint(SnakeHEADPoint head) : base(head.X, head.Y)
        {
        }
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('■');   
        }
    }

    public class DigestibleBody : SnakePart
    {
        private DigestibleBody(Food food) : base(food.X, food.Y)
        {
        }
        
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('█');   
        }

        public static List<SnakePart> GetListOfAddedBody (Food food)
        {
            FoodsInformation.Delete(food);
            List<SnakePart> result = new List<SnakePart>(); 
            for (int i = 0; i < food.NutritionalValue; i++)
                result.Add(new DigestibleBody(food));
            return result;
        }
    }
    
    public class SnakeHEADPoint : SnakePart
    {
        public SnakeHEADPoint(int x, int y) : base(x, y)
        {
        }
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('☻');   
        }
    }
    
    
}