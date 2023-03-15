using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SnakeGame
{
    // The main class of the game
    public static partial class Game
    {
        // Class responsible for full screen mode
        private static class FullScreen
        {
            // Class responsible for full screen mode
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetConsoleWindow();
            // WinAPI function to show or hide the window
            [DllImport("user32.dll")]
            private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
            // Show the window
            private const int SwMaximize = 3;

            // Method to set the console window to full screen
            public static void Set()
            {
                IntPtr handle = GetConsoleWindow();
                ShowWindow(handle, SwMaximize);
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);    
            }
        }
        
        // Method to set console settings
        private static void SetConsoleSettings()
        {
            Console.Title = "Snake Game";
        
            // Method to set console settings
            Console.BackgroundColor = BackgroundColor;

            // Set console window to full screen
            FullScreen.Set();

            Console.CursorVisible = false;
            
            // Set field borders
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
        private const int ScoreToWin = 200;
        
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
            ConsoleColor.Magenta,
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
            // Set the border color
            Console.ForegroundColor = BorderColor;

            // Set the border color
            Console.SetCursorPosition(_bordersTuple.LeftBorder, _bordersTuple.UpBorder);
            Console.Write(new string('▄', Console.BufferWidth - _bordersTuple.LeftBorder));

            // Draw left and right borders
            for (var i = _bordersTuple.UpBorder + 1; i < _bordersTuple.DownBorder; i++)
            {
                Console.SetCursorPosition(_bordersTuple.RightBorder, i);
                Console.Write('█');
                Console.SetCursorPosition(_bordersTuple.LeftBorder, i);
                Console.Write('█');
            }

            // Draw bottom border
            Console.SetCursorPosition(_bordersTuple.LeftBorder, _bordersTuple.DownBorder);
            Console.Write(new string('▀', Console.BufferWidth - _bordersTuple.LeftBorder));
        }
        
        // Creating the playing field 
        private static void GameCreation()
        {
            // Output message about entering the number of players
            Console.ForegroundColor = TextColor;
            Console.SetCursorPosition(Console.WindowWidth / 2 - 23, Console.WindowHeight / 2);
            Console.Write("Enter the amount of players. Amount can be from 1 to 3");

            // Waiting for the user to enter a valid number of players
            do
                _amountSnakes = (int)Console.ReadKey(true).Key - '0';
            while (_amountSnakes is < 1 or > 3 ); 
            
            // Clear the console and draw the game borders
            Console.Clear();
            MarkBorder();
            
            // Creating objects for managing game entities
            _snakesService = new SnakesService(_amountSnakes, ColorsForSnakes);
            _foodService = new FoodService(AmountSimpleFood, _snakesService.SnakesPointsDict);

            // Creating objects for managing game entities
            _foodCollisionManager = new FoodCollisionManager(_foodService);
            _obstaclesCollisionManager = new ObstaclesCollisionManager(_snakesService, _bordersTuple);
        }

        // End of game caption
        private static void GameOver()
        {
            // Clear the console and set the text color
            Console.Clear();
            Console.ForegroundColor = TextColor;

            // Output message about the end of the game
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 - 3);
            Console.Write($"Score {ScoreToWin} has been reached");

            // Sort the list of snakes by score and output the results
            var playerArray = _snakesService.GetSnakeList.OrderByDescending(snake => 
                snake.BodyPoints.Count).ToList();

            // Output the results of the game
            for (var i = 0; i < playerArray.Count; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight / 2 + 3 + i*2);
                Console.Write(playerArray[i].Head.Color +
                              $", you are {i}! Your score: {playerArray[i].BodyPoints.Count}");
            }
        }

        // The main method of the game
        public static async Task Main()
        {
            SetConsoleSettings();
            
            GameCreation();
            await Task.Delay(2000);

            // The game will continue until all players have points less than ScoreToWin
            while (_snakesService.GetSnakeList.All(snake => snake.BodyPoints.Count < ScoreToWin))
            {
                // Frame delay
                var delayTask = Task.Delay(45);
                
                // Key handling asynchronous
                var handlingKeysTask = HandlingKeysAsync();
                
                // Snake movement handling asynchronous
                var handlingSnakesTask = HandlingSnakesAsync();

                // Wait for both tasks to complete
                await Task.WhenAll(handlingSnakesTask, handlingKeysTask, delayTask);
                
                // If there are snakes to kill, kill them
                if (_obstaclesCollisionManager.ListOfSnakesToKill.Count() != 0)
                    KillSnakes();
            }

            GameOver();
            await Task.Delay(60000);
        }

        // Killing snakes from the list: process the snake into food and respawn, then clear the list
        private static void KillSnakes()
        {
            foreach (var snakeToKill in _obstaclesCollisionManager.ListOfSnakesToKill)
            {
                _snakesService.Respawn(snakeToKill);
                _foodService.ProcessIntoFood(snakeToKill);
            }
            _obstaclesCollisionManager.ClearListOfSnakesToKill();
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

                    // If the snake is not on the kill list, work with it
                    if (!_obstaclesCollisionManager.ListOfSnakesToKill.Contains(snake))
                    {
                        snake.Move();

                        // Checking snake collision with obstacles
                        if (!_obstaclesCollisionManager.HasCollisionOccurred(snake))
                        {
                            // If there was no collusion with obstacles, draw the snake and check the collision with food
                            snake.Draw();
                            _foodCollisionManager.CollisionCheck(snake);
                        }
                        
                        _snakesService.UpdateSnakePointsDict
                            (snake.PreviousTail, snake.LastBodyPart, snake.Head);
                    }
                }
            });
        }
        
        // Handling keys asynchronously 
        private static async Task HandlingKeysAsync()
        {
            // Boolean array, for control: the player can change direction once per iteration
            var hasDirectionChanged = new bool[_snakesService.GetSnakeList.Count];  
            
            await Task.Run(() =>
            {
                // Processing user input
                while (Console.KeyAvailable && !hasDirectionChanged.All(hasChanged => hasChanged))
                {
                    var key = Console.ReadKey(true).Key;
                    for (var i = 0; i < _snakesService.GetSnakeList.Count; i++)
                        if (!hasDirectionChanged[i])
                        {
                            var snake = _snakesService.GetSnakeList[i];
                            hasDirectionChanged[i] = SnakeDirectionManagers[snake.Id].TryChangeDirection(snake, key);
                        }
                }
            });
        }
    }
}
