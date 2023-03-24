namespace SnakeGame
{
    // Interface for adding and removing food on the playing field
    public interface IFoodAddRemove
    {
        // Remove food from the game canvas
        void Remove(Food food);
    
        // Add food to the game canvas
        void Add(Food food);
    }
    
    // Creating food and turning a snake into food
    public interface IFoodProcessSpawn
    {
        // Spawn a specified amount of food on the canvas
        void SpawnSimpleFood(int amount);
        
        // Processes the snake's body and head into food
        void ProcessIntoFood(Snake snake);

        // Enable periodic food spawn
        void EnablePeriodicSpawn();

        // Disable periodic food spawn
        void DisablePeriodicSpawn();
    }

    // Service working with food
    public class FoodService : IFoodProcessSpawn, IFoodAddRemove
    {
        // Color for generated simple food
        private const Color ColorForGeneratedSimpleFood = Color.Cyan;

        // Game canvas
        private readonly IMapCanvas _mapCanvas;
        
        public FoodService(IMapCanvas mapCanvas)
        {
            _mapCanvas = mapCanvas;
        }

        // Food spawn timer
        private System.Timers.Timer _timer;
        
        // Food spawn period
        private const int SpawnPeriod = 4200;
        
        // Enable periodic food spawn
        public void EnablePeriodicSpawn()
        {
            _timer = new System.Timers.Timer(SpawnPeriod);
            _timer.Elapsed += (_, _) => Add(CreateSimpleFood());
            _timer.Enabled = true;
        }
        
        // Disable periodic food spawn
        public void DisablePeriodicSpawn()
        {
            _timer.Enabled = false;
            _timer.Dispose();
        }

        // Spawn simple food
        public void SpawnSimpleFood(int amount)
        {
            for (var i = 0; i < amount; i++)
                Add(CreateSimpleFood());
        }

        // Processing a snake into food
        public void ProcessIntoFood(Snake snake)
        {
            // Processing a body points into simple food
            foreach (var snakeBodyPoint in snake.BodyPoints)
                Add(new SimpleFood(snakeBodyPoint));

            // Processing a head into sneak head food
            Add(new SnakeHeadFood(snake.Head, snake.BodyPoints.Count));
        }

        // Removes food from the canvas and erase it
        public void Remove(Food food)
        {
            _mapCanvas.RemoveFromMap(food);
            _mapCanvas.ClearPoint(food);
        }

        // Add food to the canvas and draw it
        public void Add(Food food)
        {
            _mapCanvas.AddToMap(food);
            _mapCanvas.DrawPoint(food);
        }

        // This method creates a simple food with randomly coordinates
        private static Food CreateSimpleFood()
        {
            // Return SimpleFood with the generated coordinates and a specified color
            return new SimpleFood(Game.Generator.GenerateFreeCoordinates(), ColorForGeneratedSimpleFood);
        }
    }
}