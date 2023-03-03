namespace SnakeGame
{
    // Abstract class for food
    public abstract class Food : Point
    {
        protected Food(int x, int y) : base(x, y)
        {
            Draw();
        }
        
        public abstract int NutritionalValue {get;}

        public override void Remove()
        {
            base.Remove();
            FoodsInformation.Delete(this);
        }
    }
    
    public class SimpleFood : Food
    {
        protected override char Symbol => '☼';   
        public SimpleFood((int x, int y) randomCoord) : base(randomCoord.x, randomCoord.y)
        {
        }
        
        public SimpleFood(SnakeBodyPoint snakeBodyPoint) : base(snakeBodyPoint.X, snakeBodyPoint.Y)
        {
        }

        public override int NutritionalValue => 1;
    }

    public class SnakeHeadFood : Food
    {
        protected override char Symbol => '†'; 
        public SnakeHeadFood(SnakeHeadPoint head) : base(head.X, head.Y)
        {
        }

        public override int NutritionalValue => 10;
    }
}