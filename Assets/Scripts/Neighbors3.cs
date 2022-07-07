using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Neighbors3<T>
{ 
    public T Right;
    public T Left;
    public T Forward;
    public T Back;
    public T Up;
    public T Down;

    public const int Length = 6; 
    
    public T this[Direction3 direction]
    {
        get 
        { 
            switch(direction)
            {
                case Direction3.Zero:
                    throw new NullReferenceException("Direction cannot be zero");

                case Direction3.Right: return Right;
                case Direction3.Left: return Left;
                case Direction3.Foward: return Forward;
                case Direction3.Back: return Back;
                case Direction3.Up: return Up;
                case Direction3.Down: return Down;
                default: throw new IndexOutOfRangeException();
            }
        }
        set
        {
            switch (direction)
            {
                case Direction3.Zero:
                    throw new NullReferenceException("Direction cannot be zero");

                case Direction3.Right: Right = value; break;
                case Direction3.Left: Left = value; break;
                case Direction3.Foward: Forward = value; break;
                case Direction3.Back: Back = value; break;
                case Direction3.Up: Up = value;break;
                default: Down = value; break;
            }
        }
    }

    public T this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return Right;
                case 1: return Left;
                case 2: return Forward;
                case 3: return Back;
                case 4: return Up;
                case 5: return Down;
                default: throw new IndexOutOfRangeException();
            }
        }
        set
        {
            switch (i)
            {
                case 0: Right = value;
                    break;
                case 1: Left = value;
                    break;
                case 2: Forward = value;                   
                    break;
                case 3: Back = value;                   
                    break;
                case 4: Up = value;
                    break;
                case 5: Down = value;
                    break;

                default: throw new IndexOutOfRangeException();
            }
        }
    }
    
}

public class Neighbors2<T>
{ 
    public T Right;
    public T Left;
    public T Foward;
    public T Back;

    public const int Length = 4; 
    public T this[Direction3 direction]
    {
        get 
        { 
            switch(direction)
            {
                case Direction3.Zero:
                    throw new NullReferenceException("Direction cannot be zero");

                case Direction3.Right: return Right;
                case Direction3.Left: return Left;
                case Direction3.Foward: return Foward;
                case Direction3.Back: return Back;
                default: throw new NullReferenceException();
            }
        }
        set
        {
            switch (direction)
            {
                case Direction3.Zero:
                    throw new NullReferenceException("Direction cannot be zero");

                case Direction3.Right: Right = value; return;
                case Direction3.Left: Left = value; return;
                case Direction3.Foward: Foward = value; return;
                case Direction3.Back: Back = value; return;
            }
        }
    }

    public T this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return Right;
                case 1: return Left;
                case 2: return Foward;
                case 3: return Back;
                default: throw new IndexOutOfRangeException();
            }
        }
        set
        {
            switch (i)
            {
                case 0: Right = value;
                    break;
                case 1: Left = value;
                    break;
                case 2: Foward = value;                   
                    break;
                case 3: Back = value;                   
                    break;

                default: throw new IndexOutOfRangeException();
            }
        }
    }
}
