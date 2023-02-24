using System;
using System.Collections.Generic;


namespace SnakeGame
{
    
        public static class FoodsInformation
        {

            private static readonly List<Food> FoodList = new(300);

            public static void Add(Food food)
            {
                FoodList.Add(food);
            }

            public static List<Food> GetFoodList()
            {
                return FoodList;
            }

            public static void Delete(Food food)
            {
                food.Remove();
                FoodList.Remove(food);
                
                // Bad
                Add(new SimpleFood(Generate()));
            }

            public static void Fill(int amount)
            {
                for (int i = 0; i < amount; i++)
                {
                    Add(new SimpleFood(Generate()));
                    System.Threading.Thread.Sleep(1);
                }
            }
            
            // Generate a new position for food
            private static (int x, int y) Generate()
            {
                var random = new Random();

                // Checking that the food is in an even position on the x coordinate
                int randomX = random.Next(2, Console.WindowWidth - 2); 
                return (x: randomX % 2 == 1 ? ++randomX : randomX, y: random.Next(2, Console.WindowHeight - 2));
            }

        }

        public static class SnakesInformation
        {
            private static readonly List<Snake> SnakeList = new(3);

            private static readonly IMovementKeys[] MovementKeys = {new Arrows(), new WASD(), new UHJK()};

            public static void Add(Snake snake)
            {
                SnakeList.Add(snake);
            }

            public static List<Snake> GetSnakeList()
            {
                return SnakeList;
            }

            public static List<SnakePart> GetListPartsOfSnakes()
            {
                List<SnakePart> result = new (300);
                foreach (var snake in SnakeList)
                    result.AddRange(snake.GetPoints());    
                
                return result;
            }

            public static void Dead(Snake snake)
            {
                foreach (var point in snake.GetPoints())
                    FoodsInformation.Add(new SimpleFood(point.X, point.Y));
                
                FoodsInformation.Add(new SnakeHeadFood(snake.Head));

                SnakeList.Remove(snake);
            }

            public static void Fill(int amount)
            {
                for (int i = 0; i < amount; i++)
                    Add(new Snake(Console.WindowWidth / 2 , Console.WindowHeight / 2 - 5 * i, MovementKeys[i]) );
            }
            
        }
}