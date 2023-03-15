using System;
using System.Collections.Generic;

namespace SnakeGame
{
    // Service working with food information 
    public class FoodService
    {
        // When a service is created, he will spawn amount simple food.
        // Further, he always controls that there is this amount on the field
        public FoodService(int amount, IReadOnlyDictionary<(int x, int y), Point> snakePointsDict)
        {
            _foodAmount = amount;
            _snakePointsDict = snakePointsDict;
            EnsureFoodQuantity();
            _foodDict = new Dictionary<(int x, int y), Food>(amount * 2);
        }

        // Information about snake points to avoid spawning food in them
        private readonly IReadOnlyDictionary<(int x, int y), Point> _snakePointsDict;
        
        // Stores the amount of food that needs to be maintained on the field
        private readonly int _foodAmount;

        // Generated food is always one color
        private const ConsoleColor ColorForGeneratedSimpleFood = ConsoleColor.Cyan;

        // Stores a list of all food on the field
        private readonly Dictionary<(int x, int y), Food> _foodDict;
        public IReadOnlyDictionary<(int x, int y), Food> FoodDict => _foodDict; 

        // Adds food to the field only if there is no other food at this position
        private void Add(Food food)
        {
            if (!_foodDict.ContainsKey((food.X, food.Y)) && !_snakePointsDict.ContainsKey((food.X, food.Y)))
            {
                food.Draw();
                _foodDict.Add((food.X, food.Y), food);
            }
        }

        // Processing a snake into food
        public void ProcessIntoFood(Snake snake)
        {
            Add(new SnakeHeadFood(snake.Head, snake.BodyPoints.Count));
            foreach (var snakeBodyPoint in snake.BodyPoints)
                Add(new SimpleFood(snakeBodyPoint));
        }

        // Removes food from the field and checks whether new food needs to be added to maintain its quantity
        public void Eaten(Food food)
        {
            _foodDict.Remove((food.X, food.Y));

            EnsureFoodQuantity();
        }

        // Controls the amount of food on the field
        private void EnsureFoodQuantity()
        {
            while (_foodDict.Count < _foodAmount)
                Add(new SimpleFood(Game.Generator.GenerateCoordinates(), ColorForGeneratedSimpleFood));   
        }
    }
    
}