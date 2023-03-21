using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SnakeGame
{
    // This is an interface for a map of points
    public interface IPointMap
    {
        // Get a point on the map
        IReadOnlyDictionary<(int x, int y), IPoint> Map { get; }

        // Tuple with the borders of the map
        (int UpWall, int DownWall, int LeftWall, int RightWall) WallTuple { get; }

        // Add a point to the map
        void AddToMap(IPoint point);

        // Remove a point from the map
        void RemoveFromMap(IPoint point);
    }

    // This is an interface for a canvas that implements IPointMap
    public interface ICanvas
    {
        // Tuple with the borders of the map
        (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) BorderTuple { get; }

        // Set the background color of the canvas
        void SetBackgroundColor(Color color);

        // Draw a point on the canvas
        void DrawPoint(IPoint point);

        // Clear a point from the canvas
        void ClearPoint(IPoint point);

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
    }

    // This is a concrete implementation of ICanvas that draws on the console
    public sealed class ConsoleCanvas : IMapCanvas
    {
        // Recycle colors
        private readonly IColorRecycle<ConsoleColor> _recycler;

        // 2D Hash Table of points that represents the canvas
        private readonly ConcurrentDictionary<(int x, int y), IPoint> _map;

        // Get a point on the map
        public IReadOnlyDictionary<(int x, int y), IPoint> Map => _map;
        
        // Walls of the map
        public (int UpWall, int DownWall, int LeftWall, int RightWall) WallTuple { get; }

        // Borders of the canvas
        public (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) BorderTuple { get; }

        public ConsoleCanvas((int UpWall, int DownWall, int LeftWall, int RightWall) wallTuple,
            IColorRecycle<ConsoleColor> recycler)
        {
            WallTuple = wallTuple;
            BorderTuple = (0, Console.WindowHeight, 0, Console.BufferWidth);
            _recycler = recycler;
            _map = new ConcurrentDictionary<(int x, int y), IPoint>();
            SetConsoleSettings();
        }

        // Set the background color of the console
        public void SetBackgroundColor(Color color)
        {
            Console.BackgroundColor = _recycler.Get(color);
        }

        // Add a point to the map
        public void AddToMap(IPoint point)
        {
            _map.AddOrUpdate((point.X, point.Y), point, (_, _) => point);
        }

        // Remove a point from the map
        public void RemoveFromMap(IPoint point)
        {
            _map.TryRemove((point.X, point.Y), out _ );
        }
        
        // Object to use as a lock object to synchronize access to the shared resource
        private static readonly object Lock = new();

        // Write a point to the console
        public void DrawPoint(IPoint point)
        {
            // Access synchronization
            lock (Lock)
            {
                Console.SetCursorPosition(point.X, point.Y);
                Console.ForegroundColor = _recycler.Get(point.Color);
                Console.Write(point.Symbol);   
            }
        }

        // Write a blank space to the console
        public void ClearPoint(IPoint point)
        {
            Console.SetCursorPosition(point.X, point.Y);
            Console.Write(' ');
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





