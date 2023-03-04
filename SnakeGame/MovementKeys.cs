using System;

namespace SnakeGame
{
    
    public interface IMovementKeys
    {
        Direction MovementDirection(ConsoleKey key, Direction direction);
    }

    public class Arrows : IMovementKeys
    {
        public Direction MovementDirection(ConsoleKey key, Direction direction)
        {
            switch (key)
            {
                case ConsoleKey.RightArrow:
                    if (direction != Direction.Left)
                        return Direction.Right;
                    break;
                case ConsoleKey.DownArrow:
                    if (direction != Direction.Up)
                        return Direction.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    if (direction != Direction.Right)
                        return Direction.Left;
                    break;
                case ConsoleKey.UpArrow:
                    if (direction != Direction.Down)
                        return Direction.Up;
                    break;
            }
            return direction;    
        }

    }

    public class Wasd : IMovementKeys
    {
        public Direction MovementDirection(ConsoleKey key, Direction direction)
        {
            switch (key)
            {
                case ConsoleKey.D:
                    if (direction != Direction.Left)
                        return Direction.Right;
                    break;
                case ConsoleKey.S:
                    if (direction != Direction.Up)
                        return Direction.Down;
                    break;
                case ConsoleKey.A:
                    if (direction != Direction.Right)
                        return Direction.Left;
                    break;
                case ConsoleKey.W:
                    if (direction != Direction.Down)
                        return Direction.Up;
                    break;
            }
            return direction;
        }
    }

    public class Uhjk : IMovementKeys
    {
        public Direction MovementDirection(ConsoleKey key, Direction direction)
        {
            switch (key)
            {
                case ConsoleKey.K:
                    if (direction != Direction.Left)
                        return Direction.Right;
                    break;
                case ConsoleKey.J:
                    if (direction != Direction.Up)
                        return Direction.Down;
                    break;
                case ConsoleKey.H:
                    if (direction != Direction.Right)
                        return Direction.Left;
                    break;
                case ConsoleKey.U:
                    if (direction != Direction.Down)
                        return Direction.Up;
                    break;
            }
            return direction;
        }
    }
    
}