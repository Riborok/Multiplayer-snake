using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeGame
{
    // Service working with snake information 
    public class SnakesService
    {
        // Array of colors that snakes can accept. The number in the array corresponds to the id of the snake 
        private readonly ConsoleColor[] _colorsForSnakes;
        
        // When a service is created, he will spawn amount snakes
        public SnakesService(int amount, ConsoleColor[] colorsForSnakes)
        {
            _colorsForSnakes = colorsForSnakes;
            _snakeList = new List<Snake>(amount);
            Spawn(amount);
        }
        
        // Stores the amount of snakes
        private readonly List<Snake> _snakeList;
        public IReadOnlyList<Snake> GetSnakeList => _snakeList;

        // This method returns all parts of all snakes
        public IEnumerable<Point> GetListPointsOfSnakes()
        {
            return _snakeList.SelectMany<Snake, Point>(snake => snake.BodyPoints.Concat<Point>
                (new[] { snake.Head }));
        }

        // This method respawn the snake. Before adding to the list, there is a delay.
        // This is necessary for the players to navigate the new spawn
        public async void Respawn(Snake snake)
        {
            await Task.Run(async () =>
            {

                if (!_snakeList.Remove(snake))
                    throw new ArgumentException("The provided snake does not exist in the list of snakes.");

                var newSnake = new Snake(Game.Generator.GenerateCoordinates(),
                    Game.Generator.GenerateDirection(), color: _colorsForSnakes[snake.Id], id: snake.Id);
                
                newSnake.Head.Draw();
                
                await Task.Delay(2000);

                _snakeList.Add(newSnake);
            });
        }
        
        // Spawner in the game amount snakes
        private void Spawn(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var newSnake = new Snake(Game.Generator.GenerateCoordinates(), Game.Generator.GenerateDirection(),
                    color: _colorsForSnakes[i], id: i); 
                
                newSnake.Head.Draw();
                
                _snakeList.Add(newSnake);
            }
        }
    }
}