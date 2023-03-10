using System.Collections.Generic;


namespace SnakeGame
{
    // Class for the snake
    public class Snake 
    {
        // List of points that composed a snake body
        private readonly List<SnakeBodyPoint> _snakeBodyPoints = new(300);
        public IReadOnlyList<SnakeBodyPoint> BodyPoints => _snakeBodyPoints;
        public void AddBodyPoints(IEnumerable<SnakeBodyPoint> snakeParts)
        {
            // Add new points of the body and update the previous point
            _snakeBodyPoints.AddRange(snakeParts);
        }
        
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
        }
        
        // Last part of the snake's body 
        public SnakeBodyPoint LastBodyPart { get; private set; }
        
        // Movement of the snake
        public void Move()
        {
            // Save the coordinates of the previous head
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

        public void Draw()
        {
            // Drawing a snake 
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