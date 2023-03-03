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
        protected override char Symbol => '■';
        public SnakeBodyPoint(SnakeHeadPoint head) : base(head.X, head.Y)
        {
        }
    }

    public class DigestibleBody : SnakePart
    {
        protected override char Symbol => '█';
        private DigestibleBody(Food food) : base(food.X, food.Y)
        {
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
    
    public class SnakeHeadPoint : SnakePart
    {
        protected override char Symbol => '☻';
        public SnakeHeadPoint(int x, int y) : base(x, y)
        {
        }
    }
    
    
}