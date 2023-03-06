using System;
using System.Collections.Generic;

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

        private static readonly List<Food> FoodList = new(300);
        private static int _foodAmount;

        public static void Add(Food food)
        {
            FoodList.Add(food);
        }

        public static List<Food> GetFoodList()
        {
            return new List<Food>(FoodList);
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
        private static readonly List<Snake> SnakeList = new(30);

        private static readonly IMovementKeys[] MovementKeys = { new Arrows(), new Wasd(), new Uhjk() };

        public static void Add(Snake snake)
        {
            SnakeList.Add(snake);
        }
        
        public static void Remove(Snake snake)
        {
            SnakeList.Remove(snake);
        }

        public static List<Snake> GetSnakeList()
        {
            return new List<Snake>(SnakeList);
        }

        public static List<SnakePart> GetListPartsOfSnakes()
        {
            List<SnakePart> result = new(SnakeList.Count * 100);
            foreach (var snake in SnakeList)
            {
                result.AddRange(snake.GetBodyPoints());
                result.Add(snake.Head);
            }

            return result;
        }

        public static void SpawnSnakes(int amount)
        {
            for (var i = 0; i < amount; i++)
                Add(new Snake(Generator.GenerateCoordinates(), Generator.GenerateDirection(), 
                    MovementKeys[i], id: i));
        }
    }
}