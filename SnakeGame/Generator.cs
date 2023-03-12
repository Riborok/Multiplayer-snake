using System;

namespace SnakeGame
{
   
    // A static class for generating random values
    public static class Generator
    {
        private static readonly Random Random = new();

        // Generates random x and y coordinates within the console window size. X is always an even number
        public static (int x, int y) GenerateCoordinates()
        {
            // Checking that randomX is in an even position
            var randomX = Random.Next(1, Console.WindowWidth - 1);
            return (x: randomX % 2 == 1 ? ++randomX : randomX, y: Random.Next(1, Console.WindowHeight - 1));
        }

        // Generates a random direction
        public static Direction GenerateDirection()
        {
            return (Direction)Random.Next(4);
        }

        // Generates a random color from the given array
        public static ConsoleColor GenerateColor(ConsoleColor[] colorsForSnakes)
        {
            return colorsForSnakes[Random.Next(colorsForSnakes.Length)];
        }
    }
}