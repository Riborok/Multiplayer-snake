using System;

namespace SnakeGame
{
    // The main class of the game
    class Program
    {
        private static bool isNotGameOver = true;
        public static void GameOver(Snake snake)
        {
            Console.Clear(); 
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2);
            Console.Write("Game Over");
            
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 + 5);
            Console.Write($"Your score: {snake.GetScore()}");

            isNotGameOver = false;
            Console.ReadKey();
        }

        static void Main(string[] args)
        {

            // Create a snake
            Snake snake = new Snake(Console.WindowWidth / 2, Console.WindowHeight / 2);
            
            // Food on the game field
            FoodInformation foodInformation = FoodInformation.Get();

            for (int i = 0; i < 5; i++)
            {
                foodInformation.Add(new Food()); 
                System.Threading.Thread.Sleep(50);
            }

            // The main game loop
            while (isNotGameOver)
            {

                // Processing user input
                if (Console.KeyAvailable)
                {
                    snake.Turn(Console.ReadKey().Key);
                }

                // Moving the snake
                snake.Move();

                // Interframe delay
                System.Threading.Thread.Sleep(80);
                Console.CursorVisible = false;
            }
        }
    }
}
