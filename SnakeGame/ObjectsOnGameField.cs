using System;
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
                
                // Bad
                Add(new SimpleFood());
            }

            static public void Fill(int amount)
            {
                for (int i = 0; i < amount; i++)
                {
                    Add(new SimpleFood());
                    System.Threading.Thread.Sleep(1);
                }
            }

        }

        public static class SnakesInformation
        {
            private static readonly List<Snake> snakeList = new(10);

            private static readonly IMovementKeys[] movementKeys = {new Arrows(), new WASD(), new UHJK()};

            static public void Add(Snake snake)
            {
                snakeList.Add(snake);
            }

            public static List<Snake> GetSnakeList()
            {
                return snakeList;
            }

            public static List<SnakePart> GetListPartsOfSnakes()
            {
                List<SnakePart> result = new (300);
                foreach (var snake in snakeList)
                    result.AddRange(snake.GetPoints());    
                
                return result;
            }

            public static void Dead(Snake snake)
            {
                foreach (var point in snake.GetPoints())
                    FoodsInformation.Add(new SimpleFood(point.X, point.Y));
                
                FoodsInformation.Add(new SnakeHeadFood(snake.head));

                snakeList.Remove(snake);
            }

            public static void Fill(int amount)
            {
                for (int i = 0; i < amount; i++)
                    Add(new Snake(Console.WindowWidth / 2 , Console.WindowHeight / 2 - 5 * i, movementKeys[i]) );
            }
            
        }
}