using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SnakeGame
{
    // The main class of the game
    static class Game
    {
        private static class FullScreen
        {
            // WinAPI function
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetConsoleWindow();
            [DllImport("user32.dll")]
            private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
            private const int  SwMaximize = 3;

            public static void Set()
            {
                IntPtr handle = GetConsoleWindow();
                ShowWindow(handle, SwMaximize);
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);    
            }
    
        }
        private static void SetConsoleSettings()
        {
            Console.Title = "Snake Game";
        
            // Color setting
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            // Console window setting
            FullScreen.Set();

            Console.CursorVisible = false;   
        }

        private static int _amountSnakes;
        private const int AmountFood = 250;
        private const int ScoreToWin = 100;

        private static void GameCreation()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 23, Console.WindowHeight / 2);
            Console.Write("Enter the amount of players. Amount can be from 1 to 3");

            // Enter the amount of snakes. 1 to 3
            do
                _amountSnakes = (int)Console.ReadKey(true).Key - '0';
            while (_amountSnakes is < 1 or > 3 ); 
            
            Console.Clear();

            // Filling the field with food
            FoodInformation.FillWithSimpleFood(AmountFood);
            
            // Create a snake
            SnakeInformation.Fill(_amountSnakes);    
        }

        private static void GameOver()
        {
            Console.Clear(); 
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 3);
            Console.Write("Game Over");
            
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 + 3);
            Console.Write($"Score {ScoreToWin} has been reached");
            
            System.Threading.Thread.Sleep(10000);
        }

        static async Task Main()
        {
            SetConsoleSettings();

            GameCreation();

            // The main game loop (the game will continue until there is at least 1 snake)
            while (SnakeInformation.GetSnakeList().Any(snake => snake.GetBodyPoints().Count < ScoreToWin))
            {
                // Key handling asynchronous
                var inputTask = HandlingAsync.Key();
                
                // Snake movement handling asynchronous
                var moveTask = HandlingAsync.SnakeMove();

                // Frame delay
                await Task.Delay(48);
                
                // Wait for both tasks to complete
                await Task.WhenAll(moveTask, inputTask);
            }

            GameOver();
        }

        private static class HandlingAsync
        {
            private static readonly object LockObject = new();
            public static async Task SnakeMove()
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(SnakeInformation.GetSnakeList(), snake =>
                    {
                        lock (LockObject)
                        {
                            snake.Move();
                        }
                    });
                });
            }

            private static readonly bool[] WasSnake = new bool[_amountSnakes]; 
            public static async Task Key()
            {
                await Task.Run(() =>
                {
                    Array.Clear(WasSnake, 0, WasSnake.Length);
                    
                    // Processing user input
                    while (Console.KeyAvailable && WasSnake.Any(snake => !snake))
                    {
                        var key = Console.ReadKey(true).Key;
                        Parallel.For(0, SnakeInformation.GetSnakeList().Count, i =>
                        {
                            if (!WasSnake[i])
                                WasSnake[i] = SnakeInformation.GetSnakeList()[i].PassedTurn(key);
                        });
                    }
                });
            }
        }

    }
}
