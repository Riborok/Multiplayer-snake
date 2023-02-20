using System;
using System.Collections.Generic;
using System.Configuration;

namespace SnakeGame
{
    
        public static class FoodInformation
        {

            private static List<Food> foodList = new List<Food>();

            static public void Add(Food food)
            {
                foodList.Add(food);
            }

            public static List<Food> GetFoodList()
            {
                return foodList;
            }

            static public void Delete(Food food)
            {
                food.Remove();
                foodList.Remove(food);

                Add(new Food());
            }

        }

        public static class SnakeInformation
        {
            private static List<Snake> snakeList = new List<Snake>();

            static public void Add(Snake snake)
            {
                snakeList.Add(snake);
            }

            public static List<Snake> GetSnakeList()
            {
                return snakeList;
            }

            public static List<Point> GetSnakePoints()
            {
                List<Point> result = new List<Point>();
                foreach (var snake in snakeList)
                {
                    result.AddRange(snake.GetPoints());    
                }

                return result;
            }

            static public void Dead(Snake snake)
            {
                foreach (var point in snake.GetPoints())
                {
                    FoodInformation.Add(new Food(point.X, point.Y));
                }
                snakeList.Remove(snake);
            }
            
            
        }
}