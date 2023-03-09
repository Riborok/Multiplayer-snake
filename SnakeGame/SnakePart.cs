using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<DigestibleBody> GetListOfAddedBody (Food food)
        {
            FoodInformationManager.Remove(food);
            return Enumerable.Repeat(new DigestibleBody(food), food.NutritionalValue);
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