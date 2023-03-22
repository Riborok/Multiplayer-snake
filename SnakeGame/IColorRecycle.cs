using System;

namespace SnakeGame
{
    // This interface defines a generic method for returning a color of a specified type
    public interface IColorRecycle<out TColor>
    {
        TColor Get(Color color);
    }

    // This is a concrete implementation of IColorRecycle with ConsoleColor as the generic type
    public class ColorRecycle : IColorRecycle<ConsoleColor>
    {
        public ConsoleColor Get(Color color)
        {
            return color switch
            {
                Color.White => ConsoleColor.White,
                Color.Magenta => ConsoleColor.Magenta,
                Color.DarkYellow => ConsoleColor.DarkYellow,
                Color.Cyan => ConsoleColor.Cyan,
                Color.Black => ConsoleColor.Black,
                Color.DarkBlue => ConsoleColor.DarkBlue,
                Color.DarkGreen => ConsoleColor.DarkGreen,
                Color.DarkCyan => ConsoleColor.DarkCyan,
                Color.DarkRed => ConsoleColor.DarkRed,
                Color.DarkMagenta => ConsoleColor.DarkMagenta,
                Color.Gray => ConsoleColor.Gray,
                Color.DarkGray => ConsoleColor.DarkGray,
                Color.Blue => ConsoleColor.Blue,
                Color.Green => ConsoleColor.Green,
                Color.Red => ConsoleColor.Red,
                Color.Yellow => ConsoleColor.Yellow,
                _ => throw new ArgumentException("Invalid color value", nameof(color))
            };
        }
    }
}