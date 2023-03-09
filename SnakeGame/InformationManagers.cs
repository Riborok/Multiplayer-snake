using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    public static class FoodInformationManager
    {
        private static int _foodAmount;
        
        private static readonly List<Food> FoodList = new(300);
        public static IEnumerable<Food> GetFoodList => FoodList;
        
        public static void Add(Food food)
        {
            if (!FoodList.Any(existingFood => existingFood.IsEquals(food)))
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

    public static class SnakeInformationManager
    {
        private static readonly List<Snake> SnakeList = new(30);
        public static IReadOnlyList<Snake> GetSnakeList => SnakeList;

        public static IEnumerable<Point> GetListSnakesPoints()
        {
            return SnakeList.SelectMany<Snake, Point>(snake => snake.BodyPoints.Concat<Point>
                (new[] { snake.Head }));
        }

        public static void SpawnSnakes(int amount)
        {
            for (var i = 0; i < amount; i++)
                SnakeList.Add(new Snake(Generator.GenerateCoordinates(), Generator.GenerateDirection(), id: i));
        }

        public static void SnakeRespawn(Snake snake)
        {
            if (!SnakeList.Contains(snake))
                throw new ArgumentException("The provided snake does not exist in the list of snakes.");
            
            
            SnakeList.Remove(snake);
            SnakeList.Add(new Snake(Generator.GenerateCoordinates(), Generator.GenerateDirection(), snake.Id));

        }
    }
}