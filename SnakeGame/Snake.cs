using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Сlass for the snake
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
            Head = new SnakeHeadPoint(head.x, head.y);
            
            // In the list of bodies, we will store the head (the previous values,
            // and the new value of the head will be stored in Head) as a body.
            // This is necessary in the event of a collision, it was possible to take a step back.
            _snakeBodyPoints.Add(new SnakeBodyPoint(Head));
        }

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
                SnakesInformation.GetListPartsOfSnakes().Any(point => point.IsEquals(Head)) )
            {
                // Because the snake hit the obstacle, take a step back
                Head.X = _snakeBodyPoints.Last().X;
                Head.Y = _snakeBodyPoints.Last().Y;
                _snakeBodyPoints.RemoveAt(_snakeBodyPoints.Count-1);
                
                //Program.GameOver(this);
                SnakesInformation.Dead(this);
            }
            else
            {
                // Draw the last point of the body 
                _snakeBodyPoints.Last().Draw();

                // Adding a new body point
                _snakeBodyPoints.Add(new SnakeBodyPoint(Head));

                // Checking if the snake has eaten food
                if (FoodsInformation.GetFoodList().FirstOrDefault(currFood => currFood.IsEquals(Head)) is Food food)
                    _snakeBodyPoints.AddRange(DigestibleBody.GetListOfAddedBody(food));

                // Draw the head
                Head.Draw();
                
                // Removing the tail of the snake
                _snakeBodyPoints[0].Remove();
                _snakeBodyPoints.RemoveAt(0);
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