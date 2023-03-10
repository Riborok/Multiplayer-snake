using System.Linq;

namespace SnakeGame
{
    // The manager checks the collision of the snake with food
    public class FoodCollisionManager
    {
        private readonly FoodInformationManager _foodInformationManager;
        public FoodCollisionManager(FoodInformationManager foodInformationManager)
        {
            _foodInformationManager = foodInformationManager;
        }
        
        // Check for collision with food
        public void CollisionCheck(Snake snake)
        {
            if (_foodInformationManager.GetFoodList
                    .FirstOrDefault(currFood => currFood.IsEquals(snake.Head)) is { } collidingFood)
            {
                snake.AddBodyPoints(DigestibleBody.GetListOfAddedBody(collidingFood));
                _foodInformationManager.Remove(collidingFood);
            }
            
        }
    }
}