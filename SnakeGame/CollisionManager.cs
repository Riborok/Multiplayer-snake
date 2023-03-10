using System;
using System.Linq;

namespace SnakeGame
{
    // The manager checks the collision of the snake with some object
    public class CollisionManager
    {
        private readonly FoodInformationManager _foodInformationManager;
        private readonly SnakeInformationManager _snakeInformationManager;

        public CollisionManager(FoodInformationManager foodInformationManager,
            SnakeInformationManager snakeInformationManager)
        {
            _foodInformationManager = foodInformationManager;
            _snakeInformationManager = snakeInformationManager;
        }

        // Check for collision with food
        public void FoodCollisionCheck(Snake snake)
        {
            if (_foodInformationManager.GetFoodList
                    .FirstOrDefault(currFood => currFood.IsEquals(snake.Head)) is { } collidingFood)
            {
                snake.AddBodyPoints(DigestibleBody.GetListOfAddedBody(collidingFood));
                _foodInformationManager.Remove(collidingFood);
            }
            
        }
        
        // Check for collision with objects 
        public bool IsNotCollisionOccured(Snake snake)
        {
            // Check for collision with an obstacle or another snake (and with their parts)
            if (CheckCollisionWithObstacles(snake) || CheckCollisionWithPartsOfSnakes(snake))
            {
                // If the snake collided with an obstacle or another snake, roll the snake back
                snake.Head.CopyCoordinatesFrom(snake.PreviousPart);

                Kill(snake);
                return false;
            }
            return true;
        }
        
        // Check collision with obstacles 
        private bool CheckCollisionWithObstacles(Snake snake)
        {
            return snake.Head.X < 1 || snake.Head.X > Console.WindowWidth - 1 || 
                   snake.Head.Y < 1 || snake.Head.Y > Console.WindowHeight - 2;
        }
        
        // Check collision with parts of snakes
        private bool CheckCollisionWithPartsOfSnakes(Snake snake)
        {
            // Checking for collisions with other parts of the snakes and own parts (except head)    
            if (_snakeInformationManager.GetListPointsOfSnakes().FirstOrDefault(point => point.IsEquals(snake.Head)
                 && point != snake.Head) is { } snakePart)
            {
                // If the snakes collided head to head, kill another snake
                if (snakePart.GetType() == typeof(SnakeHeadPoint))
                {
                    Kill(_snakeInformationManager.GetSnakeList.Single(
                        snakeOnTheList => snakeOnTheList.Head == snakePart));
                }

                return true;
            }
            return false;
        }


        // Kill this snake and spawn a new snake
        private void Kill(Snake snake)
        {
            // Add all body points as food
            foreach (var body in snake.BodyPoints)
                _foodInformationManager.Add(new SimpleFood(body));

            _foodInformationManager.Add(new SnakeHeadFood(snake.Head));
            
            _snakeInformationManager.SnakeRespawn(snake);
        }
        
    }
}