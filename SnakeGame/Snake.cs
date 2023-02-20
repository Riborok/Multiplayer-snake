using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Сlass for the snake
    public class Snake
    {
        // List of points that composed a snake
        private List<Point> snakePoints = new List<Point>();

        public List<Point> GetPoints()
        {
            return snakePoints;
        }
        
        // The direction of the snake
        private Direction direction;

        private MovementKeys movementKeys;

        public Snake(int x, int y, MovementKeys movementKeys)
        {
            snakePoints.Add(new SnakePoint(x, y));
            this.movementKeys = movementKeys;
        }

        // Movement of the snake
        public void Move()
        {
            // Defining a new head for the snake
            Point head = new SnakePoint(snakePoints.Last().X, snakePoints.Last().Y);
            switch (direction)
            {
                case Direction.Right:
                    head.X++;
                    break;
                case Direction.Down:
                    head.Y++;
                    break;
                case Direction.Left:
                    head.X--;
                    break;
                case Direction.Up:
                    head.Y--;
                    break;
            }

            // Checking if the snake has collided with a wall, its own body, or another snake
            if (head.X < 1 || head.X > Console.WindowWidth - 1 || head.Y < 1 || head.Y > Console.WindowHeight - 1 ||
                SnakeInformation.GetSnakePoints().Any(point => point.IsEquals(head)))
            {
                //Program.GameOver(this);
                SnakeInformation.Dead(this);
            }
            
            // Adding a new snake head
            snakePoints.Add(head);
            
            // Checking if the snake has eaten food
            Food isEaten = FoodInformation.GetFoodList().FirstOrDefault(food => food.IsEquals(head));
            if (isEaten != null)
            {
                FoodInformation.Delete(isEaten);
                score++;
                
                snakePoints.Add(isEaten);
                head = isEaten;
            }

            // Draw the head
            head.Draw();

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