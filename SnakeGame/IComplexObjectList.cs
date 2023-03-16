using System.Collections.Generic;

namespace SnakeGame;

// An interface for a ReadOnly list of complex objects that extends the IObjDict interface
public interface IComplexObjectList<T> : IObjectDictionary<Point>
{
    // Gets the read-only list of complex objects
    IReadOnlyList<T> ComplexObjList { get; }
    
    // Removes the specified complex object from the list
    void RemoveFromComplexObjList(T obj);
}