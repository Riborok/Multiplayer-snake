using System;
using System.Collections.Generic;

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

    public class DigestibleBody : Point
    {
        private DigestibleBody(Food food) : base((food.X, food.Y))
        {
        }
        
        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write('█');   
        }

        public static List<Point> GetListOfAddedBody (Food food)
        {
            FoodsInformation.Delete(food);
            List<Point> result = new List<Point>(); 
            for (int i = 0; i < food.NutritionalValue; i++)
                result.Add(new DigestibleBody(food));
            return result;
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