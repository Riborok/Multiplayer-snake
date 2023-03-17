using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    public class SnakeBodyPoint : Point
    {
        // Symbol used to represent the SnakeBodyPoint
        protected override char Symbol => '■';
        public SnakeBodyPoint(SnakeHeadPoint snakeHeadPoint) : base(snakeHeadPoint.Coords, snakeHeadPoint.Color)
        {
        }
        protected SnakeBodyPoint(Food food) : base(food.Coords, food.Color)
        {
        }
    }

    public class DigestibleBody : SnakeBodyPoint
    {
        // Symbol used to represent the DigestibleBody
        protected override char Symbol => '█';
        private DigestibleBody(Food food) : base(food)
        {
        }

        // Returns a list of DigestibleBody with length equal to the NutritionalValue of the food
        public static IEnumerable<DigestibleBody> GetListOfAddedBody (Food food)
        {
            return Enumerable.Repeat(new DigestibleBody(food), food.NutritionalValue);
        }
    }
    
    public class SnakeHeadPoint : Point
    {
        // Symbol used to represent the SnakeHeadPoint
        protected override char Symbol => '☻';
        public SnakeHeadPoint((int x, int y) coords, ConsoleColor color) : base(coords, color)
        {
        }
    }
    
}