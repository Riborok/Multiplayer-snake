using System;

namespace SnakeGame
{
    // The main class of the game
    class Program
    {

        public static void GameOver()
        {
            Console.Clear(); 
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2);
            Console.Write("Game Over");
            
            //Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 + 5);
            //Console.Write($"Your score: ");
            
            System.Threading.Thread.Sleep(10000);
        }

        static void Main(string[] args)
        { 
            
            // Filling the field with food
            const int amountFood = 200;
            FoodsInformation.Fill(amountFood);

            // Create a snake
            const int amountSnaiks = 3;
            SnakesInformation.Fill(amountSnaiks);

            // The main game loop (the game will continue until there is at least 1 snake)
            while (SnakesInformation.GetSnakeList().Count != 0)
            {
                
                // Processing user input
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    foreach (var snake in SnakesInformation.GetSnakeList())
                        snake.Turn(key);
                }

                // Moving snakes
                for (int i = 0; i < SnakesInformation.GetSnakeList().Count; i++)
                    SnakesInformation.GetSnakeList()[i].Move();

                // Interframe delay
                System.Threading.Thread.Sleep(60);
                Console.CursorVisible = false;
            }

            GameOver();
        }
    }
}
