namespace SnakeGame
{
    // This class manages collisions between the snake's head and food objects on the canvas
    public class FoodCollisionManager
    {
        // Store the map to check for collisions
        private readonly IPointMap _pointMap;

        // Service for managing food objects
        private readonly IFoodService _foodService;
        
        public FoodCollisionManager(IFoodService foodService, IPointMap pointMap)
        {
            _foodService = foodService;
            _pointMap = pointMap;
        }
        
        // Checks for collisions between the snake's head and food
        public void CollisionCheck(Snake snake)
        {
            // If a collision occurs, the snake eats the food and the food is removed
            if (_pointMap.GetMap[snake.Head.X, snake.Head.Y] is Food collidingFood)
            {
                // Make the snake eat the food and remove it from the Map
                snake.Eat(collidingFood);
                _foodService.Remove(collidingFood);
            }
        }
    }
}