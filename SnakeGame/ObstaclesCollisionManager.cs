using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // The manager checks the collision of the snake with some object
    public class ObstaclesCollisionManager
    {
        // Store the map to check for collisions
        private readonly IPointMap _pointMap;

        // List of snakes on the map
        private readonly IEnumerable<Snake> _snakes;
        
        public ObstaclesCollisionManager(IEnumerable<Snake> snakes, IPointMap pointMap)
        {
            _snakes = snakes;
            _pointMap = pointMap;
            _listOfSnakesToKill = new List<Snake>(_snakes.Count());
        }

        // List of snakes to kill
        private readonly List<Snake> _listOfSnakesToKill;
        public IEnumerable<Snake> ListOfSnakesToKill => _listOfSnakesToKill;
        
        // Clear the list _listOfSnakesToKill
        public void ClearListOfSnakesToKill()
        {
            _listOfSnakesToKill.Clear();
        }

        // Check for collision with objects 
        public bool HasCollisionOccurred(Snake snake)
        {
            bool result = false;

            // Check for collision with an obstacle or another snake (and with their parts)
            if (CheckCollisionWithBorder(snake) || CheckCollisionWithPartsOfSnakes(snake))
            {
                // If the snake collided with an obstacle or another snake,
                // roll back the snake head and add to the list
                snake.Head.X = snake.LastBodyPart.X;
                snake.Head.Y = snake.LastBodyPart.Y;
                _listOfSnakesToKill.Add(snake);
                
                result = true;
            }

            return result;
        }
        
        // Check collision with border 
        private bool CheckCollisionWithBorder(Snake snake)
        {
            return snake.Head.X <= _pointMap.WallTuple.LeftWall || 
                   snake.Head.X >= _pointMap.WallTuple.RightWall || 
                   snake.Head.Y <= _pointMap.WallTuple.UpWall || 
                   snake.Head.Y >= _pointMap.WallTuple.DownWall;
        }
        
        // Check collision with parts of snakes
        private bool CheckCollisionWithPartsOfSnakes(Snake snake)
        {
            bool result = false;
            
            // Checking for collisions with other parts of the snakes and own parts
            if (_pointMap.GetMap[snake.Head.X, snake.Head.Y] is SnakePart collidingFood)
            {
                // If the snakes collided head to head, add to the list
                if (_pointMap.GetMap[snake.Head.X, snake.Head.Y] is SnakeHeadFood)
                    _listOfSnakesToKill.Add(_snakes.Single(snakeOnTheList => snakeOnTheList.Head == collidingFood));
                
                result = true;
            }

            return result;
        }
    }
}