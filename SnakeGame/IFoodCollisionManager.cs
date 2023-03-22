namespace SnakeGame
{
    // Interface for checking collisions between snake and food
    public interface IFoodCollisionManager
    {
        void CollisionCheck(Snake snake);
    }
    
    // This class manages collisions between the snake's head and food on the canvas
    public class FoodCollisionManager : IFoodCollisionManager
    {
        // Store the map to check for collisions
        private readonly IPointMap _pointMap;

        // Service for managing food
        private readonly IFoodAddRemove _foodService;
        
        public FoodCollisionManager(IFoodAddRemove foodService, IPointMap pointMap)
        {
            _foodService = foodService;
            _pointMap = pointMap;
        }
        
        // Checks for collisions between the snake's head and food
        public void CollisionCheck(Snake snake)
        {
            // If a collision occurs, the snake eats the food and the food is removed
            if (_pointMap.GetPoint(snake.Head.X, snake.Head.Y) is Food collidingFood)
            {
                // Make the snake eat the food and remove it from the Map
                snake.Eat(collidingFood);
                _foodService.Remove(collidingFood);
            }
        }
    }
}