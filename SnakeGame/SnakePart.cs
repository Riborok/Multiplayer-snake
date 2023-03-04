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
        public SnakeBodyPoint(SnakePart snakePart) : base(snakePart.X, snakePart.Y)
        {
        }
        protected SnakeBodyPoint(Food food) : base(food.X, food.Y)
        {
        }
    }

    public class DigestibleBody : SnakeBodyPoint
    {
        protected override char Symbol => '█';
        private DigestibleBody(Food food) : base(food)
        {
        }

        public static List<SnakeBodyPoint> GetListOfAddedBody (Food food)
        {
            FoodInformation.Remove(food);
            List<SnakeBodyPoint> result = new(30); 
            for (var i = 0; i < food.NutritionalValue; i++)
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