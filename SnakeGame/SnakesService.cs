using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Service working with snake information 
    public class SnakesService
    {
        // Array of colors that snakes can accept. The number in the array corresponds to the id of the snake 
        private readonly ConsoleColor[] _colorsForSnakes;
        
        // When a service is created, he will spawn amount snakes
        public SnakesService(int amount, ConsoleColor[] colorsForSnakes)
        {
            _colorsForSnakes = colorsForSnakes;
            _snakeList = new List<Snake>(amount);
            Spawn(amount);
        }
        
        // Stores the amount of snakes
        private readonly List<Snake> _snakeList;
        public IReadOnlyList<Snake> GetSnakeList => _snakeList;

        // This method returns all parts of all snakes
        public IEnumerable<Point> GetListPointsOfSnakes()
        {
            return _snakeList.SelectMany<Snake, Point>(snake => snake.BodyPoints.Concat<Point>
                (new[] { snake.Head }));
        }

        // This method respawn the snake under its id. The id corresponds to the number in the list
        public void Respawn(Snake snake)
        {
            if (!_snakeList.Contains(snake))
                throw new ArgumentException("The provided snake does not exist in the list of snakes.");

            _snakeList[snake.Id] =
                new Snake(Game.Generator.GenerateCoordinates(), Game.Generator.GenerateDirection(),
                    color: _colorsForSnakes[snake.Id], id: snake.Id);
        }
        
        // Spawner in the game amount snakes
        private void Spawn(int amount)
        {
            for (var i = 0; i < amount; i++)
                _snakeList.Add(
                        new Snake(Game.Generator.GenerateCoordinates(), Game.Generator.GenerateDirection(),
                        color: _colorsForSnakes[i], id: i));
        }
    }
}