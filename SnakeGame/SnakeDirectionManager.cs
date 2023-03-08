using System;

namespace SnakeGame
{
    public class SnakeDirectionManager 
    {
        private readonly IMovementKeys _iMovementKeys;
        public SnakeDirectionManager(IMovementKeys iMovementKeys)
        {
            _iMovementKeys = iMovementKeys;
        }

        public bool TryChangeDirection(Snake snake, ConsoleKey key)
        {
            switch (snake.Direction)
            {
                case Direction.Left:
                case Direction.Right:
                    if (key == _iMovementKeys.Down)
                    {
                        snake.Direction = Direction.Down; 
                        return true;
                    }
                    if (key == _iMovementKeys.Up)
                    {
                        snake.Direction = Direction.Up; 
                        return true;    
                    }
                    break;
                case Direction.Up:
                case Direction.Down:
                    if (key == _iMovementKeys.Left)
                    {
                        snake.Direction = Direction.Left; 
                        return true;
                    }
                    if (key == _iMovementKeys.Right)
                    {
                        snake.Direction = Direction.Right; 
                        return true;    
                    }
                    break;
            }
            return false;
        }
        
    }
    
    

    public interface IMovementKeys
    {
        ConsoleKey Right { get; }
        ConsoleKey Down { get; }
        ConsoleKey Left { get; }
        ConsoleKey Up { get; }
    }

    public struct Arrows : IMovementKeys
    {
        public ConsoleKey Right  => ConsoleKey.RightArrow;
        public ConsoleKey Down => ConsoleKey.DownArrow;
        public ConsoleKey Left => ConsoleKey.LeftArrow;
        public ConsoleKey Up => ConsoleKey.UpArrow;
    }
    
    public struct Wasd : IMovementKeys
    {
        public ConsoleKey Right  => ConsoleKey.D;
        public ConsoleKey Down => ConsoleKey.S;
        public ConsoleKey Left => ConsoleKey.A;
        public ConsoleKey Up => ConsoleKey.W;
    }
    
    public struct Uhjk : IMovementKeys
    {
        public ConsoleKey Right  => ConsoleKey.K;
        public ConsoleKey Down => ConsoleKey.J;
        public ConsoleKey Left => ConsoleKey.H;
        public ConsoleKey Up => ConsoleKey.U;
    }
    
}