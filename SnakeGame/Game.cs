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
        private const int AmountSimpleFood = 450;
        private const int ScoreToWin = 100;
        
        // Array of SnakeDirectionManager. The number in the array corresponds to the id of the snake
        private static readonly SnakeDirectionManager[] SnakeDirectionManagers =
        {
            new (new ArrowsMovementKey()),
            new (new WasdMovementKey()),
            new (new UhjkMovementKey())
        };

        private static FoodCollisionManager _foodCollisionManager;
        private static ObstaclesCollisionManager _obstaclesCollisionManager;
        
        private static FoodInformationManager _foodInformationManager;
        private static SnakeInformationManager _snakeInformationManager;


        private static void GameCreation()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 23, Console.WindowHeight / 2);
            Console.Write("Enter the amount of players. Amount can be from 1 to 3");

            // Enter the amount of snakes. 1 to 3
            do
                _amountSnakes = (int)Console.ReadKey(true).Key - '0';
            while (_amountSnakes is < 1 or > 3 ); 
            
            Console.Clear();
            
            _foodInformationManager = new FoodInformationManager(AmountSimpleFood);
            _snakeInformationManager = new SnakeInformationManager(_amountSnakes);

            _foodCollisionManager = new FoodCollisionManager(_foodInformationManager);
            _obstaclesCollisionManager = new ObstaclesCollisionManager(_snakeInformationManager);
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
            while (_snakeInformationManager.GetSnakeList.All(snake => snake.BodyPoints.Count < ScoreToWin))
            {
                // Frame delay
                var delayTask = Task.Delay(45);
                
                // Key handling asynchronous
                var inputTask = HandlingKeysAsync();
                
                // Snake movement handling asynchronous
                var moveTask = HandlingSnakesAsync();

                // Wait for both tasks to complete
                await Task.WhenAll(moveTask, inputTask, delayTask);

            }

            GameOver();
            await Task.Delay(5000);
        }

        private static void Kill(Snake snake)
        {
            _foodInformationManager.AddRange(new[] { new SnakeHeadFood(snake.Head) }
                .Concat<Food>(snake.BodyPoints.Select(body => new SimpleFood(body)))); 
            _snakeInformationManager.SnakeRespawn(snake);
        }


        private static async Task HandlingSnakesAsync()
        {
            await Task.Run(() =>
            {
                // Can't use foreach here, because if the snake dies will be an error
                for (var i = 0; i < _snakeInformationManager.GetSnakeList.Count; i++)
                {
                    var snake = _snakeInformationManager.GetSnakeList[i];
                    
                    snake.Move();
                    
                    // Checking snake collision with obstacles
                    if (_obstaclesCollisionManager.IsCollisionOccured(snake))
                        foreach (var snakeToKill in _obstaclesCollisionManager.GetSnakesToKill)
                            Kill(snakeToKill);
                    
                    // If there was no collusion with obstacles, draw the snake and check the collision with food
                    else
                    {
                        snake.Draw();
                        _foodCollisionManager.FoodCollisionCheck(snake);
                    }
                    
                }
            });
        }
        
        private static async Task HandlingKeysAsync()
        {
            // Boolean array, for control: the player can change direction once per iteration
            var hasDirectionChanged = new bool[_amountSnakes];  
            await Task.Run(() =>
            {
                // Processing user input
                while (Console.KeyAvailable && !hasDirectionChanged.All(hasChanged => hasChanged))
                {
                    var key = Console.ReadKey(true).Key;
                    Parallel.ForEach(_snakeInformationManager.GetSnakeList, snake =>
                    {
                        if (!hasDirectionChanged[snake.Id])
                            hasDirectionChanged[snake.Id] = SnakeDirectionManagers[snake.Id].TryChangeDirection(snake, key);    
                    });
                }
            });
        }
    }
}
