using System;

namespace SnakeGame
{

    public interface IMovementKeys
    {
        Direction? MovementDirection(ConsoleKey key);
    }

    public class Arrows : IMovementKeys
    {
        public Direction? MovementDirection(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.RightArrow:
                    return Direction.Right;
                case ConsoleKey.DownArrow:
                    return Direction.Down;
                case ConsoleKey.LeftArrow:
                    return Direction.Left;
                case ConsoleKey.UpArrow:
                    return Direction.Up;
                default:
                    return null;
            }
        }

    }

    public class WASD : IMovementKeys
    {
        public Direction? MovementDirection(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D:
                    return Direction.Right;
                case ConsoleKey.S:
                    return Direction.Down;
                case ConsoleKey.A:
                    return Direction.Left;
                case ConsoleKey.W:
                    return Direction.Up;
                default:
                    return null;
            }
        }
    }

    public class UHJK : IMovementKeys
    {
        public Direction? MovementDirection(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.K:
                    return Direction.Right;
                case ConsoleKey.J:
                    return Direction.Down;
                case ConsoleKey.H:
                    return Direction.Left;
                case ConsoleKey.U:
                    return Direction.Up;
                default:
                    return null;
            }
        }
    }
}