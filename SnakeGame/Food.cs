using System;

namespace SnakeGame
{
    // Abstract class for food
    public abstract class Food : Point
    {
        protected Food(int x, int y, ConsoleColor color) : base(x, y, color)
        {
        }
        
        // The nutritional value of the food
        public abstract int NutritionalValue { get; }
    }
    
    // Simple food class
    public class SimpleFood : Food
    {
        // Symbol used to represent the SimpleFood
        protected override char Symbol => '☼';   
        
        // Nutritional value of simple food
        public override int NutritionalValue => 1;
        
        public SimpleFood((int x, int y) randomCoord, ConsoleColor color) : base(randomCoord.x, randomCoord.y, color)
        {
        }
        
        public SimpleFood(SnakeBodyPoint snakeBodyPoint) : base(snakeBodyPoint.X, snakeBodyPoint.Y, snakeBodyPoint.Color)
        {
        }
    }

    // Food class that is spawned on the snake's head
    public class SnakeHeadFood : Food
    {
        // Amount of snake body points
        private readonly int _amountOfBodyPoints;
        
        // Symbol used to represent the SnakeHeadFood
        protected override char Symbol => '†'; 
        
        // Nutritional value of snake head food
        public override int NutritionalValue => Math.Min(3, _amountOfBodyPoints / 20) * 10 + 1;
        public SnakeHeadFood(SnakeHeadPoint head, int amountOfBodyPoints) : base(head.X, head.Y, head.Color)
        {
            _amountOfBodyPoints = amountOfBodyPoints;
        }
    }
}