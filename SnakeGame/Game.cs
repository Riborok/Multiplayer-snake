using System;
using System.Runtime.InteropServices;

namespace SnakeGame
{
    // The main class of the game
    class Game
    {
        // WinAPI function
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static void SetConsoleSettings()
        {
            // Color setting
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            // Console window setting
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, 3);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            
            Console.CursorVisible = false;   
        }

        public static void GameOver()
        {
            Console.Clear(); 
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2);
            Console.Write("Game Over");
            
            //Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 + 5);
            //Console.Write($"Your score: ");
            
            System.Threading.Thread.Sleep(10000);
        }

        static void Main()
        {
            SetConsoleSettings();

            Console.SetCursorPosition(Console.WindowWidth / 2 - 23, Console.WindowHeight / 2);
            Console.Write("Enter the amount of players. Amount can be from 1 to 3");

            // Create a snake
            int amountSnaiks;
            do
                amountSnaiks = (int)Console.ReadKey(true).Key - '0';
            while (amountSnaiks is < 1 or > 3 ); 
            
            Console.Clear();

            // Filling the field with food
            const int amountFood = 150;
            FoodsInformation.Fill(amountFood);
            
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
            }

            GameOver();
        }
    }
}
