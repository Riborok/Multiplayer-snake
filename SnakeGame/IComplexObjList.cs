using System.Collections.Generic;

namespace SnakeGame;

// An interface for a ReadOnly list of complex objects that extends the IObjDict interface
public interface IComplexObjList<out T> : IObjDict<Point>
{
    // Gets the read-only list of complex objects
    IReadOnlyList<T> ComplexObjList { get; }
}