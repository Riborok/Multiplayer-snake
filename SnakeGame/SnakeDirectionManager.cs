using System;

namespace SnakeGame
{
    // This class manages the direction of the snakes in the game
    public class SnakeDirectionManager 
    {
        private readonly IMovementKeys _iMovementKeys;
        
        // Constructor that takes an instance of IMovementKeys to be used by the SnakeDirectionManager
        public SnakeDirectionManager(IMovementKeys iMovementKeys)
        {
            _iMovementKeys = iMovementKeys;
        }

        // Trying to change direction. Returns true if it succeeded, else returns false
        public bool TryChangeDirection(Snake snake, ConsoleKey key)
        {
            bool result = false;
            switch (snake.Direction)
            {
                case Direction.Left:
                case Direction.Right:
                    if (key == _iMovementKeys.Down)
                    {
                        snake.Direction = Direction.Down; 
                        result = true;
                    }
                    else if (key == _iMovementKeys.Up)
                    {
                        snake.Direction = Direction.Up; 
                        result = true;    
                    }
                    break;
                case Direction.Up:
                case Direction.Down:
                    if (key == _iMovementKeys.Left)
                    {
                        snake.Direction = Direction.Left; 
                        result = true;
                    }
                    else if (key == _iMovementKeys.Right)
                    {
                        snake.Direction = Direction.Right; 
                        result = true;    
                    }
                    break;
            }
            return result;
        }
    }

    // Interface for movement keys
    public interface IMovementKeys
    {
        ConsoleKey Right { get; }
        ConsoleKey Down { get; }
        ConsoleKey Left { get; }
        ConsoleKey Up { get; }
    }

    // Struct for movement keys using arrow keys
    public struct ArrowsMovementKey : IMovementKeys
    {
        public ConsoleKey Right  => ConsoleKey.RightArrow;
        public ConsoleKey Down => ConsoleKey.DownArrow;
        public ConsoleKey Left => ConsoleKey.LeftArrow;
        public ConsoleKey Up => ConsoleKey.UpArrow;
    }
    
    // Struct for movement keys using arrow keys
    public struct WasdMovementKey : IMovementKeys
    {
        public ConsoleKey Right  => ConsoleKey.D;
        public ConsoleKey Down => ConsoleKey.S;
        public ConsoleKey Left => ConsoleKey.A;
        public ConsoleKey Up => ConsoleKey.W;
    }
    
    // Struct for movement keys using arrow keys
    public struct UhjkMovementKey : IMovementKeys
    {
        public ConsoleKey Right  => ConsoleKey.K;
        public ConsoleKey Down => ConsoleKey.J;
        public ConsoleKey Left => ConsoleKey.H;
        public ConsoleKey Up => ConsoleKey.U;
    }
    
}