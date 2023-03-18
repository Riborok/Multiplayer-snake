using System;
using System.Collections.Generic;

namespace SnakeGame
{
    // Interface for managing snakes in the game
    public interface ISnakeService
    {
        // A read-only list of snakes
        public IReadOnlyList<Snake> SnakeList { get; }
        
        // Update the snake on the canvas with its new position
        void UpdateSnakeOnCanvas(Snake snake);
        
        // Spawn multiple snakes
        void SpawnSnakes(int amount);
        
        // Spawn a single snake with a given id
        void SpawnSnake(int id);
        
        // Remove a snake from the list and the canvas
        void RemoveSnake(Snake snake);
    }
    
    
    // Service working with snakes
    public class SnakeService : ISnakeService
    {
        // Array of colors that snakes can accept. The number in the array corresponds to the id of the snake 
        private readonly Color[] _colorsForSnakes;
        
        // Game canvas
        private readonly IPointMapCanvas _pointMapCanvas;
        
        public SnakeService(IPointMapCanvas pointMapCanvas, Color[] colorsForSnakes)
        {
            _pointMapCanvas = pointMapCanvas;
            _colorsForSnakes = colorsForSnakes;
        }

        // A list of all snakes in the game
        private readonly List<Snake> _snakeList = new(3);
        public IReadOnlyList<Snake> SnakeList => _snakeList;

        // Update the snake on the canvas with its new position
        public void UpdateSnakeOnCanvas(Snake snake)
        {
            // Add the last body part to the map and draw it
            _pointMapCanvas.AddToMap(snake.LastBodyPart);
            _pointMapCanvas.DrawPoint(snake.LastBodyPart);
            
            // Remove the previous tail from the map and clear it
            _pointMapCanvas.RemoveFromMap(snake.PreviousTail);
            _pointMapCanvas.ClearPoint(snake.PreviousTail);
            
            // Add the head to the map and draw it
            _pointMapCanvas.AddToMap(snake.Head);
            _pointMapCanvas.DrawPoint(snake.Head);
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
            var snake = CreateSnake(id);
            
            var timer = new System.Timers.Timer(5000);
            timer.Elapsed += (_,_) => _snakeList.Add(snake);
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        // Remove a snake from the list and the canvas
        public void RemoveSnake(Snake snake)
        {
            // Remove the snake from the list and verify that the provided snake is in the list
            if (!_snakeList.Remove(snake))
                throw new ArgumentException("The provided snake does not exist in the list of snakes.");
            
            // Remove all of the snake's body points and head from the canvas
            foreach (var bodyPoint in snake.BodyPoints)
                _pointMapCanvas.RemoveFromMap(bodyPoint);
            _pointMapCanvas.RemoveFromMap(snake.Head);
        }

        // Create a snake on the canvas and the map
        private Snake CreateSnake(int id)
        {
            var snake = new Snake(Game.Generator.GenerateFreeCoordinates(),
                Game.Generator.GenerateDirection(), color: _colorsForSnakes[id], id: id);

            var sleepingPart = new SleepingPart(snake.Head);

            // Add the sleepingPart to the map and draw it
            _pointMapCanvas.AddToMap(sleepingPart);
            _pointMapCanvas.DrawPoint(sleepingPart);
            
            return snake;
        }
        
        
    }
}