using System;

namespace SnakeGame
{
    // A partial class of the main Game class, containing a static nested class for generating random values
    public static partial class Game
    {
        // A static class for generating random values
        public static class Generator
        {
            private static readonly Random Random = new();

            // Generates random x and y coordinates within the console window size. X is always an even number
            private static (int x, int y) GenerateCoordinates()
            {
                // Checking that randomX is in an even position
                var randomX = Random.Next(_canvas.WallTuple.LeftWall + 1, _canvas.WallTuple.RightWall - 1);
                return (x: randomX % 2 == 1 ? ++randomX : randomX,
                    y: Random.Next(_canvas.WallTuple.UpWall + 1, _canvas.WallTuple.DownWall));
            }

            // Generates random coordinates that do not overlap with existing food or snakes
            public static (int x, int y) GenerateFreeCoordinates()
            {
                (int x, int y) randomCoords;

                // Generate new coordinates until they don't overlap with existing food or snakes
                do
                    randomCoords = GenerateCoordinates();
                while (_canvas.GetPoint(randomCoords.x, randomCoords.y) != null);

                return randomCoords;
            }
        }
    }
}