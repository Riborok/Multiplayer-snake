using System.Collections.Generic;

namespace SnakeGame;


// An interface for a dictionary of objects, providing read-only access
// to the dictionary and a method for deleting objects from it
public interface IObjDict<T> 
{
    // Gets the ReadOnly dictionary of objects
    IReadOnlyDictionary<(int x, int y), T> ObjDict { get; }
    
    // Removes the specified object from the dictionary
    void RemoveFromObjDict(T obj);
}
