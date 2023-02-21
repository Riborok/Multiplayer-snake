using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Сlass for the snake
    public class Snake
    {
        // List of points that composed a snake
        private List<Point> snakePoints = new(100);

        public List<Point> GetPoints()
        {
            return snakePoints;
        }
        
        // The direction of the snake
        private Direction direction;
        
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
            snakePoints.Add(new SnakeHEADPoint(xHEAD, yHEAD));
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
                SnakesInformation.GetSnakePoints().Any(point => point.IsEquals(head)))
            {
                //Program.GameOver(this);
                SnakesInformation.Dead(this);
                
            }
            else
            {
                // Draw the last point of the body 
                snakePoints.Last().Draw();

                // Adding a new body point
                snakePoints.Add(new SnakeBodyPoint(head.X, head.Y));

                // Checking if the snake has eaten food
                Food food = FoodsInformation.GetFoodList().FirstOrDefault(food => food.IsEquals(head));
                if (food != null)
                {
                    FoodsInformation.Delete(food);
                    score++;

                    snakePoints.Add(food);
                }

                // Draw the head
                head.Draw();
            }

            // Removing the tail of the snake
            snakePoints[0].Remove();
            snakePoints.RemoveAt(0);
        }



        // Turning the snake
        public void Turn(ConsoleKey key)
        {
            Direction? trydirection = movementKeys.MovementDirection(key);
            if (trydirection.HasValue && Math.Abs((byte)direction - (byte)trydirection) != 2)
                direction = trydirection.Value;
        }

        // Snake score
        private int score;

        // Getting snake score
        public int GetScore()
        {
            return score;
        }
        
    }
}