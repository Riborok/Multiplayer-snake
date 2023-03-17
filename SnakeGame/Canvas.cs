using System;

namespace SnakeGame;

// This is an interface for a map of points
public interface IPointMap
{
    // 2D array of points
    Point[,] GetMap { get; }
    
    // Tuple with the borders of the map
    (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) BordersTuple { get; }
    
    // Add a point to the map
    void AddToMap(Point point);
    
    // Remove a point from the map
    void RemoveFromMap(Point point);
}

// This is an interface for a canvas that implements IPointMap
public interface ICanvas : IPointMap
{
    // Set the background color of the canvas
    void SetBackgroundColor(Color color);
    
    // Draw a point on the canvas
    void DrawPoint(Point point);
    
    // Clear a point from the canvas
    void ClearPoint(Point point);
    
    // Write a message on the canvas
    void WriteMessage(int x, int y, Color color, string line);
    
    // Draw a borders
    void MarkBorders(Color color);
    
    // Clear the entire canvas
    void ClearCanvas();
}

// This is a concrete implementation of ICanvas that draws on the console
public sealed class ConsoleCanvas : ICanvas
{
    // Recycle colors
    private readonly IColorRecycle<ConsoleColor> _recycler;
    
    // 2D array of points that represents the canvas
    public Point[,] GetMap { get; }
    
    // Borders of the canvas
    public (int UpBorder, int DownBorder, int LeftBorder, int RightBorder) BordersTuple { get; }

    public ConsoleCanvas((int UpBorder, int DownBorder, int LeftBorder, int RightBorder) bordersTuple, 
        IColorRecycle<ConsoleColor> recycler)
    {
        BordersTuple = bordersTuple;
        GetMap = new Point[BordersTuple.RightBorder, BordersTuple.DownBorder];
        _recycler = recycler;
        SetConsoleSettings();
    }
    
    // Set the background color of the console
    public void SetBackgroundColor(Color color)
    {
        Console.BackgroundColor = _recycler.Get(color);
    }
    
    // Add a point to the map
    public void AddToMap(Point point)
    {
        GetMap[point.X, point.Y] = point;
    }

    // Remove a point from the map
    public void RemoveFromMap(Point point)
    {
        GetMap[point.X, point.Y] = null;
    }

    // Write a point to the console
    public void DrawPoint(Point point)
    {
        Console.SetCursorPosition(point.X, point.Y);
        Console.ForegroundColor = _recycler.Get(point.Color);
        Console.Write(point.Symbol);
    }
    
    // Write a blank space to the console
    public void ClearPoint(Point point)
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
    public void MarkBorders(Color color)
    {
        Console.ForegroundColor = _recycler.Get(color);

        Console.SetCursorPosition(BordersTuple.LeftBorder, BordersTuple.UpBorder);
        Console.Write(new string('▄', Console.BufferWidth - BordersTuple.LeftBorder));

        for (var i = BordersTuple.UpBorder + 1; i < BordersTuple.DownBorder; i++)
        {
            Console.SetCursorPosition(BordersTuple.RightBorder, i);
            Console.Write('█');
            Console.SetCursorPosition(BordersTuple.LeftBorder, i);
            Console.Write('█');
        }

        Console.SetCursorPosition(BordersTuple.LeftBorder, BordersTuple.DownBorder);
        Console.Write(new string('▀', Console.BufferWidth - BordersTuple.LeftBorder));
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






