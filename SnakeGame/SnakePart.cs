using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // This class represents a part of the snake body
    public abstract class SnakePart : IDrawablePoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public abstract char Symbol { get; }
        public Color Color { get; }
        protected SnakePart(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }
    
    public class SnakeBodyPoint : SnakePart
    {
        // Symbol used to represent the SnakeBodyPoint
        public override char Symbol => '■';
        public SnakeBodyPoint(SnakeHeadPoint snakeHeadPoint) : base(snakeHeadPoint.X, snakeHeadPoint.Y, 
            snakeHeadPoint.Color)
        {
        }
        protected SnakeBodyPoint(Food food) : base(food.X, food.Y, food.Color)
        {
        }
    }

    public class DigestibleBody : SnakeBodyPoint
    {
        // Symbol used to represent the DigestibleBody
        public override char Symbol => '█';
        private DigestibleBody(Food food) : base(food)
        {
        }

        // Returns a list of DigestibleBody with length equal to the NutritionalValue of the food
        public static IEnumerable<DigestibleBody> GetListOfAddedBody (Food food)
        {
            return Enumerable.Repeat(new DigestibleBody(food), food.NutritionalValue);
        }
    }
    
    public class SnakeHeadPoint : SnakePart
    {
        // Symbol used to represent the SnakeHeadPoint
        public override char Symbol => 'W';
        public SnakeHeadPoint(int x, int y, Color color) : base(x, y, color)
        {
        }
    }
    
    public class SleepingPart : SnakePart
    {
        // Symbol used to represent the SleepingPart
        public override char Symbol => '#';
        public SleepingPart(SnakeHeadPoint head) : base(head.X, head.Y, head.Color)
        {
        }
    }
    
}