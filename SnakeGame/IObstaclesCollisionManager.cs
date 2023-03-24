using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Interface for checking collisions between snake and obstacles
    public interface IObstaclesCollisionManager
    {
        // Get the collection of snakes that collided with obstacles and should be killed
        IEnumerable<Snake> SnakesToKill { get; }

        // Clear the list of snakes to kill
        void ClearListOfSnakesToKill();

        // Checks if a collision occurred for the given snake and handles it
        // returns true if a collision occurred, false otherwise. Also adds to snakes to kill
        bool HasCollisionOccurred(Snake snake);
    }
    
    // The manager checks the collision of the snake with some object
    public class ObstaclesCollisionManager : IObstaclesCollisionManager
    {
        // Store the map to check for collisions
        private readonly IPointMap _pointMap;

        // Enumeration of snakes on the map
        private readonly IReadOnlyDictionary<int, Snake> _snakes;
        
        public ObstaclesCollisionManager(IReadOnlyDictionary<int, Snake> snakes, IPointMap pointMap)
        {
            _snakes = snakes;
            _pointMap = pointMap;
            
            _snakesToKill = new List<Snake>();
        }

        // The list of snakes to kill
        private readonly List<Snake> _snakesToKill;
        public IEnumerable<Snake> SnakesToKill => _snakesToKill;
        
        // Clear the list of snakes to kill
        public void ClearListOfSnakesToKill()
        {
            _snakesToKill.Clear();
        }

        // Check for collision with objects 
        public bool HasCollisionOccurred(Snake snake)
        {
            bool result = false;

            // Check for collision with an obstacle or another snake (and with their parts)
            if (CheckCollisionWithBorder(snake) || CheckCollisionWithPartsOfSnakes(snake))
            {
                // If the snake collided with an obstacle or another snake,
                // roll back the snake head and add to the snakes to kill
                snake.Head.X = snake.LastBodyPart.X;
                snake.Head.Y = snake.LastBodyPart.Y;
                _snakesToKill.Add(snake);
                
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
            if (_pointMap.GetPoint(snake.Head.X, snake.Head.Y) is SnakePart snakePart)
            {
                // If the snakes collided head to head, add to the snakes to kill
                if (snakePart is SnakeHeadPoint)
                    _snakesToKill.Add(_snakes.Values.Single(otherSnake => otherSnake.Head == snakePart));
                
                result = true;
            }

            return result;
        }
    }
}