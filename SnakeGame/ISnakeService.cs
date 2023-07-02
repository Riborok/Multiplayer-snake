using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        
        bool СanDrawSnake { set; }
    }
    
    
    // Service working with snakes
    public class SnakeService : ISnakeService
    {
        public bool СanDrawSnake { get; set; }
        
        // Array of colors that snakes can accept. The number in the array corresponds to the id of the snake 
        private readonly Color[] _colorsForSnakes;
        
        // Game canvas
        private readonly IMapCanvas _mapCanvas;
        
        public SnakeService(IMapCanvas mapCanvas, Color[] colorsForSnakes)
        {
            _mapCanvas = mapCanvas;
            _colorsForSnakes = colorsForSnakes;
            _snakes = new ConcurrentDictionary<int, Snake>();

            СanDrawSnake = true;
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
        
        // Snake spawn period
        private const int SpawnPeriod = 4200;
        
        // Flicker period
        private const int FlickerPeriod = 150;
        
        // Spawn a single snake with a given id
        public async void SpawnSnake(int id)
        {
            await Task.Run(async () =>
            {
                // Create a new snake with the given ID
                var snake = CreateSnake(id);
                
                // Create a new sleeping part for the snake's head
                var sleepingPart = new SleepingPart(snake.Head);

                // Add the sleepingPart to the map
                _mapCanvas.AddToMap(sleepingPart);

                // Create a new attention point based on the sleeping part's coordinates
                var attentionPoint = new AttentionPoint(sleepingPart);

                // Flicker the attention point for a period of time to draw attention to the new snake
                for (var i = 0; i < SpawnPeriod / (FlickerPeriod << 1) && СanDrawSnake; i++)
                {
                    _mapCanvas.DrawPoint(attentionPoint);

                    await Task.Delay(FlickerPeriod);
                    
                    _mapCanvas.DrawPoint(sleepingPart);
                    
                    await Task.Delay(FlickerPeriod);
                }
                
                // Add the new snake to the game
                AddSnake(snake);
            });
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
            return new Snake(Game.Generator.GenerateFreeCoordinates(),
                Direction.None, color: _colorsForSnakes[id], id: id);
        }
    }
}