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
            PreviousPart = _snakeBodyPoints[_snakeBodyPoints.Count - 1];
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
            PreviousPart = new SnakeBodyPoint(Head);
        }

        public SnakeBodyPoint PreviousPart { get; private set; }

        // Movement of the snake
        public void Move()
        {
            PreviousPart = new SnakeBodyPoint(Head);
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
            PreviousPart.Draw();
            Head.Draw();
            
            BodyUpdate();
        }

        // Update snake body points 
        private void BodyUpdate()
        {
            // Update the list of body points
            _snakeBodyPoints.Add(PreviousPart);
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