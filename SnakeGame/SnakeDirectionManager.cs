using System;
using System.Collections.Generic;

namespace SnakeGame
{
    // This class manages the direction of the snakes in the game
    public class SnakeDirectionManager 
    {
        private readonly Dictionary<ConsoleKey, Direction> _keyDirections;
        
        // The constructor initializes the dictionary of movement keys and directions
        public SnakeDirectionManager(IMovementKeys iMovementKeys)
        {
            _keyDirections = new Dictionary<ConsoleKey, Direction>
            {
                {iMovementKeys.Down, Direction.Down},
                {iMovementKeys.Up, Direction.Up},
                {iMovementKeys.Left, Direction.Left},
                {iMovementKeys.Right, Direction.Right}
            };
        }

        // This method tries to change the direction of the snake 
        // Returns true if the direction is successfully changed, otherwise return false
        public bool TryChangeDirection(Snake snake, ConsoleKey key)
        {
            if (_keyDirections.TryGetValue(key, out var direction) && snake.Direction != OppositeDirection(direction))
            {
                snake.Direction = direction;
                return true;
            }
            return false;
        }

        private static Direction OppositeDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                _ => throw new ArgumentException("Key handling error.")
            };
        }
        
    }
    
    

    public interface IMovementKeys
    {
        ConsoleKey Right { get; }
        ConsoleKey Down { get; }
        ConsoleKey Left { get; }
        ConsoleKey Up { get; }
    }

    public struct ArrowsMovementKey : IMovementKeys
    {
        public ConsoleKey Right  => ConsoleKey.RightArrow;
        public ConsoleKey Down => ConsoleKey.DownArrow;
        public ConsoleKey Left => ConsoleKey.LeftArrow;
        public ConsoleKey Up => ConsoleKey.UpArrow;
    }
    
    public struct WasdMovementKey : IMovementKeys
    {
        public ConsoleKey Right  => ConsoleKey.D;
        public ConsoleKey Down => ConsoleKey.S;
        public ConsoleKey Left => ConsoleKey.A;
        public ConsoleKey Up => ConsoleKey.W;
    }
    
    public struct UhjkMovementKey : IMovementKeys
    {
        public ConsoleKey Right  => ConsoleKey.K;
        public ConsoleKey Down => ConsoleKey.J;
        public ConsoleKey Left => ConsoleKey.H;
        public ConsoleKey Up => ConsoleKey.U;
    }
    
}