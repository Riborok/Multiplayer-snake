using System;
using System.Collections.Generic;
using System.Linq;

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

    public static class FoodInformation
    {
        private static int _foodAmount;
        
        private static readonly List<Food> FoodList = new(300);
        public static IEnumerable<Food> GetFoodList => FoodList;
        
        public static void Add(Food food)
        {
            FoodList.Add(food);
        }

        public static void Remove(Food food)
        {
            food.Remove();
            FoodList.Remove(food);

            if (FoodList.Count < _foodAmount)
                Add(new SimpleFood(Generator.GenerateCoordinates()));
        }

        public static void SpawnSimpleFood(int amount)
        {
            _foodAmount = amount;
            for (var i = 0; i < amount; i++)
                Add(new SimpleFood(Generator.GenerateCoordinates()));
        }
    }

    public static class SnakeInformation
    {
        private static readonly IMovementKeys[] MovementKeys = { new Arrows(), new Wasd(), new Uhjk() };
        
        private static readonly List<Snake> SnakeList = new(30);
        public static IReadOnlyList<Snake> GetSnakeList => SnakeList;
        
        public static void Add(Snake snake)
        {
            SnakeList.Add(snake);
        }
        
        public static void Remove(Snake snake)
        {
            SnakeList.Remove(snake);
        }

        public static IEnumerable<SnakePart> GetListPartsOfSnakes()
        {
            return SnakeList.SelectMany<Snake, SnakePart>(snake => snake.BodyPoints.Concat<SnakePart>
                (new[] { snake.Head }));
        }

        public static void SpawnSnakes(int amount)
        {
            for (var i = 0; i < amount; i++)
                Add(new Snake(Generator.GenerateCoordinates(), Generator.GenerateDirection(), 
                    MovementKeys[i], id: i));
        }
    }
}