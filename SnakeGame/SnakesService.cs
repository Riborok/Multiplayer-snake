using System;
using System.Collections.Generic;

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
            _snakesPointsDict = new Dictionary<(int x, int y), Point>(amount * 150);
            Spawn(amount);
        }

        // Stores the amount of snakes
        private readonly List<Snake> _snakeList;
        public IReadOnlyList<Snake> GetSnakeList => _snakeList;
        
        // Stores a list snake points
        private readonly Dictionary<(int x, int y), Point> _snakesPointsDict;
        public IReadOnlyDictionary<(int x, int y), Point> SnakesPointsDict => _snakesPointsDict;

        // Updates the _snakesPointsDict dictionary with the new positions of the snake's
        public void UpdateSnakePointsDict(SnakeBodyPoint previousTail, SnakeBodyPoint lastBodyPoint, SnakeHeadPoint head)
        {
            _snakesPointsDict[(lastBodyPoint.X, lastBodyPoint.Y)] = lastBodyPoint;
            _snakesPointsDict.Remove((previousTail.X, previousTail.Y));
            _snakesPointsDict[(head.X, head.Y)] = head;
        }
        
        // This method respawn the snake under its id. The id corresponds to the number in the list
        public void Respawn(Snake snake)
        {
            if (!_snakeList.Contains(snake))
                throw new ArgumentException("The provided snake does not exist in the list of snakes.");

            DeleteFromSnakesPointsDict(snake);
            
            _snakeList[snake.Id] = Create(snake.Id);
        }

        // Delete all snake points from _snakePointsDict
        private void DeleteFromSnakesPointsDict(Snake snake)
        {
            foreach (var bodyPoint in snake.BodyPoints)
                _snakesPointsDict.Remove((bodyPoint.X, bodyPoint.Y));     
            _snakesPointsDict.Remove((snake.Head.X, snake.Head.Y)); 
        }
        
        // Spawner in the game amount snakes
        private void Spawn(int amount)
        {
            for (var i = 0; i < amount; i++)
                _snakeList.Add(Create(i));
        }
        
        // Method to create a snake with a generated position and direction,
        // add its head to the points dictionary, and return it.
        private Snake Create(int id)
        {
            var snake = new Snake(Game.Generator.GenerateCoordinates(), Game.Generator.GenerateDirection(),
                color: _colorsForSnakes[id], id: id);
            _snakesPointsDict[(snake.Head.X, snake.Head.Y)] = snake.Head;
            return snake;
        }
    }
}