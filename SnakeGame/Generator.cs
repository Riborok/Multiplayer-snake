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
                var randomX = Random.Next(_bordersTuple.LeftBorder + 1, _bordersTuple.RightBorder - 1);
                return (x: randomX % 2 == 1 ? ++randomX : randomX,
                    y: Random.Next(_bordersTuple.UpBorder + 1, _bordersTuple.DownBorder));
            }

            // Generates random coordinates that do not overlap with existing food or snakes
            public static (int x, int y) GenerateFreeCoordinates()
            {
                (int, int) randomCoords;
                
                // Generate new coordinates until they don't overlap with existing food or snakes
                do
                    randomCoords = GenerateCoordinates();
                while (_foodService.FoodDict.ContainsKey(randomCoords) || 
                       _snakesService.SnakesPointsDict.ContainsKey(randomCoords));
                
                return randomCoords;
            }

            // Generates a random direction
            public static Direction GenerateDirection()
            {
                return (Direction)Random.Next(4);
            }
        }
    }
}