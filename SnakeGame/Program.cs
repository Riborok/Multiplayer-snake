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
            SnakeInformation.Add(new Snake(Console.WindowWidth / 2 , Console.WindowHeight / 2 - 5, new Arrows() ));
            SnakeInformation.Add(new Snake(Console.WindowWidth / 2, Console.WindowHeight / 2 + 5, new WASD() ));
            

            for (int i = 0; i < 10; i++)
            {
                FoodInformation.Add(new Food()); 
                System.Threading.Thread.Sleep(50);
            }

            // The main game loop
            while (SnakeInformation.GetSnakeList().Count != 0)
            {

                // Processing user input
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    foreach (var snake in SnakeInformation.GetSnakeList())
                    {
                        snake.Turn(key);
                    }
                    
                }

                for (int i = 0; i < SnakeInformation.GetSnakeList().Count; i++)
                {
                    SnakeInformation.GetSnakeList()[i].Move();    
                }

                // Interframe delay
                System.Threading.Thread.Sleep(80);
                Console.CursorVisible = false;
            }

            GameOver();
        }
    }
}
