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
        private const int AmountSimpleFood = 400;
        
        // Amount of points to win
        private const int ScoreToWin = 200;
        
        // Colors for background, text and border
        private const Color BackgroundColor = Color.Black; 
        private const Color TextColor = Color.Cyan; 
        private const Color BorderColor = Color.DarkGray;

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
            FullScreen.Set();
            _canvas = new ConsoleCanvas(wallTuple: (1, Console.WindowHeight - 2, 3, Console.BufferWidth - 1), 
                new ColorRecycle());

            // Set background color
            _canvas.SetBackgroundColor(BackgroundColor);

            // Output message about entering the number of players
            _canvas.ClearCanvas();
            _canvas.WriteMessage
            ( 
                (_canvas.BorderTuple.RightBorder - _canvas.BorderTuple.LeftBorder)/ 2 - 23,
                (_canvas.BorderTuple.DownBorder - _canvas.BorderTuple.UpBorder) / 2,
                TextColor,
                "Enter the amount of players. Amount can be from 1 to 3"
            );
            
            // Creating manager for changing the direction of the snakes
            _snakeDirectionManagers = new SnakeDirectionManagerForConsole();

            // Waiting for the user to enter a valid number of players
            do
                _amountSnakes = (int)_snakeDirectionManagers.ReadKey() - '0';
            while (_amountSnakes is < 1 or > 3 ); 
            
            // Clear the canvas and draw the game borders
            _canvas.ClearCanvas();
            _canvas.MarkWalls(BorderColor);

            // Service creation
            _snakeService = new SnakeService(_canvas, ColorsForSnakes);
            _foodService = new FoodService(_canvas);
            
            // Spawn objects
            _snakeService.SpawnSnakes(_amountSnakes);
            _foodService.SpawnSimpleFood(AmountSimpleFood);

            // Creating collision control managers
            _foodCollisionManager = new FoodCollisionManager((IFoodAddRemove)_foodService, _canvas);
            _obstaclesCollisionManager = new ObstaclesCollisionManager(_snakeService.Snakes, _canvas);
        }

        // End of game caption
        private static void GameOver()
        {
            // Clear the canvas
            _canvas.ClearCanvas();

            // Output message about the end of the game
            _canvas.WriteMessage
            ( 
                (_canvas.BorderTuple.RightBorder - _canvas.BorderTuple.LeftBorder)/ 2 - 13,
                (_canvas.BorderTuple.DownBorder - _canvas.BorderTuple.UpBorder) / 2 - 3,
                TextColor,
                $"Score {ScoreToWin} has been reached"
            );

            // Sort the list of snakes by score and output the results
            var snakeResultList = _snakeService.Snakes.Values.OrderByDescending(snake => 
                snake.BodyPoints.Count).ToList();

            // Output the results of the game
            for (var i = 0; i < snakeResultList.Count; i++)
            {
                _canvas.WriteMessage
                ( 
                    (_canvas.BorderTuple.RightBorder - _canvas.BorderTuple.LeftBorder)/ 2 - 15,
                    (_canvas.BorderTuple.DownBorder - _canvas.BorderTuple.UpBorder) / 2 + 3 + i * 2,
                    TextColor,
                    $"{snakeResultList[i].Head.Color}, you are {i}! Your score: {snakeResultList[i].BodyPoints.Count}"
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
                var delayTask = Task.Delay(45);
                
                // Key handling asynchronous
                await HandlingKeysAsync();

                // Snake movement handling asynchronous
                await HandlingSnakesAsync();

                // If there are snakes to kill, kill them
                if (_obstaclesCollisionManager.SnakesToKill.Count() != 0)
                    await KillSnakes();
                
                // Waiting for completion delay
                await delayTask;
            }
            
            // Disable food spawn
            _foodService.DisablePeriodicSpawn();

            GameOver();
            await Task.Delay(60000);
        }

        // Killing snakes from the list: process the snake into food and respawn, then clear the list
        private static async Task KillSnakes()
        {
            await Task.Run(() =>
            {
                foreach (var snakeToKill in _obstaclesCollisionManager.SnakesToKill)
                {
                    _snakeService.RemoveSnake(snakeToKill);
                    _foodService.ProcessIntoFood(snakeToKill);
                    _snakeService.SpawnSnake(snakeToKill.Id);
                }

                _obstaclesCollisionManager.ClearListOfSnakesToKill();
            });
        }
        
        // Handling snakes asynchronously 
        private static async Task HandlingSnakesAsync()
        {
            await Task.Run(() =>
            {
                // If the snake has a direction and is not on the kill list, work with it
                foreach (var snake in _snakeService.Snakes.Values
                             .Where(snake => snake.Direction != Direction.None &&
                                             !_obstaclesCollisionManager.SnakesToKill.Contains(snake)))
                {
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
            });
        }
        
        // Handling keys asynchronously 
        private static async Task HandlingKeysAsync()
        {
            await Task.Run(() =>
            {

                // Boolean array, for control: the player can change direction once per iteration
                var hasDirectionChanged = new bool[_amountSnakes];

                // Processing user input
                while (_snakeDirectionManagers.IsKeyPress())
                {
                    var key = _snakeDirectionManagers.ReadKey();
                    foreach (var snake in _snakeService.Snakes.Values)
                        if (!hasDirectionChanged[snake.Id])
                            hasDirectionChanged[snake.Id] = _snakeDirectionManagers
                                .TryChangeDirection(snake, MovementKeys[snake.Id], key);
                }
            });
        }
    }
}
