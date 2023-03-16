using System.Collections.Generic;

namespace SnakeGame;

// An interface for a ReadOnly list of complex objects that extends the IObjDict interface
public interface IComplexObjList<T> : IObjDict<Point>
{
    // Gets the read-only list of complex objects
    IReadOnlyList<T> ComplexObjList { get; }
    
    // Removes the specified specified object from the list
    void RemoveFromComplexObjList(T obj);
}