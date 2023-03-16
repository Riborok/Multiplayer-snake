namespace SnakeGame
{
    // The manager checks the collision of the snake with food
    public class FoodCollisionManager
    {
        // A reference to the FoodService, used to manipulate food
        private readonly IObjDict<Food> _foodDict;
        
        // Constructor for the FoodCollisionManager, requires a FoodService
        public FoodCollisionManager(IObjDict<Food> foodDict)
        {
            _foodDict = foodDict;
        }
        
        // Check for collision with food
        public void CollisionCheck(Snake snake)
        {
            if (_foodDict.ObjDict.TryGetValue((snake.Head.X, snake.Head.Y), out var collidingFood))
            {
                // Make the snake eat the food and remove it from the FoodService
                snake.Eat(collidingFood);
                _foodDict.RemoveFromObjDict(collidingFood);
            }
        }
    }
}