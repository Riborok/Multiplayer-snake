using System;
using System.Collections.Generic;


namespace SnakeGame
{
    
    public static class FoodInformation
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
            FoodList.Remove(food);
            
            // Bad
            if (FoodList.Count < Game.AmountFood)
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
            int randomX = random.Next(1, Console.WindowWidth - 1); 
            return (x: randomX % 2 == 1 ? ++randomX : randomX, y: random.Next(1, Console.WindowHeight - 1));
        }

    }

    public static class SnakeInformation
    {
        private static readonly List<Snake> SnakeList = new(3);

        private static readonly IMovementKeys[] MovementKeys = { new Arrows(), new WASD(), new UHJK() };

        private static void Add(Snake snake)
        {
            SnakeList.Add(snake);
        }

        public static List<Snake> GetSnakeList()
        {
            return SnakeList;
        }

        public static List<SnakePart> GetListPartsOfSnakes()
        {
            List<SnakePart> result = new(300);
            foreach (var snake in SnakeList)
                result.AddRange(snake.GetBodyPoints());

            return result;
        }

        public static void Kill(Snake snake)
        {
            foreach (var body in snake.GetBodyPoints())
                FoodInformation.Add(new SimpleFood(body));
            FoodInformation.Add(new SnakeHeadFood(snake.Head));
            
            SnakeList.Remove(snake);
            Add(new Snake(Generate(), snake.MovementKeys));
        }

        public static void Fill(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Add(new Snake(Generate(), MovementKeys[i]));
                System.Threading.Thread.Sleep(1);
            }
        }

        private static (int x, int y) Generate()
        {
            var random = new Random();

            // Checking that the snake is in an even position on the x coordinate
            int randomX = random.Next(1, Console.WindowWidth - 50); 
            return (x: randomX % 2 == 1 ? ++randomX : randomX, y: random.Next(1, Console.WindowHeight - 1));
        }
    }
}