using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SnakeGame
{
    // The main class of the game
    public static class Game
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
        private const int AmountSimpleFood = 250;
        private const int ScoreToWin = 100;
        
        // Array of SnakeDirectionManager. The number in the array corresponds to the id of the snake
        private static readonly SnakeDirectionManager[] SnakeDirectionManagers =
        {
            new (new ArrowsMovementKey()),
            new (new WasdMovementKey()),
            new (new UhjkMovementKey())
        };

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
            FoodInformationManager.SpawnSimpleFood(AmountSimpleFood);
            
            // Create a snake
            SnakeInformationManager.SpawnSnakes(_amountSnakes);
        }

        private static void GameOver()
        {
            Console.Clear(); 
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 3);
            Console.Write("Game Over");
            
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 + 3);
            Console.Write($"Score {ScoreToWin} has been reached");
        }

        public static async Task Main()
        {
            SetConsoleSettings();
            
            GameCreation();
            await Task.Delay(1000);

            // The main game loop (the game will continue until there is at least 1 snake)
            while (SnakeInformationManager.GetSnakeList.All(snake => snake.BodyPoints.Count < ScoreToWin))
            {
                // Frame delay
                var delayTask = Task.Delay(45);
                
                // Key handling asynchronous
                var inputTask = HandlingAsync.Key();
                
                // Snake movement handling asynchronous
                var moveTask = HandlingAsync.SnakeMove();

                // Wait for both tasks to complete
                await Task.WhenAll(moveTask, inputTask, delayTask);

            }

            GameOver();
            await Task.Delay(5000);
        }

        private static class HandlingAsync
        {
            public static async Task SnakeMove()
            {
                await Task.Run(() =>
                {
                    // Can't use foreach here, because if the snake dies will be an error
                    for (var i = 0; i < SnakeInformationManager.GetSnakeList.Count; i++)
                        SnakeInformationManager.GetSnakeList[i].Move();
                });
            }

            // Boolean array, for control: the player can change direction once per iteration
            private static readonly bool[] HasMoved = new bool[_amountSnakes]; 
            public static async Task Key()
            {
                Array.Clear(HasMoved, 0, HasMoved.Length);
                await Task.Run(() =>
                {
                    // Processing user input
                    while (Console.KeyAvailable && !HasMoved.All(hasMoved => hasMoved))
                    {
                        var key = Console.ReadKey(true).Key;
                        Parallel.ForEach(SnakeInformationManager.GetSnakeList, snake =>
                        {
                            if (!HasMoved[snake.Id])
                                HasMoved[snake.Id] = SnakeDirectionManagers[snake.Id].TryChangeDirection(snake, key);    
                        });
                    }
                });
            }
        }

    }
}
