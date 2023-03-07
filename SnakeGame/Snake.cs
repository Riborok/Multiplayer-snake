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
        public IReadOnlyList<SnakeBodyPoint> BodyPoints => _snakeBodyPoints;

        // The direction of the snake
        private Direction _direction;
        
        // Snake movement keys 
        private readonly IMovementKeys _movementKeys;
        
        // Snake head
        public SnakeHeadPoint Head {get;}
        
        // Id (it is equal to the number in the SnakeList)
        public int Id {get;}

        public Snake((int x, int y) head, Direction direction, IMovementKeys movementKeys, int id)
        {
            _movementKeys = movementKeys;
            _direction = direction;
            Id = id;
            
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
            if (SnakeInformation.GetListPartsOfSnakes().FirstOrDefault(point => point.IsEquals(Head)
                    && point != Head) is { } otherSnakePart)
            {
                // Because the snake hit the obstacle, take a step back
                Head.CopyCoordinatesFrom(_previousPart);

                if (otherSnakePart.GetType() == typeof(SnakeHeadPoint))
                    SnakeInformation.GetSnakeList.First(snake => snake.Head == otherSnakePart).Dead();

                this.Dead();
            }
            else if (Head.X < 1 || Head.X > Console.WindowWidth - 1 || Head.Y < 1 || Head.Y > Console.WindowHeight - 2)
            {
                // Because the snake hit the obstacle, take a step back
                Head.CopyCoordinatesFrom(_previousPart);

                this.Dead();
            }
            else
            {
                // Draw the last point of the body 
                _previousPart.Draw();

                // Adding a new body point
                _snakeBodyPoints.Add(_previousPart);

                // Removing the tail of the snake
                RemoveSnakeBodyPointAt(0);

                // Checking if the snake has eaten food
                if (FoodInformation.GetFoodList.FirstOrDefault(currFood => currFood.IsEquals(Head)) is { } food)
                {
                    _snakeBodyPoints.AddRange(DigestibleBody.GetListOfAddedBody(food));
                    _previousPart = _snakeBodyPoints[_snakeBodyPoints.Count - 1];
                }
                else 
                    _previousPart = new SnakeBodyPoint(Head);
                
                // Draw the head
                Head.Draw();
            }
        }

        private void RemoveSnakeBodyPointAt(int index)
        {
            _snakeBodyPoints[index].Remove();
            _snakeBodyPoints.RemoveAt(index);    
        }
            
        private void Dead()
        {
            foreach (var body in _snakeBodyPoints)
                FoodInformation.Add(new SimpleFood(body));
            FoodInformation.Add(new SnakeHeadFood(Head));

            SnakeInformation.Remove(this);
            SnakeInformation.Add(new Snake(Generator.GenerateCoordinates(), Generator.GenerateDirection(), 
                _movementKeys, Id));
        }
        
        // Turning the snake
        public bool PassedTurn(ConsoleKey key)
        {
            var redirection = _movementKeys.MovementDirection(key, _direction);

            if (redirection != _direction)
            {
                _direction = redirection;
                return true;
            }
            return false;
        }

    }
}