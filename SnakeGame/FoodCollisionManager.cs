using System.Linq;

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
            if (_foodService.GetFoodList
                    .FirstOrDefault(currFood => currFood.IsEquals(snake.Head)) is { } collidingFood)
            {
                snake.Eat(collidingFood);
                _foodService.Eaten(collidingFood);
            }
            
        }
    }
}