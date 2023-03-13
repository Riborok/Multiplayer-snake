using System;

namespace SnakeGame
{
    // Abstract class for food
    public abstract class Food : Point
    {
        protected Food(int x, int y, ConsoleColor color) : base(x, y, color)
        {
        }
        
        public abstract int NutritionalValue {get;}
    }
    
    public class SimpleFood : Food
    {
        protected override char Symbol => '☼';   
        public override int NutritionalValue => 1;
        
        public SimpleFood((int x, int y) randomCoord, ConsoleColor color) : base(randomCoord.x, randomCoord.y, color)
        {
        }
        
        public SimpleFood(SnakeBodyPoint snakeBodyPoint) : base(snakeBodyPoint.X, snakeBodyPoint.Y, snakeBodyPoint.Color)
        {
        }
    }

    public class SnakeHeadFood : Food
    {
        private readonly int _amountOfBodyPoints;
        protected override char Symbol => '†'; 
        public override int NutritionalValue => _amountOfBodyPoints / 10 * 9 + 1;
        public SnakeHeadFood(SnakeHeadPoint head, int amountOfBodyPoints) : base(head.X, head.Y, head.Color)
        {
            _amountOfBodyPoints = amountOfBodyPoints;
        }
    }
}