using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // The manager checks the collision of the snake with some object
    public class ObstaclesCollisionManager
    {
        // A reference to the SnakesService, used to manipulate snakes
        private readonly SnakesService _snakesService;
        
        // Tuple with borders of the game field
        private readonly (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) _bordersTuple;
        
        // Constructor for the ObstaclesCollisionManager, requires a SnakesService and the field borders
        public ObstaclesCollisionManager(SnakesService snakesService, 
            (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) bordersTuple)
        {
            _snakesService = snakesService;
            _listOfSnakesToKill = new List<Snake>(_snakesService.GetSnakeList.Count);
            _bordersTuple = bordersTuple;
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
                // If the snake collided with an obstacle or another snake, roll the snake back and add to the list
                snake.Head.CopyCoordinatesFrom(snake.LastBodyPart);
                _listOfSnakesToKill.Add(snake);
                
                result = true;
            }

            return result;
        }
        
        // Check collision with border 
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
            if (_snakesService.SnakesPointsDict.TryGetValue((snake.Head.X, snake.Head.Y), out var snakePart))
            {
                // If the snakes collided head to head, add to the list
                if (snakePart is SnakeHeadPoint)
                    _listOfSnakesToKill.Add(_snakesService.GetSnakeList.Single(
                        snakeOnTheList => snakeOnTheList.Head == snakePart));
                
                result = true;
            }
            
            return result;
        }
    }
}