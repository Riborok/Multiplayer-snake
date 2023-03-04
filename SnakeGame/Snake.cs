using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Class for the snake
    public class Snake 
    {
        // List of points that composed a snake body
        private readonly List<SnakeBodyPoint> _snakeBodyPoints = new(300);

        public List<SnakeBodyPoint> GetBodyPoints()
        {
            return _snakeBodyPoints;
        }
        
        // The direction of the snake
        private Direction _direction = Direction.Right;
        
        // Snake movement keys 
        public IMovementKeys MovementKeys {get;}
        
        // Snake head
        public SnakeHeadPoint Head {get;}

        public Snake((int x, int y) head, IMovementKeys movementKeys)
        {
            MovementKeys = movementKeys;
            
            // Since the head in the Move method immediately changes its position, record the value to the _previousPart 
            Head = new SnakeHeadPoint(head.x, head.y);
            _previousPart = new SnakeBodyPoint(Head);
        }

        private SnakeBodyPoint _previousPart; 

        // Movement of the snake
        public void Move()
        {
            // Defining a new head for the snake
            switch (_direction)
            {
                case Direction.Right:
                    Head.X+=2;
                    break;
                case Direction.Down:
                    Head.Y++;
                    break;
                case Direction.Left:
                    Head.X-=2;
                    break;
                case Direction.Up:
                    Head.Y--;
                    break;
            }

            // Checking if the snake has collided with a wall, its own body, or another snake
            if (Head.X < 1 || Head.X > Console.WindowWidth || Head.Y < 1 || Head.Y > Console.WindowHeight - 2 ||
                SnakeInformation.GetListPartsOfSnakes().Any(point => point.IsEquals(Head)) )
            {
                // Because the snake hit the obstacle, take a step back
                Head.CopyCoordinatesFrom(_previousPart);
                
                SnakeInformation.Kill(this);
            }
            else
            {
                // Draw the last point of the body 
                _previousPart.Draw();
                
                // Draw the head
                Head.Draw();

                // Adding a new body point
                _snakeBodyPoints.Add(_previousPart);
                
                // Removing the tail of the snake
                _snakeBodyPoints[0].Remove();
                _snakeBodyPoints.RemoveAt(0);

                // Checking if the snake has eaten food
                if (FoodInformation.GetFoodList().FirstOrDefault(currFood => currFood.IsEquals(Head)) is { } food)
                {
                    _snakeBodyPoints.AddRange(DigestibleBody.GetListOfAddedBody(food));
                    _previousPart = _snakeBodyPoints[_snakeBodyPoints.Count - 1];
                }
                else 
                    _previousPart = new SnakeBodyPoint(Head);
            }
        }
        
        // Turning the snake
        public bool PassedTurn(ConsoleKey key)
        {
            Direction redirection = MovementKeys.MovementDirection(key, _direction);

            if (redirection != _direction)
            {
                _direction = redirection;
                return true;
            }
            return false;
        }

    }
}