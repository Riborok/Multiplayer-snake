using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    // Сlass for the snake
    class Snake
    {
        // List of points that composed a snake
        private List<Point> snakePoints = new List<Point>();

        // The direction of the snake
        private Direction direction;

        public Snake(int x, int y)
        {
            snakePoints.Add(new SnakePoint(x, y));
        }

        // Movement of the snake
        public void Move()
        {
            // Defining a new head for the snake
            Point head = new SnakePoint(snakePoints.Last().X, snakePoints.Last().Y);
            switch (direction)
            {
                case Direction.Rigth:
                    head.X++;
                    break;
                case Direction.Down:
                    head.Y++;
                    break;
                case Direction.Lef:
                    head.X--;
                    break;
                case Direction.Up:
                    head.Y--;
                    break;
            }

            // Checking if the snake has collided with a wall, its own body, or another snake
            if (head.X < 1 || head.X > Console.WindowWidth - 1 || head.Y < 1 || head.Y > Console.WindowHeight - 1 ||
                snakePoints.Any(point => point.IsEquals(head)))
            {
                Program.GameOver(this);
            }
            
            // Adding a new snake head
            snakePoints.Add(head);
            
            // Checking if the snake has eaten food
            Food isEaten = foodInformation.GetFoodList().FirstOrDefault(food => food.IsEquals(head));
            if (isEaten != null)
            {
                foodInformation.Delete(isEaten);
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
            switch (key)
            {
                case ConsoleKey.RightArrow:
                    if (direction != Direction.Lef)
                        direction = Direction.Rigth;
                    break;
                
                case ConsoleKey.DownArrow:
                    if (direction != Direction.Up)
                        direction = Direction.Down;
                    break;
                
                case ConsoleKey.LeftArrow:
                    if (direction != Direction.Rigth)
                        direction = Direction.Lef;
                    break;
                
                case ConsoleKey.UpArrow:
                    if (direction != Direction.Down)
                        direction = Direction.Up;
                    break;
            }
        }

        // Snake score
        private int score;

        // Getting snake score
        public int GetScore()
        {
            return score;
        }

        // Information about other objects on the game field
        private FoodInformation foodInformation = FoodInformation.Get();
    }
}