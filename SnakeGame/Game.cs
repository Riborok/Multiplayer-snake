using System;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeGame
{
    // The main class of the game
    public static partial class Game
    {
        // Canvas for the game
        private static IMapCanvas _canvas;

        // Fields storing the amount of snakes and food
        private static int _amountSnakes;
        private static int _amountSimpleFood;
        
        // Amount of points to win
        private const int ScoreToWin = 200;

        // Frame Delay
        private const int FrameDelay = 45;
        
        // Colors for background, text and border
        private const Color BackgroundColor = Color.Black; 
        private const Color TextColor = Color.Cyan; 
        private const Color BorderColor = Color.DarkGray;

        // Indent when outputting by Y coordinate
        private const int YIndent = 2;

        // Manager is responsible for changing the direction of the snakes
        private static ISnakeDirectionManager<ConsoleKey> _snakeDirectionManagers;

        // Array of movement keys. The number in the array corresponds to the id of the snake
        private static readonly IMovementKeys<ConsoleKey>[] MovementKeys =
        {
            new ArrowsMovementKey(),
            new WasdMovementKey(),
            new UhjkMovementKey()
        };
        
        // Array of colors that snakes can accept. The number in the array corresponds to the id of the snake 
        private static readonly Color[] ColorsForSnakes =
        {
            Color.DarkBlue,
            Color.Magenta,
            Color.DarkYellow
        };
        
        // Managers are responsible for collisions with obstacles and food
        private static IFoodCollisionManager _foodCollisionManager;
        private static IObstaclesCollisionManager _obstaclesCollisionManager;
        
        // Services are responsible for the correct drawing and storage of points on the canvas
        private static IFoodProcessSpawn _foodService;
        private static ISnakeService _snakeService;

        // Creating the playing field 
        private static void GameCreation()
        {
            // Setting Full Screen for the console and create a canvas
            _canvas = new ConsoleCanvas(new ColorRecycle());

            // Set background color
            _canvas.SetBackgroundColor(BackgroundColor);

            // Output message about entering the number of players
            _canvas.ClearCanvas();

            // Calculate the median coordinates
            var centralHeight = (_canvas.BorderTuple.DownBorder - _canvas.BorderTuple.UpBorder) >> 1;
            var centralWidth = (_canvas.BorderTuple.RightBorder - _canvas.BorderTuple.LeftBorder) >> 1;
            
            // Defining messages to display
            const string firstMsg = "Set window size for game field adjustment.";
            const string secondMsg = "Window size cannot be changed during the game!!!";
            const string thirdMsg = "Enter the amount of players. Amount can be from 1 to 3.";            
            
            _canvas.WriteMessage
            ( 
                centralWidth - (thirdMsg.Length >> 1),
                centralHeight,
                TextColor,
                thirdMsg
            );

            _canvas.WriteMessage
            (
                centralWidth - (secondMsg.Length >> 1),
                centralHeight - YIndent,
                TextColor,
                secondMsg
            );
            
            _canvas.WriteMessage
            (
                centralWidth - (firstMsg.Length >> 1),
                centralHeight - (YIndent << 1),
                TextColor,
                firstMsg
            );
            
            // Creating manager for changing the direction of the snakes
            _snakeDirectionManagers = new SnakeDirectionManagerForConsole();

            // Waiting for the user to enter a valid number of players
            do
                _amountSnakes = (int)_snakeDirectionManagers.ReadKey() - '0';
            while (_amountSnakes is < 1 or > 3 );
            
            // Setting boundaries
            _canvas.SetBorders((1, Console.WindowHeight - 2, 3, Console.WindowWidth - 1));

            _amountSimpleFood = (Console.WindowHeight + Console.WindowWidth) << 1;
            
            // Initialization the boolean array that control: the player can change direction once per iteration
            _hasDirectionChanged = new bool[_amountSnakes];
            
            // Clear the canvas and draw the game borders
            _canvas.ClearCanvas();
            _canvas.MarkWalls(BorderColor);

            // Service creation
            _snakeService = new SnakeService(_canvas, ColorsForSnakes);
            _foodService = new FoodService(_canvas);
            
            // Spawn objects
            _snakeService.SpawnSnakes(_amountSnakes);
            _foodService.SpawnSimpleFood(_amountSimpleFood);

            // Creating collision control managers
            _foodCollisionManager = new FoodCollisionManager((IFoodAddRemove)_foodService, _canvas);
            _obstaclesCollisionManager = new ObstaclesCollisionManager(_snakeService.Snakes, _canvas);
        }

        // End of game caption
        private static void GameOver()
        {
            // Clear the canvas
            _canvas.ClearCanvas();

            // Defining messages to display
            var currMsg = $"Score {ScoreToWin} has been reached";
            
            // Calculate the median coordinates
            var centralHeight = (_canvas.BorderTuple.DownBorder - _canvas.BorderTuple.UpBorder) >> 1;
            var centralWidth = (_canvas.BorderTuple.RightBorder - _canvas.BorderTuple.LeftBorder) >> 1;

            // Output message about the end of the game
            _canvas.WriteMessage
            ( 
                centralWidth - (currMsg.Length >> 1),
                centralHeight - YIndent,
                TextColor,
                currMsg
            );

            // Sort the list of snakes by score and output the results
            var snakeResultList = _snakeService.Snakes.Values.OrderByDescending(snake => 
                snake.BodyPoints.Count).ToList();

            // Output the results of the game
            for (var i = 0; i < snakeResultList.Count; i++)
            {
                currMsg =
                    $"{snakeResultList[i].Head.Color}, you are {i}! Your score: {snakeResultList[i].BodyPoints.Count}";
                _canvas.WriteMessage
                (
                    centralWidth - (currMsg.Length >> 1),
                    centralHeight + i * YIndent,
                    TextColor,
                    currMsg
                );
            }
        }

        // The main method of the game
        public static async Task Main()
        {
            // Create a game
            GameCreation();

            // Enable food spawn
            _foodService.EnablePeriodicSpawn();

            // The game will continue until all players have points less than ScoreToWin
            while (_snakeService.Snakes.Values.All(snake => snake.BodyPoints.Count < ScoreToWin))
            {
                // Frame delay
                var delayTask = Task.Delay(FrameDelay);
                
                // Key handling asynchronous
                HandlingKeys();

                // Snake movement handling asynchronous
                HandlingSnakes();

                // If there are snakes to kill, kill them
                if (_obstaclesCollisionManager.SnakesToKill.Count() != 0)
                    KillSnakes();
                
                // Waiting for completion delay
                await delayTask;
            }
            
            // Disable food spawn and snake drawing
            _foodService.DisablePeriodicSpawn();
            _snakeService.СanDrawSnake = false;

            GameOver();
            await Task.Delay(60000);
        }

        // Killing snakes from the list: process the snake into food and respawn, then clear the list
        private static void KillSnakes()
        {
            foreach (var snakeToKill in _obstaclesCollisionManager.SnakesToKill)
            {
                _snakeService.RemoveSnake(snakeToKill);
                _foodService.ProcessIntoFood(snakeToKill);
                _snakeService.SpawnSnake(snakeToKill.Id);
            }

            _obstaclesCollisionManager.ClearListOfSnakesToKill();
        }
        
        // Handling snakes asynchronously 
        private static void HandlingSnakes()
        {
            // If the snake has a direction and is not on the kill list, work with it
            foreach (var snake in _snakeService.Snakes.Values
                         .Where(snake => snake.Direction != Direction.None &&
                                         !_obstaclesCollisionManager.SnakesToKill.Contains(snake)))
            {
                // Move the snake
                snake.Move();

                // Checking snake collision with obstacles
                if (!_obstaclesCollisionManager.HasCollisionOccurred(snake))
                {
                    // If there was no collusion with obstacles, update the snake,
                    // then draw it and check the collision with food
                    snake.BodyUpdate();
                    _foodCollisionManager.CollisionCheck(snake);
                    _snakeService.UpdateSnakeOnCanvas(snake);
                }
            }
        }

        // Boolean array that control: the player can change direction once per iteration
        private static bool[] _hasDirectionChanged; 
        
        // Handling keys asynchronously 
        private static void HandlingKeys()
        {
            // Clear array for new iteration
            Array.Clear(_hasDirectionChanged, 0, _amountSnakes);
            
            // Processing user input
            while (_snakeDirectionManagers.IsKeyPress())
            {
                var key = _snakeDirectionManagers.ReadKey();
                foreach (var snake in _snakeService.Snakes.Values)
                    if (!_hasDirectionChanged[snake.Id])
                        _hasDirectionChanged[snake.Id] = _snakeDirectionManagers
                            .TryChangeDirection(snake, MovementKeys[snake.Id], key);
            }
        }
    }
}
