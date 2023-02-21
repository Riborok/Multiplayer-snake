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

            // Create a snake
            SnakesInformation.Add(new Snake(Console.WindowWidth / 2 , Console.WindowHeight / 2 - 5, new Arrows() ));
            SnakesInformation.Add(new Snake(Console.WindowWidth / 2, Console.WindowHeight / 2 + 5, new WASD() ));
            SnakesInformation.Add(new Snake(Console.WindowWidth / 2, Console.WindowHeight / 2 + 10, new UHJK() ));

            for (int i = 0; i < 150; i++)
            {
                FoodsInformation.Add(new Food()); 
                System.Threading.Thread.Sleep(1);
            }

            // The main game loop
            while (SnakesInformation.GetSnakeList().Count != 0)
            {

                // Processing user input
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    foreach (var snake in SnakesInformation.GetSnakeList())
                    {
                        snake.Turn(key);
                    }
                    
                }

                for (int i = 0; i < SnakesInformation.GetSnakeList().Count; i++)
                {
                    SnakesInformation.GetSnakeList()[i].Move();    
                }

                // Interframe delay
                System.Threading.Thread.Sleep(60);
                Console.CursorVisible = false;
            }

            GameOver();
        }
    }
}
