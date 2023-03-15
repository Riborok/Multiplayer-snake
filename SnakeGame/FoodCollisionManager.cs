namespace SnakeGame
{
    // The manager checks the collision of the snake with food
    public class FoodCollisionManager
    {
        // A reference to the FoodService, used to manipulate food
        private readonly FoodService _foodService;
        
        // Constructor for the FoodCollisionManager, requires a FoodService
        public FoodCollisionManager(FoodService foodService)
        {
            _foodService = foodService;
        }
        
        // Check for collision with food
        public void CollisionCheck(Snake snake)
        {
            if (_foodService.FoodDict.TryGetValue((snake.Head.X, snake.Head.Y), out var collidingFood))
            {
                // Make the snake eat the food and remove it from the FoodService
                snake.Eat(collidingFood);
                _foodService.Eaten(collidingFood);
            }
        }
    }
}