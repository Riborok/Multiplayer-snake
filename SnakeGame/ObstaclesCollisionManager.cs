using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // The manager checks the collision of the snake with some object
    public class ObstaclesCollisionManager
    {
        private readonly SnakesService _snakesService;
        private readonly (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) _bordersTuple;
        public ObstaclesCollisionManager(SnakesService snakesService, 
            (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) bordersTuple)
        {
            _snakesService = snakesService;
            _bordersTuple = bordersTuple;
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
            return snake.Head.X <= _bordersTuple.LeftBorder || snake.Head.X >= _bordersTuple.RightBorder || 
                   snake.Head.Y <= _bordersTuple.UpBorder || snake.Head.Y >= _bordersTuple.DownBorder;
        }
        
        // Check collision with parts of snakes
        private bool CheckCollisionWithPartsOfSnakes(Snake snake)
        {
            bool result = false;
            
            // Checking for collisions with other parts of the snakes and own parts (except head)    
            if (_snakesService.GetListPointsOfSnakes().FirstOrDefault(point => point.IsEquals(snake.Head)
                 && point != snake.Head) is { } snakePart)
            {
                // If the snakes collided head to head, kill another snake
                if (snakePart is SnakeHeadPoint)
                {
                    _snakesToKill.Add(_snakesService.GetSnakeList.Single(
                        snakeOnTheList => snakeOnTheList.Head == snakePart));
                }

                result = true;
            }
            
            return result;
        }
        
    }
}