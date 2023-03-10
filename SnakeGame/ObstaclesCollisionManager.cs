using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // The manager checks the collision of the snake with some object
    public class ObstaclesCollisionManager
    {
        private readonly SnakeInformationManager _snakeInformationManager;
        public ObstaclesCollisionManager(SnakeInformationManager snakeInformationManager)
        {
            _snakeInformationManager = snakeInformationManager;
        }

        // List of snakes to kill
        private readonly List<Snake> _snakesToKill = new(2);
        public IEnumerable<Snake> SnakesToKill => _snakesToKill;

        // Check for collision with objects 
        public bool IsCollisionOccured(Snake snake)
        {
            bool result = false;
            
            // Clear the list
            _snakesToKill.Clear();
            
            // Check for collision with an obstacle or another snake (and with their parts)
            if (CheckCollisionWithBorder(snake) || CheckCollisionWithPartsOfSnakes(snake))
            {
                // If the snake collided with an obstacle or another snake, roll the snake back
                snake.Head.CopyCoordinatesFrom(snake.LastBodyPart);

                _snakesToKill.Add(snake);
                result = true;
            }

            return result;
        }
        
        // Check collision with obstacles 
        private bool CheckCollisionWithBorder(Snake snake)
        {
            return snake.Head.X < 1 || snake.Head.X > Console.WindowWidth - 1 || 
                   snake.Head.Y < 1 || snake.Head.Y > Console.WindowHeight - 2;
        }
        
        // Check collision with parts of snakes
        private bool CheckCollisionWithPartsOfSnakes(Snake snake)
        {
            bool result = false;
            
            // Checking for collisions with other parts of the snakes and own parts (except head)    
            if (_snakeInformationManager.GetListPointsOfSnakes().FirstOrDefault(point => point.IsEquals(snake.Head)
                 && point != snake.Head) is { } snakePart)
            {
                // If the snakes collided head to head, kill another snake
                if (snakePart is SnakeHeadPoint)
                {
                    _snakesToKill.Add(_snakeInformationManager.GetSnakeList.Single(
                        snakeOnTheList => snakeOnTheList.Head == snakePart));
                }

                result = true;
            }
            
            return result;
        }
        
    }
}