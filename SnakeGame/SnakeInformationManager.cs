using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Manager working with snake information 
    public class SnakeInformationManager
    {
        // When a manager is created, he will spawn amount snakes
        public SnakeInformationManager(int amount)
        {
            SpawnSnakes(amount);
        }
        
        private readonly List<Snake> _snakeList = new(3);
        public IReadOnlyList<Snake> GetSnakeList => _snakeList;

        // This method returns all parts of all snakes
        public IEnumerable<Point> GetListPointsOfSnakes()
        {
            return _snakeList.SelectMany<Snake, Point>(snake => snake.BodyPoints.Concat<Point>
                (new[] { snake.Head }));
        }

        // This method respawn the snake under its id. The id corresponds to the number in the list
        public void SnakeRespawn(Snake snake)
        {
            if (!_snakeList.Contains(snake))
                throw new ArgumentException("The provided snake does not exist in the list of snakes.");

            _snakeList[snake.Id] =
                new Snake(Generator.GenerateCoordinates(), Generator.GenerateDirection(), snake.Id);
        }
        
        private void SpawnSnakes(int amount)
        {
            for (var i = 0; i < amount; i++)
                _snakeList.Add(new Snake(Generator.GenerateCoordinates(), Generator.GenerateDirection(), id: i));
        }
    }
}