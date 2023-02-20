using System;
using System.Collections.Generic;

namespace SnakeGame
{
    class FoodInformation
    {
        // Singleton
        private FoodInformation() { }
        private static FoodInformation instance;
        public static FoodInformation Get()
        {
            if (instance == null)
            {
                instance = new FoodInformation();
            }
            return instance;
        }
        
        
        private List<Food> foodList = new List<Food>();
        public void Add(Food food)
        {
            foodList.Add(food);
        }

        public List<Food> GetFoodList()
        {
            return foodList;
        }
        
        public void Delete(Food food)
        {
            food.Remove();
            foodList.Remove(food);
            
            Add(new Food());
        }

    }
}