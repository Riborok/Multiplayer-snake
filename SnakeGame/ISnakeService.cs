using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SnakeGame
{
    // Interface for managing snakes in the game
    public interface ISnakeService
    {
        // A read-only dictionary of snakes
        IReadOnlyDictionary<int, Snake> Snakes { get; }
        
        // Update the snake on the canvas with its new position
        void UpdateSnakeOnCanvas(Snake snake);
        
        // Spawn multiple snakes
        void SpawnSnakes(int amount);
        
        // Spawn a single snake with a given id
        void SpawnSnake(int id);
        
        // Remove a snake from the dictionary and the canvas
        void RemoveSnake(Snake snake);
    }
    
    
    // Service working with snakes
    public class SnakeService : ISnakeService
    {
        // Array of colors that snakes can accept. The number in the array corresponds to the id of the snake 
        private readonly Color[] _colorsForSnakes;
        
        // Game canvas
        private readonly IMapCanvas _mapCanvas;
        
        public SnakeService(IMapCanvas mapCanvas, Color[] colorsForSnakes)
        {
            _mapCanvas = mapCanvas;
            _colorsForSnakes = colorsForSnakes;
            _snakes = new ConcurrentDictionary<int, Snake>();
        }

        // Hash table of snakes. The key is the snake's id
        private readonly ConcurrentDictionary<int, Snake> _snakes;
        public IReadOnlyDictionary<int, Snake> Snakes => _snakes;

        // Update the snake on the canvas with its new position
        public void UpdateSnakeOnCanvas(Snake snake)
        {
            // Add the last body part to the map and draw it
            _mapCanvas.AddToMap(snake.LastBodyPart);
            _mapCanvas.DrawPoint(snake.LastBodyPart);
            
            // Remove the previous tail from the map and clear it
            _mapCanvas.RemoveFromMap(snake.PreviousTail);
            _mapCanvas.ClearPoint(snake.PreviousTail);
            
            // Add the head to the map and draw it
            _mapCanvas.AddToMap(snake.Head);
            _mapCanvas.DrawPoint(snake.Head);
        }
        
        // Spawn multiple snakes
        public void SpawnSnakes(int amount)
        {
            for (var i = 0; i < amount; i++)
                SpawnSnake(i);
        }
        
        // Spawn a single snake with a given id
        public void SpawnSnake(int id)
        {
            var timer = new System.Timers.Timer(4200);
            timer.Elapsed += (_,_) => AddSnake(CreateSnake(id));
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        // Add the snake to the dictionary
        private void AddSnake(Snake snake)
        {
            _snakes.TryAdd(snake.Id, snake);
        }

        // Remove a snake from the dictionary and the canvas
        public void RemoveSnake(Snake snake)
        {
            // Remove the snake from the dictionary and verify that the provided snake is in the dictionary
            if (!_snakes.TryRemove(snake.Id, out _))
                throw new ArgumentException("The provided snake does not exist in the dictionary of snakes.");
            
            // Remove all of the snake's body points and head from the canvas
            foreach (var bodyPoint in snake.BodyPoints)
                _mapCanvas.RemoveFromMap(bodyPoint);
            _mapCanvas.RemoveFromMap(snake.Head);
        }

        // Create a snake on the canvas and the map
        private Snake CreateSnake(int id)
        {
            var snake = new Snake(Game.Generator.GenerateFreeCoordinates(),
                Direction.None, color: _colorsForSnakes[id], id: id);

            var sleepingPart = new SleepingPart(snake.Head);

            // Add the sleepingPart to the map and draw it
            _mapCanvas.AddToMap(sleepingPart);
            _mapCanvas.DrawPoint(sleepingPart);
            
            return snake;
        }
    }
}