using System;
using System.Collections.Generic;

namespace SnakeGame
{
    // Service working with snake information 
    public class SnakesService : IComplexObjectList<Snake> 
    {
        // Array of colors that snakes can accept. The number in the array corresponds to the id of the snake 
        private readonly ConsoleColor[] _colorsForSnakes;
        
        public SnakesService(ConsoleColor[] colorsForSnakes)
        {
            _colorsForSnakes = colorsForSnakes;
        }

        // Stores the amount of snakes
        private readonly List<Snake> _snakeList = new(3);
        public IReadOnlyList<Snake> ComplexObjList => _snakeList;
        
        // Stores a list snake points
        private readonly Dictionary<(int x, int y), Point> _snakesPointsDict = new (600);
        public IReadOnlyDictionary<(int x, int y), Point> ObjDict => _snakesPointsDict;

        // Updates the _snakesPointsDict dictionary with the new positions of the snake's
        public void UpdateSnakePointsDict(SnakeBodyPoint previousTail, SnakeBodyPoint lastBodyPoint, SnakeHeadPoint head)
        {
            _snakesPointsDict[lastBodyPoint.Coords] = lastBodyPoint;
            _snakesPointsDict.Remove(previousTail.Coords);
            _snakesPointsDict[head.Coords] = head;
        }
        
        // Spawn snakes
        public void SpawnSnakes(int amount)
        {
            for (var i = 0; i < amount; i++)
                SpawnSnake(i);
        }
        
        // Spawn a snake
        public void SpawnSnake(int id)
        {
            _snakeList.Add(Create(id));
        }

        // Remove the snake from the list
        public void RemoveFromComplexObjList(Snake snake)
        {
            // Remove the snake from the list and verify that the provided snake is in the list
            if (!_snakeList.Remove(snake))
                throw new ArgumentException("The provided snake does not exist in the list of snakes.");
            
            // Remove all of the snake's body points and head from the dictionary
            foreach (var bodyPoint in snake.BodyPoints)
                RemoveFromObjDict(bodyPoint);
            RemoveFromObjDict(snake.Head);
        }

        // Remove point from the dictionary
        public void RemoveFromObjDict(Point point)
        {
            _snakesPointsDict.Remove(point.Coords);
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
            _snakesPointsDict[snake.Head.Coords] = snake.Head;
            
            // Return the created snake
            return snake;
        }
    }
}