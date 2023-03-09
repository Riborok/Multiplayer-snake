using System;

namespace SnakeGame
{
    public static class Generator
    {
        private static readonly Random Random = new();

        public static (int x, int y) GenerateCoordinates()
        {
            // Checking that randomX is in an even position
            var randomX = Random.Next(1, Console.WindowWidth - 1);
            return (x: randomX % 2 == 1 ? ++randomX : randomX, y: Random.Next(1, Console.WindowHeight - 1));
        }

        public static Direction GenerateDirection()
        {
            return (Direction)Random.Next(4);
        }
    }
}