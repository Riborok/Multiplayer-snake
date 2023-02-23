using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Сlass for the snake
    public class Snake 
    {
        // List of points that composed a snake
        private List<SnakePart> snakePoints = new(300);

        public List<SnakePart> GetPoints()
        {
            return snakePoints;
        }
        
        // The direction of the snake
        private Direction direction = Direction.Right;
        
        // Snake movement keys 
        private readonly IMovementKeys movementKeys;
        
        // Snake head
        public SnakeHEADPoint head {get;}

        public Snake(int xHEAD, int yHEAD, IMovementKeys movementKeys)
        {
            this.movementKeys = movementKeys;
            
            // Checking that the snake is in an even position on the x coordinate
            if (xHEAD % 2 == 1)
                ++xHEAD;

            head = new SnakeHEADPoint(xHEAD, yHEAD);
            snakePoints.Add(new SnakeBodyPoint(head));
        }

        // Movement of the snake
        public void Move()
        {
            // Defining a new head for the snake
            switch (direction)
            {
                case Direction.Right:
                    head.X+=2;
                    break;
                case Direction.Down:
                    head.Y++;
                    break;
                case Direction.Left:
                    head.X-=2;
                    break;
                case Direction.Up:
                    head.Y--;
                    break;
            }

            // Checking if the snake has collided with a wall, its own body, or another snake
            if (head.X < 1 || head.X > Console.WindowWidth - 2 || head.Y < 1 || head.Y > Console.WindowHeight - 1 ||
                SnakesInformation.GetListPartsOfSnakes().Any(point => point.IsEquals(head)) )
            {
                // Because the snake hit the obstacle, take a step back
                head.X = snakePoints.Last().X;
                head.Y = snakePoints.Last().Y;
                snakePoints.RemoveAt(snakePoints.Count-1);
                
                //Program.GameOver(this);
                SnakesInformation.Dead(this);
            }
            else
            {
                // Draw the last point of the body 
                snakePoints.Last().Draw();

                // Adding a new body point
                snakePoints.Add(new SnakeBodyPoint(head));

                // Checking if the snake has eaten food
                Food food = FoodsInformation.GetFoodList().FirstOrDefault(food => food.IsEquals(head));
                if (food != null)
                    snakePoints.AddRange(DigestibleBody.GetListOfAddedBody(food));

                // Draw the head
                head.Draw();
                
                // Removing the tail of the snake
                snakePoints[0].Remove();
                snakePoints.RemoveAt(0);
            }
        }
        
        // Turning the snake
        public void Turn(ConsoleKey key)
        {
            Direction? trydirection = movementKeys.MovementDirection(key);
            if (trydirection.HasValue && (sbyte)direction + (sbyte)trydirection != 0)
                direction = trydirection.Value;
        }

    }
}