using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    public class SnakeBodyPoint : Point
    {
        protected override char Symbol => '■';
        public SnakeBodyPoint(SnakeHeadPoint snakeHeadPoint) : base(snakeHeadPoint.X, snakeHeadPoint.Y)
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
            return Enumerable.Repeat(new DigestibleBody(food), food.NutritionalValue);
        }
    }
    
    public class SnakeHeadPoint : Point
    {
        protected override char Symbol => '☻';
        public SnakeHeadPoint(int x, int y) : base(x, y)
        {
        }
    }
    
    
}