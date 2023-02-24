using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Сlass for the snake
    public class Snake 
    {
        // List of points that composed a snake
        private readonly List<SnakePart> _snakePoints = new(300);

        public List<SnakePart> GetPoints()
        {
            return _snakePoints;
        }
        
        // The direction of the snake
        private Direction _direction = Direction.Right;
        
        // Snake movement keys 
        private readonly IMovementKeys _movementKeys;
        
        // Snake head
        public SnakeHeadPoint Head {get;}

        public Snake(int xHead, int yHead, IMovementKeys movementKeys)
        {
            _movementKeys = movementKeys;
            
            // Checking that the snake is in an even position on the x coordinate
            if (xHead % 2 == 1)
                ++xHead;
            Head = new SnakeHeadPoint(xHead, yHead);
            
            _snakePoints.Add(new SnakeBodyPoint(Head));
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
            if (Head.X < 1 || Head.X > Console.WindowWidth - 2 || Head.Y < 1 || Head.Y > Console.WindowHeight - 1 ||
                SnakesInformation.GetListPartsOfSnakes().Any(point => point.IsEquals(Head)) )
            {
                // Because the snake hit the obstacle, take a step back
                Head.X = _snakePoints.Last().X;
                Head.Y = _snakePoints.Last().Y;
                _snakePoints.RemoveAt(_snakePoints.Count-1);
                
                //Program.GameOver(this);
                SnakesInformation.Dead(this);
            }
            else
            {
                // Draw the last point of the body 
                _snakePoints.Last().Draw();

                // Adding a new body point
                _snakePoints.Add(new SnakeBodyPoint(Head));

                // Checking if the snake has eaten food
                Food food = FoodsInformation.GetFoodList().FirstOrDefault(food => food.IsEquals(Head));
                if (food != null)
                    _snakePoints.AddRange(DigestibleBody.GetListOfAddedBody(food));

                // Draw the head
                Head.Draw();
                
                // Removing the tail of the snake
                _snakePoints[0].Remove();
                _snakePoints.RemoveAt(0);
            }
        }
        
        // Turning the snake
        public void Turn(ConsoleKey key)
        {
            _direction = _movementKeys.MovementDirection(key, _direction);
        }

    }
}