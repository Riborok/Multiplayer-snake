using System;

namespace SnakeGame
{
    // This is an abstract base class for managing the direction of the snake in the game
    public abstract class SnakeDirectionManager<TKey>
    {
        
        // This is an abstract method to change the direction of the snake
        // Returns true if the direction was changed, else returns false
        public abstract bool TryChangeDirection(Snake snake, IMovementKeys movementKeys, TKey key);

        // Read the key
        public abstract TKey ReadKey();

        // Determines if the key is pressed
        public abstract bool IsKeyPress();
    }

    // This class manages the direction of the snakes in the game
    public class SnakeDirectionManagerForConsole : SnakeDirectionManager<ConsoleKey>
    {
        // Read the console key
        public override ConsoleKey ReadKey()
        {
            return Console.ReadKey(true).Key;
        }
        
        // Determines if the console key is pressed
        public override bool IsKeyPress()
        {
            return Console.KeyAvailable;
        }

        // Trying to change direction. Returns true if it succeeded, else returns false
        public override bool TryChangeDirection(Snake snake, IMovementKeys movementKeys, ConsoleKey key)
        {
            bool result = false;
            switch (snake.Direction)
            {
                case Direction.Left:
                case Direction.Right:
                    if (key == movementKeys.Down)
                    {
                        snake.Direction = Direction.Down; 
                        result = true;
                    }
                    else if (key == movementKeys.Up)
                    {
                        snake.Direction = Direction.Up; 
                        result = true;    
                    }
                    break;
                case Direction.Up:
                case Direction.Down:
                    if (key == movementKeys.Left)
                    {
                        snake.Direction = Direction.Left; 
                        result = true;
                    }
                    else if (key == movementKeys.Right)
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