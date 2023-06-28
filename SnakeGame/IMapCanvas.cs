using System;

namespace SnakeGame
{
    // This is an interface for a map of points
    public interface IPointMap
    {
        // Get a point on the map
        ICoordinates GetPoint(int x, int y);

        // Tuple with the borders of the map
        (int UpWall, int DownWall, int LeftWall, int RightWall) WallTuple { get; }

        // Add a point to the map
        void AddToMap(ICoordinates coordinates);

        // Remove a point from the map
        void RemoveFromMap(ICoordinates coordinates);
    }

    // This is an interface for a canvas that implements IPointMap
    public interface ICanvas
    {
        // Tuple with the borders of the map
        (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) BorderTuple { get; }

        // Set the background color of the canvas
        void SetBackgroundColor(Color color);

        // Draw a point on the canvas
        void DrawPoint(IDrawablePoint drawablePoint);

        // Clear a point from the canvas
        void ClearPoint(ICoordinates coordinates);

        // Write a message on the canvas
        void WriteMessage(int x, int y, Color color, string line);

        // Clear the entire canvas
        void ClearCanvas();
    }

    // This interface combines the functionality of IPointMap and ICanvas,
    // allowing for the manipulation and display of a 2D map of points on a canvas. 
    public interface IMapCanvas : IPointMap, ICanvas
    {
        // Draw a borders
        void MarkWalls(Color color);

        void SetBorders((int UpWall, int DownWall, int LeftWall, int RightWall) wallTuple);
    }

    // This is a concrete implementation of ICanvas that draws on the console
    public sealed class ConsoleCanvas : IMapCanvas
    {
        // Recycle colors
        private readonly IColorRecycle<ConsoleColor> _recycler;

        // 2D array of points that represents the canvas
        // Since in the console the snake moves along the X coordinates +2,
        // in all calls to X, perform the >>1 operation to reduce memory
        private ICoordinates[,] _getMap;

        // Get a point on the map
        public ICoordinates GetPoint(int x, int y)
        {
            // Access synchronization
            lock (MapLock)
                return _getMap[x >>1, y];
        }
        
        // Walls of the map
        public (int UpWall, int DownWall, int LeftWall, int RightWall) WallTuple { get; private set; }

        // Borders of the canvas
        public (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) BorderTuple { get; private set; }

        public ConsoleCanvas(IColorRecycle<ConsoleColor> recycler)
        {
            _recycler = recycler;
            SetConsoleSettings();
            BorderTuple = (0, Console.WindowHeight, 0, Console.BufferWidth);
        }

        public void SetBorders((int UpWall, int DownWall, int LeftWall, int RightWall) wallTuple)
        {
            WallTuple = wallTuple; 
            
            // Since a dynamic array starts at 0 and ends at length-1, add 1 to make the last element the length
            lock (MapLock)
                _getMap = new ICoordinates[(WallTuple.RightWall >>1) + 1, WallTuple.DownWall];

            BorderTuple = (0, Console.WindowHeight, 0, Console.BufferWidth);
        }

        // Set the background color of the console
        public void SetBackgroundColor(Color color)
        {
            Console.BackgroundColor = _recycler.Get(color);
        }
        
        // Object to use as a lock object to synchronize access to the shared resource
        private static readonly object MapLock = new();

        // Add a point to the map
        public void AddToMap(ICoordinates coordinates)
        {
            // Access synchronization
            lock (MapLock)
                _getMap[coordinates.X >>1, coordinates.Y] = coordinates;
        }

        // Remove a point from the map
        public void RemoveFromMap(ICoordinates coordinates)
        {
            // Access synchronization
            lock (MapLock)
                _getMap[coordinates.X >>1, coordinates.Y] = null;
        }
        
        // Object to use as a lock object to synchronize access to the shared resource
        private static readonly object DrawLock = new();

        // Write a point to the console
        public void DrawPoint(IDrawablePoint drawablePoint)
        {
            // Access synchronization
            lock (DrawLock)
            {
                Console.SetCursorPosition(drawablePoint.X, drawablePoint.Y);
                Console.ForegroundColor = _recycler.Get(drawablePoint.Color);
                Console.Write(drawablePoint.Symbol);   
            }
        }

        // Write a blank space to the console
        public void ClearPoint(ICoordinates coordinates)
        {
            // Access synchronization
            lock (DrawLock)
            {
                Console.SetCursorPosition(coordinates.X, coordinates.Y);
                Console.Write(' ');
            }
        }

        // Write the message to the console
        public void WriteMessage(int x, int y, Color color, string line)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = _recycler.Get(color);
            Console.Write(line);
        }

        // Write borders to the console
        public void MarkWalls(Color color)
        {
            Console.ForegroundColor = _recycler.Get(color);

            Console.SetCursorPosition(WallTuple.LeftWall, WallTuple.UpWall);
            Console.Write(new string('▄', Console.BufferWidth - WallTuple.LeftWall));

            for (var i = WallTuple.UpWall + 1; i < WallTuple.DownWall; i++)
            {
                Console.SetCursorPosition(WallTuple.RightWall, i);
                Console.Write('█');
                Console.SetCursorPosition(WallTuple.LeftWall, i);
                Console.Write('█');
            }

            Console.SetCursorPosition(WallTuple.LeftWall, WallTuple.DownWall);
            Console.Write(new string('▀', Console.BufferWidth - WallTuple.LeftWall));
        }

        // Clears the console
        public void ClearCanvas()
        {
            Console.Clear();
        }

        // Method to set console settings
        private static void SetConsoleSettings()
        {
            Console.Title = "Snake Game";

            Console.CursorVisible = false;
        }
    }
}






