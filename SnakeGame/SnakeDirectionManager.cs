using System;

namespace SnakeGame
{
    // This is an abstract base class for managing the direction of the snake in the game
    public abstract class SnakeDirectionManager<TKey>
    {
        // It has a protected field MovementKeys, which holds an instance of IMovementKeys interface
        protected readonly IMovementKeys MovementKeys;

        protected SnakeDirectionManager(IMovementKeys iMovementKeys)
        {
            MovementKeys = iMovementKeys;
        }
        
        // This is an abstract method to change the direction of the snake
        // Returns true if the direction was changed, else returns false
        public abstract bool TryChangeDirection(Snake snake, TKey key);
    }

    // This class manages the direction of the snakes in the game
    public class SnakeDirectionManagerForConsole : SnakeDirectionManager<ConsoleKey>
    {
        // Constructor that takes an instance of IMovementKeys to be used by the SnakeDirectionManager
        public SnakeDirectionManagerForConsole(IMovementKeys iMovementKeys) : base(iMovementKeys)
        {
        }

        // Trying to change direction. Returns true if it succeeded, else returns false
        public override bool TryChangeDirection(Snake snake, ConsoleKey key)
        {
            bool result = false;
            switch (snake.Direction)
            {
                case Direction.Left:
                case Direction.Right:
                    if (key == MovementKeys.Down)
                    {
                        snake.Direction = Direction.Down; 
                        result = true;
                    }
                    else if (key == MovementKeys.Up)
                    {
                        snake.Direction = Direction.Up; 
                        result = true;    
                    }
                    break;
                case Direction.Up:
                case Direction.Down:
                    if (key == MovementKeys.Left)
                    {
                        snake.Direction = Direction.Left; 
                        result = true;
                    }
                    else if (key == MovementKeys.Right)
                    {
                        snake.Direction = Direction.Right; 
                        result = true;    
                    }
                    break;
            }
            return result;
        }
    }

    // An interface for movement keys
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