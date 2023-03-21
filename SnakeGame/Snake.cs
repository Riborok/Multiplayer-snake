using System.Collections.Generic;

namespace SnakeGame
{
    // Class for the snake
    public class Snake 
    {
        // List of points that composed a snake body
        private readonly LinkedList<SnakeBodyPoint> _snakeBodyPoints;
        public IReadOnlyCollection<SnakeBodyPoint> BodyPoints => _snakeBodyPoints;
        
        // Eat food (add a new points to the _snakeBodyPoints) 
        public void Eat(Food food)
        {
            // Add new points of the body and set _isDigestingFood 
            foreach (var digestibleBody in DigestibleBody.GetListOfAddedBody(food))
                _snakeBodyPoints.AddLast(digestibleBody);
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
        
        public Snake((int x, int y) head, Direction direction, Color color, int id)
        {
            // Set Id and Direction
            Direction = direction;
            Id = id;
            
            _snakeBodyPoints = new LinkedList<SnakeBodyPoint>();
            
            // Head initialization
            Head = new SnakeHeadPoint(head.x, head.y, color);
        }
        
        // Last part of the snake's body 
        public SnakeBodyPoint LastBodyPart { get; private set; }
        
        // Previous tail (which was removed)
        public SnakeBodyPoint PreviousTail { get; private set; }

        // Movement of the snake
        public void Move()
        {
            
            // If the food has been eaten, then the last body part is already
            // in the list - it's the digest part (he is last on the list)
            if (_isDigestingFood)
            {
                LastBodyPart = _snakeBodyPoints.Last.Value;
                _isDigestingFood = false;
            }
            
            // If the food has not been eaten, then the last part is the head
            else
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
        }

        // Update snake body points 
        public void BodyUpdate()
        {
            // Update the list of body points
            _snakeBodyPoints.AddLast(LastBodyPart);

            // The first body part is the part that was removed
            PreviousTail = _snakeBodyPoints.First.Value;
            _snakeBodyPoints.RemoveFirst();
        }
    }
}