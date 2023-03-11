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
        
        // Stores the amount of food that needs to be maintained on the field
        private readonly int _foodAmount;
        
        // Stores a list of all food on the field.
        private readonly List<Food> _foodList = new(300);
        public IEnumerable<Food> GetFoodList => _foodList;

        // Adds food to the field only if there is no other food at this position
        private void Add(Food food)
        {
            if (!_foodList.Any(existingFood => existingFood.IsEquals(food)))
            {
                food.Draw();
                _foodList.Add(food);
            }
        }

        // Adds multiple pieces of food to the field
        public void AddRange(IEnumerable<Food> foodList)
        {
            foreach (var food in foodList)
                Add(food);
        }

        // Removes food from the field and checks whether new food needs to be added to maintain its quantity
        public void Eaten(Food food)
        {
            _foodList.Remove(food);

            ControlFoodAmount();
        }

        // Controls the amount of food on the field
        private void ControlFoodAmount()
        {
            while (_foodList.Count < _foodAmount)
                Add(new SimpleFood(Generator.GenerateCoordinates()));   
        }

        // Generates and adds the specified amount of simple food to the field
        private void SpawnSimpleFood(int amount)
        {
            for (var i = 0; i < amount; i++)
                Add(new SimpleFood(Generator.GenerateCoordinates()));
        }
    }
    
}