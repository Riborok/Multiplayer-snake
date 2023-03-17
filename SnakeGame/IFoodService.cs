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
        void SpawnFood(int amount);
        
        // Processes the snake's body and head into food
        void ProcessIntoFood(Snake snake);
    }

    // Service working with food
    public class FoodService : IFoodProcessSpawn, IFoodAddRemove
    {

        // The amount of food that needs to be maintained on the canvas
        private int _foodAmount;
        
        // The current amount of food on the canvas
        private int _foodAmountOnMap;

        // Color for generated simple food
        private const Color ColorForGeneratedSimpleFood = Color.Cyan;

        // Game canvas
        private readonly ICanvas _canvas;
        
        public FoodService(ICanvas canvas)
        {
            _canvas = canvas;
        }

        // Spawn simple food. This amount of food will be maintained
        public void SpawnFood(int amount)
        {
            _foodAmount = amount;
            _foodAmountOnMap += amount;
            
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

        // Removes food from the canvas and if the current amount is less than
        // the initial, add new simple food
        public void Remove(Food food)
        {
            _canvas.RemoveFromMap(food);
            _canvas.ClearPoint(food);
            _foodAmountOnMap--;

            if (_foodAmountOnMap < _foodAmount)
                Add(CreateSimpleFood());
        }

        // Add food to the canvas and draw it
        public void Add(Food food)
        {
            _foodAmountOnMap++;
            _canvas.AddToMap(food);
            _canvas.DrawPoint(food);
        }

        // This method creates a simple food with randomly coordinates
        private Food CreateSimpleFood()
        {
            // Return SimpleFood with the generated coordinates and a specified color
            return new SimpleFood(Game.Generator.GenerateFreeCoordinates(), ColorForGeneratedSimpleFood);
        }
    }
}