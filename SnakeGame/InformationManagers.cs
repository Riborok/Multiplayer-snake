using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    public class FoodInformationManager
    {
        public FoodInformationManager(int amount)
        {
            _foodAmount = amount;
            SpawnSimpleFood(amount);
        }
        
        private readonly int _foodAmount;
        
        private readonly List<Food> _foodList = new(300);
        public IEnumerable<Food> GetFoodList => _foodList;

        private void Add(Food food)
        {
            // Spawn only if there is no food on this position
            if (!_foodList.Any(existingFood => existingFood.IsEquals(food)))
            {
                food.Draw();
                _foodList.Add(food);
            }
        }

        public void AddRange(IEnumerable<Food> foods)
        {
            foreach (var food in foods)
                Add(food);
        }

        public void Remove(Food food)
        {
            food.Remove();
            _foodList.Remove(food);

            while (_foodList.Count < _foodAmount)
                Add(new SimpleFood(Generator.GenerateCoordinates()));
        }

        private void SpawnSimpleFood(int amount)
        {
            for (var i = 0; i < amount; i++)
                Add(new SimpleFood(Generator.GenerateCoordinates()));
        }
    }

    public class SnakeInformationManager
    {
        public SnakeInformationManager(int amount)
        {
            SpawnSnakes(amount);
        }
        
        private readonly List<Snake> _snakeList = new(3);
        public IReadOnlyList<Snake> GetSnakeList => _snakeList;

        public IEnumerable<Point> GetListPointsOfSnakes()
        {
            return _snakeList.SelectMany<Snake, Point>(snake => snake.BodyPoints.Concat<Point>
                (new[] { snake.Head }));
        }

        public void SnakeRespawn(Snake snake)
        {
            if (!_snakeList.Contains(snake))
                throw new ArgumentException("The provided snake does not exist in the list of snakes.");

            _snakeList[snake.Id] =
                new Snake(Generator.GenerateCoordinates(), Generator.GenerateDirection(), snake.Id);
        }
        
        private void SpawnSnakes(int amount)
        {
            for (var i = 0; i < amount; i++)
                _snakeList.Add(new Snake(Generator.GenerateCoordinates(), Generator.GenerateDirection(), id: i));
        }
    }
}