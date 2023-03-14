using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SnakeGame
{
    // The main class of the game
    public static partial class Game
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
            Console.BackgroundColor = BackgroundColor;

            // Console window setting
            FullScreen.Set();

            Console.CursorVisible = false;
            
            _bordersTuple = 
            (
                UpBorder: 1, 
                DownBorder: Console.WindowHeight - 2, 
                LeftBorder: 3, 
                RightBorder: Console.BufferWidth - 1
            );
        }

        // Fields storing the amount of snakes and food
        private static int _amountSnakes;
        private const int AmountSimpleFood = 250;
        
        // Amount of points to win
        private const int ScoreToWin = 100;
        
        // Colors for background, text and border
        private const ConsoleColor BackgroundColor = ConsoleColor.Black; 
        private const ConsoleColor TextColor = ConsoleColor.Cyan; 
        private const ConsoleColor BorderColor = ConsoleColor.DarkGray; 
        
        // Array of SnakeDirectionManager. The number in the array corresponds to the id of the snake
        private static readonly SnakeDirectionManager[] SnakeDirectionManagers =
        {
            new (new ArrowsMovementKey()),
            new (new WasdMovementKey()),
            new (new UhjkMovementKey())
        };
        
        // Array of colors that snakes can accept. The number in the array corresponds to the id of the snake 
        private static readonly ConsoleColor[] ColorsForSnakes =
        {
            ConsoleColor.White,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkYellow
        };
        
        // Managers are responsible for collisions with obstacles and food
        private static FoodCollisionManager _foodCollisionManager;
        private static ObstaclesCollisionManager _obstaclesCollisionManager;
        
        // Services are responsible for information about snakes and food in the game 
        private static FoodService _foodService;
        private static SnakesService _snakesService;

        // Field borders
        private static (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) _bordersTuple;

        // Draws borders
        private static void MarkBorder()
        {
            // Set the border text color
            Console.ForegroundColor = BorderColor;

            // Border drawing
            Console.SetCursorPosition(_bordersTuple.LeftBorder, _bordersTuple.UpBorder);
            Console.Write(new string('▄', Console.BufferWidth - _bordersTuple.LeftBorder));

            for (var i = _bordersTuple.UpBorder + 1; i < _bordersTuple.DownBorder; i++)
            {
                Console.SetCursorPosition(_bordersTuple.RightBorder, i);
                Console.Write('█');
                Console.SetCursorPosition(_bordersTuple.LeftBorder, i);
                Console.Write('█');
            }

            Console.SetCursorPosition(_bordersTuple.LeftBorder, _bordersTuple.DownBorder);
            Console.Write(new string('▀', Console.BufferWidth - _bordersTuple.LeftBorder));
        }
        
        // Creating the playing field 
        private static void GameCreation()
        {
            Console.ForegroundColor = TextColor;
            Console.SetCursorPosition(Console.WindowWidth / 2 - 23, Console.WindowHeight / 2);
            Console.Write("Enter the amount of players. Amount can be from 1 to 3");

            // The user will press the keys until he presses a number from 1 to 3
            do
                _amountSnakes = (int)Console.ReadKey(true).Key - '0';
            while (_amountSnakes is < 1 or > 3 ); 
            
            Console.Clear();
            MarkBorder();
            
            _foodService = new FoodService(AmountSimpleFood);
            _snakesService = new SnakesService(_amountSnakes, ColorsForSnakes);

            _foodCollisionManager = new FoodCollisionManager(_foodService);
            _obstaclesCollisionManager = new ObstaclesCollisionManager(_snakesService, _bordersTuple);
        }

        // End of game caption
        private static void GameOver()
        {
            Console.Clear();
            Console.ForegroundColor = TextColor;

            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 - 3);
            Console.Write($"Score {ScoreToWin} has been reached");

            var playerArray = _snakesService.GetSnakeList.OrderByDescending(snake => 
                snake.BodyPoints.Count).ToList();

            for (var i = 0; i < playerArray.Count; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight / 2 + 3 + i*2);
                Console.Write($"{playerArray[i].Head.Color}, " +
                              $"you are {i}! Your score: {playerArray[i].BodyPoints.Count}");
            }
        }

        public static async Task Main()
        {
            SetConsoleSettings();
            
            GameCreation();
            await Task.Delay(1000);

            // The main game loop (the game will continue until there is at least 1 snake)
            while (_snakesService.GetSnakeList.All(snake => snake.BodyPoints.Count < ScoreToWin))
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
            await Task.Delay(60000);
        }

        // Killing a snake. Process the snake into food and respawn
        private static void Kill(Snake snake)
        {
            _foodService.ProcessIntoFood(snake);
            
            _snakesService.Respawn(snake);
        }
        
        // Handling snakes asynchronously 
        private static async Task HandlingSnakesAsync()
        {
            await Task.Run(() =>
            {
                // Can't use foreach here, because if the snake dies will be an error
                for (var i = 0; i < _snakesService.GetSnakeList.Count; i++)
                {
                    var snake = _snakesService.GetSnakeList[i];
                    
                    snake.Move();
                    
                    // Checking snake collision with obstacles
                    if (_obstaclesCollisionManager.IsCollisionOccured(snake))
                        foreach (var snakeToKill in _obstaclesCollisionManager.SnakesToKill)
                            Kill(snakeToKill);
                    
                    // If there was no collusion with obstacles, draw the snake and check the collision with food
                    else
                    {
                        snake.Draw();
                        _foodCollisionManager.CollisionCheck(snake);
                    }
                    
                }
            });
        }
        
        // Handling keys asynchronously 
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
                    foreach (var snake in _snakesService.GetSnakeList)
                        if (!hasDirectionChanged[snake.Id])
                            hasDirectionChanged[snake.Id] = SnakeDirectionManagers[snake.Id].TryChangeDirection(snake, key);
                }
            });
        }
    }
}
