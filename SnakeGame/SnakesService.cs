using System;
using System.Collections.Generic;

namespace SnakeGame
{
    // Service working with snake information 
    public class SnakesService
    {
        // Array of colors that snakes can accept. The number in the array corresponds to the id of the snake 
        private readonly ConsoleColor[] _colorsForSnakes;
        
        public SnakesService(ConsoleColor[] colorsForSnakes)
        {
            _colorsForSnakes = colorsForSnakes;
        }

        // Stores the amount of snakes
        private readonly List<Snake> _snakeList = new(3);
        public IReadOnlyList<Snake> GetSnakeList => _snakeList;
        
        // Stores a list snake points
        private readonly Dictionary<(int x, int y), Point> _snakesPointsDict = new (600);
        public IReadOnlyDictionary<(int x, int y), Point> SnakesPointsDict => _snakesPointsDict;

        // Updates the _snakesPointsDict dictionary with the new positions of the snake's
        public void UpdateSnakePointsDict(SnakeBodyPoint previousTail, SnakeBodyPoint lastBodyPoint, SnakeHeadPoint head)
        {
            _snakesPointsDict[(lastBodyPoint.X, lastBodyPoint.Y)] = lastBodyPoint;
            _snakesPointsDict.Remove((previousTail.X, previousTail.Y));
            _snakesPointsDict[(head.X, head.Y)] = head;
        }
        
        // Spawn snakes
        public void Spawn(int amount)
        {
            for (var i = 0; i < amount; i++)
                _snakeList.Add(Create(i));
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

        // Method to create a snake with a generated position and direction,
        // add its head to the points dictionary, and return it.
        private Snake Create(int id)
        {
            // Create the new snake with the randomly generated coordinates, a random direction,
            // a specific color, and the specified ID
            var snake = new Snake(Game.Generator.GenerateFreeCoordinates(), Game.Generator.GenerateDirection(),
                color: _colorsForSnakes[id], id: id);
            
            // Add the head of the new snake to the dictionary of snake points
            _snakesPointsDict[(snake.Head.X, snake.Head.Y)] = snake.Head;
            
            // Return the created snake
            return snake;
        }
    }
}