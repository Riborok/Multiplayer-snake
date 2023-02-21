using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Сlass for the snake
    public class Snake
    {
        // List of points that composed a snake
        private List<Point> snakeBodyPoints = new(100);

        public List<Point> GetPoints()
        {
            return snakeBodyPoints;
        }
        
        // The direction of the snake
        private Direction direction;

        private IMovementKeys movementKeys;

        public Snake(int x, int y, IMovementKeys movementKeys)
        {
            snakeBodyPoints.Add(new SnakeHEADPoint(x, y));
            this.movementKeys = movementKeys;
        }

        // Movement of the snake
        public void Move()
        {
            // Defining a new head for the snake
            SnakeHEADPoint head = new SnakeHEADPoint(snakeBodyPoints.Last().X, snakeBodyPoints.Last().Y);
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
            
            // Draw the last point of the body 
            snakeBodyPoints.Last().Draw();
            
            // Adding a new body point
            snakeBodyPoints.Add( new SnakeBodyPoint(head.X, head.Y) );

            // Checking if the snake has eaten food
            Food food = FoodsInformation.GetFoodList().FirstOrDefault(food => food.IsEquals(head));
            if (food != null)
            {
                FoodsInformation.Delete(food);
                score++;
                
                snakeBodyPoints.Add(food);
            }

            // Draw the head
            head.Draw();

            // Removing the tail of the snake
            snakeBodyPoints[0].Remove();
            snakeBodyPoints.RemoveAt(0);
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