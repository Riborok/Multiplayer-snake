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
        private readonly ICanvas _canvas;
        
        public SnakeService(ICanvas canvas, Color[] colorsForSnakes)
        {
            _canvas = canvas;
            _colorsForSnakes = colorsForSnakes;
        }

        // A list of all snakes in the game
        private readonly List<Snake> _snakeList = new(3);
        public IReadOnlyList<Snake> SnakeList => _snakeList;

        // Update the snake on the canvas with its new position
        public void UpdateSnakeOnCanvas(Snake snake)
        {
            // Add the last body part to the map and draw it
            _canvas.AddToMap(snake.LastBodyPart);
            _canvas.DrawPoint(snake.LastBodyPart);
            
            // Remove the previous tail from the map and clear it
            _canvas.RemoveFromMap(snake.PreviousTail);
            _canvas.ClearPoint(snake.PreviousTail);
            
            // Add the head to the map and draw it
            _canvas.AddToMap(snake.Head);
            _canvas.DrawPoint(snake.Head);
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
            _snakeList.Add(new Snake(Game.Generator.GenerateFreeCoordinates(), 
                Game.Generator.GenerateDirection(), color: _colorsForSnakes[id], id: id));
        }

        // Remove a snake from the list and the canvas
        public void RemoveSnake(Snake snake)
        {
            // Remove the snake from the list and verify that the provided snake is in the list
            if (!_snakeList.Remove(snake))
                throw new ArgumentException("The provided snake does not exist in the list of snakes.");
            
            // Remove all of the snake's body points and head from the canvas
            foreach (var bodyPoint in snake.BodyPoints)
                _canvas.RemoveFromMap(bodyPoint);
            _canvas.RemoveFromMap(snake.Head);
        }
        
    }
}