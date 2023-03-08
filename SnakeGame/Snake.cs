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
        public Direction Direction { get; set; }

        // Snake head
        public SnakeHeadPoint Head {get;}
        
        // Id (id number corresponds to the various lists)
        public int Id {get;}
        

        public Snake((int x, int y) head, Direction direction, int id)
        {
            Direction = direction;
            Id = id;
            
            // Since the head in the Move method immediately changes its position, record the value to the _previousPart 
            Head = new SnakeHeadPoint(head.x, head.y);
            _previousPart = new SnakeBodyPoint(Head);
        }

        private SnakeBodyPoint _previousPart; 

        // Movement of the snake
        public void Move()
        {
            MoveHead();
            if (CheckCollisionWithObstacles() || CheckCollisionWithPartsOfSnakes())
            {
                // If the snake collided with an obstacle or another snake, roll back head movement and die
                Head.CopyCoordinatesFrom(_previousPart);
                Die();
                return;
            }
            
            _previousPart.Draw();
            BodyUpdate();
            CheckCollisionWithFood();
            Head.Draw();
        }

        // Movement of the snakes head
        private void MoveHead()
        {
            switch (Direction)
            {
                case Direction.Right:
                    Head.X += 2;
                    break;
                case Direction.Down:
                    Head.Y++;
                    break;
                case Direction.Left:
                    Head.X -= 2;
                    break;
                case Direction.Up:
                    Head.Y--;
                    break;
            }
        }

        // Check collision with obstacles 
        private bool CheckCollisionWithObstacles()
        {
            return Head.X < 1 || Head.X > Console.WindowWidth - 1 || Head.Y < 1 || Head.Y > Console.WindowHeight - 2;
        }

        // Check collision with parts of snakes
        private bool CheckCollisionWithPartsOfSnakes()
        {
            // Checking for collisions with other parts of the snakes and own parts (except head)    
            if (SnakeInformation.GetListPartsOfSnakes().FirstOrDefault(point => point.IsEquals(Head) 
                    && point != Head) is { } snakePart)
            {
                // If the snakes collided head to head, kill another snake
                if (snakePart.GetType() == typeof(SnakeHeadPoint))
                    SnakeInformation.GetSnakeList.Single(snake => snake.Head == snakePart).Die();

                return true;
            }
            
            return false;
        }

        // Update snake body points 
        private void BodyUpdate()
        {
            // Update the list of body points
            _snakeBodyPoints.Add(_previousPart);
            RemoveSnakeBodyPointAt(0);
            _previousPart = new SnakeBodyPoint(Head);
        }

        // Check collision with food
        private void CheckCollisionWithFood()
        {
            // Checking if the snake collision with food
            if (FoodInformation.GetFoodList.FirstOrDefault(currFood => currFood.IsEquals(Head)) is { } food)
            {
                // If there is, add new points of the body and update the previous point
                _snakeBodyPoints.AddRange(DigestibleBody.GetListOfAddedBody(food));
                _previousPart = _snakeBodyPoints[_snakeBodyPoints.Count - 1];
            }
        }

        // Remove from _snakeBodyPoints and from the field the part of the body
        private void RemoveSnakeBodyPointAt(int index)
        {
            _snakeBodyPoints[index].Remove();
            _snakeBodyPoints.RemoveAt(index);    
        }
            
        // Kill this snake and spawn a new snake
        private void Die()
        {
            foreach (var body in _snakeBodyPoints)
                FoodInformation.Add(new SimpleFood(body));
            FoodInformation.Add(new SnakeHeadFood(Head));

            SnakeInformation.SnakeRespawn(this);
        }

    }
}