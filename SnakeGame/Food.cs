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
        protected override char Symbol => '†'; 
        public override int NutritionalValue => 10;
        public SnakeHeadFood(SnakeHeadPoint head) : base(head.X, head.Y, head.Color)
        {
        }
    }
}