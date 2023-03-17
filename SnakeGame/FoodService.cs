using System;
using System.Collections.Generic;

namespace SnakeGame
{
    // Service working with food information 
    public class FoodService : IObjectDictionary<Food>
    {

        // Stores the amount of food that needs to be maintained on the field
        private int _foodAmount;

        // Generated food is always one color
        private const ConsoleColor ColorForGeneratedSimpleFood = ConsoleColor.Cyan;

        // Stores a list of all food on the field
        private readonly Dictionary<(int x, int y), Food> _foodDict = new(600);
        public IReadOnlyDictionary<(int x, int y), Food> ObjDict => _foodDict;

        // Spawn simple food. This amount of food will be maintained
        public void SpawnSimpleFood(int amount)
        {
            for (var i = 0; i < amount; i++)
                Add(CreateSimpleFood());

            _foodAmount = amount;
        }

        // Processing a snake into food
        public void ProcessIntoFood(Snake snake)
        {
            // Processing a body points into simple food
            foreach (var snakeBodyPoint in snake.BodyPoints)
                Add(new SimpleFood(snakeBodyPoint));

            // Processing a head into sneak head food
            Add(new SnakeHeadFood(snake.Head, snake.BodyPoints.Count));
        }

        // Removes food from the dictionary and if the amount of food on the field is less than
        // the initial, add new simple food
        public void RemoveFromObjDict(Food food)
        {
            _foodDict.Remove(food.Coords);

            if (_foodDict.Count < _foodAmount)
                Add(CreateSimpleFood());
        }

        // Add food to _foodDict and draw on the field
        private void Add(Food food)
        {
            _foodDict[food.Coords] = food;
            food.Draw();
        }

        // This method creates a simple food with randomly coordinates
        private Food CreateSimpleFood()
        {
            // Return SimpleFood with the generated coordinates and a specified color
            return new SimpleFood(Game.Generator.GenerateFreeCoordinates(), ColorForGeneratedSimpleFood);
        }
    }
}