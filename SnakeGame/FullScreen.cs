using System;
using System.Runtime.InteropServices;

namespace SnakeGame;

// Class responsible for full screen mode
public static class FullScreen
{
    // Class responsible for full screen mode
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetConsoleWindow();
    // WinAPI function to show or hide the window
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    // Show the window
    private const int SwMaximize = 3;

    // Method to set the console window to full screen
    public static void Set()
    {
        IntPtr handle = GetConsoleWindow();
        ShowWindow(handle, SwMaximize);
        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);    
    }
}