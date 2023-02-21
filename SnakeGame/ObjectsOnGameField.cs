using System.Collections.Generic;


namespace SnakeGame
{
    
        public static class FoodsInformation
        {

            private static List<Food> foodList = new(300);

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

        public static class SnakesInformation
        {
            private static List<Snake> snakeList = new(10);

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
                List<Point> result = new (300);
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
                    FoodsInformation.Add(new Food(point.X, point.Y));
                }
                snakeList.Remove(snake);
            }
            
            
        }
}