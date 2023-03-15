namespace SnakeGame
{
    // The manager checks the collision of the snake with food
    public class FoodCollisionManager
    {
        private readonly FoodService _foodService;
        public FoodCollisionManager(FoodService foodService)
        {
            _foodService = foodService;
        }
        
        // Check for collision with food
        public void CollisionCheck(Snake snake)
        {
            if (_foodService.FoodDict.TryGetValue((snake.Head.X, snake.Head.Y), out var collidingFood))
            {
                snake.Eat(collidingFood);
                _foodService.Eaten(collidingFood);
            }
        }
    }
}