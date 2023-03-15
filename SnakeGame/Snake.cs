using System;
using System.Collections.Generic;

namespace SnakeGame
{
    // Class for the snake
    public class Snake 
    {
        // List of points that composed a snake body
        private readonly List<SnakeBodyPoint> _snakeBodyPoints = new(300);
        
        // Returns the last point of the snake's body in the list 
        public IReadOnlyList<SnakeBodyPoint> BodyPoints => _snakeBodyPoints;
        
        // Eat food and add a new points to the _snakeBodyPoints 
        public void Eat(Food food)
        {
            // Add new points of the body and set _isDigestingFood 
            _snakeBodyPoints.AddRange(DigestibleBody.GetListOfAddedBody(food));
            
            // The last part after being eaten will be the digested part
            LastBodyPart = _snakeBodyPoints[_snakeBodyPoints.Count - 1];
            _isDigestingFood = true;
        }

        // Flag that indicates if the snake is currently digesting food.
        private bool _isDigestingFood;
        
        // The direction of the snake
        public Direction Direction { get; set; }

        // Snake head
        public SnakeHeadPoint Head { get; }
        
        // Id (Id number corresponds to a specific color and direction manager)
        public int Id { get; }
        
        public Snake((int x, int y) head, Direction direction, ConsoleColor color, int id)
        {
            // Set Id and Direction
            Direction = direction;
            Id = id;
            
            // Initialize Head, LastBodyPart and RemovedBodyPart
            Head = new SnakeHeadPoint(head.x, head.y, color);
            LastBodyPart = new SnakeBodyPoint(Head);
            PreviousTail = LastBodyPart;
        }
        
        // Last part of the snake's body 
        public SnakeBodyPoint LastBodyPart { get; private set; }
        
        // First part of the snake's body 
        public SnakeBodyPoint PreviousTail { get; private set; }

        // Movement of the snake
        public void Move()
        {
            // Save the coordinates of the previous head
            // If the food has been eaten, then the last body part has already been assigned
            if (!_isDigestingFood)
                LastBodyPart = new SnakeBodyPoint(Head);

            // Moving the head
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
            
            // Resetting the flag _isDigestingFood
            _isDigestingFood = false;
        }

        // Drawing a snake 
        public void Draw()
        {
            LastBodyPart.Draw();
            Head.Draw();

            // Update snake body points 
            BodyUpdate();
        }
        
        // Update snake body points 
        private void BodyUpdate()
        {
            // Update the list of body points
            _snakeBodyPoints.Add(LastBodyPart);

            // The first body part is the part that was removed
            PreviousTail = _snakeBodyPoints[0];
            RemoveSnakeBodyPointAt(0);
        }
        
        // Remove from _snakeBodyPoints and from the field the part of the body
        private void RemoveSnakeBodyPointAt(int index)
        {
            _snakeBodyPoints[index].Remove();
            _snakeBodyPoints.RemoveAt(index);    
        }
    }
}