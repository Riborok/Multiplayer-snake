using System;
using System.Collections.Generic;

namespace SnakeGame
{
    // Interface for managing the direction of the snake in the game
    public interface ISnakeDirectionManager<TKey>
    {
        
        // This is an abstract method to change the direction of the snake
        // Returns true if the direction was changed, else returns false
        bool TryChangeDirection(Snake snake, IMovementKeys<TKey> movementKeys, TKey key);

        // Read the key
        TKey ReadKey();

        // Determines if the key is pressed
        bool IsKeyPress();
    }

    // This class manages the direction of the snakes in the game
    public class SnakeDirectionManagerForConsole : ISnakeDirectionManager<ConsoleKey>
    {
        // Read the console key
        public ConsoleKey ReadKey()
        {
            return Console.ReadKey(true).Key;
        }
        
        // Determines if the console key is pressed
        public bool IsKeyPress()
        {
            return Console.KeyAvailable;
        }

        // This method tries to change the direction of the snake 
        // Returns true if the direction is successfully changed, otherwise return false
        public bool TryChangeDirection(Snake snake, IMovementKeys<ConsoleKey> movementKeys, ConsoleKey key)
        {
            bool result = false;
            if (movementKeys.KeyDirections.TryGetValue(key, out var direction) && 
                snake.Direction != OppositeDirection(direction))
            {
                snake.Direction = direction;
                result = true;
            }
            return result;
        }

        // Gets the opposite direction of the given direction
        private static Direction OppositeDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => Direction.None
            };
        }
    }

    // An interface for movement keys
    public interface IMovementKeys<TKey>
    {
        IReadOnlyDictionary<TKey, Direction> KeyDirections { get; }
    }

    // Struct for movement keys using arrow keys
    public struct ArrowsMovementKey : IMovementKeys<ConsoleKey>
    {
        public ArrowsMovementKey()
        {
        }
        public IReadOnlyDictionary<ConsoleKey, Direction> KeyDirections { get; } = new Dictionary<ConsoleKey, Direction>
        {
            {ConsoleKey.DownArrow, Direction.Down},
            {ConsoleKey.UpArrow, Direction.Up},
            {ConsoleKey.LeftArrow, Direction.Left},
            {ConsoleKey.RightArrow, Direction.Right}
        };
    }
    
    // Struct for movement keys using arrow keys
    public struct WasdMovementKey : IMovementKeys<ConsoleKey>
    {
        public WasdMovementKey()
        {
        }
        public IReadOnlyDictionary<ConsoleKey, Direction> KeyDirections { get; } = new Dictionary<ConsoleKey, Direction>
        {
            {ConsoleKey.S, Direction.Down},
            {ConsoleKey.W, Direction.Up},
            {ConsoleKey.A, Direction.Left},
            {ConsoleKey.D, Direction.Right}
        };
    }
    
    // Struct for movement keys using arrow keys
    public struct UhjkMovementKey : IMovementKeys<ConsoleKey>
    {
        public UhjkMovementKey()
        {
        }
        public IReadOnlyDictionary<ConsoleKey, Direction> KeyDirections { get; } = new Dictionary<ConsoleKey, Direction>
        {
            {ConsoleKey.J, Direction.Down},
            {ConsoleKey.U, Direction.Up},
            {ConsoleKey.H, Direction.Left},
            {ConsoleKey.K, Direction.Right}
        };
    }
}