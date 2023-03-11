using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Service working with food information 
    public class FoodService
    {
        // When a service is created, he will spawn amount simple food.
        // Further, he always controls that there is this amount on the field
        public FoodService(int amount)
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

        public void AddRange(IEnumerable<Food> foodList)
        {
            foreach (var food in foodList)
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
    
}