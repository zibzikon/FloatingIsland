using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Extentions
{
    public static class VectorExtentions
    {
        public static Vector3Int SetDirection(this Vector3Int vector, Direction2 vectorDirection, Direction2 direction)
        {
            var rotatedVector = SetDirection(new Vector2Int(vector.x, vector.z),vectorDirection, direction).ToVector3Int();
            return rotatedVector;
            static Vector2Int SetDirection(Vector2Int vector,Direction2 vectorDirection, Direction2 direction)
            {
                switch (direction)
                {
                    case Direction2.Right:
                        switch (vectorDirection)
                        {
                            case Direction2.Right: return vector;
                            case Direction2.Left: return vector.Rotate(180);
                            case Direction2.Foward: return vector.Rotate(-90);
                            case Direction2.Back: return vector.Rotate(90);
                        }
                        break;
                    case Direction2.Left: 
                        switch (vectorDirection)
                        {
                            case Direction2.Right: return vector.Rotate(180);
                            case Direction2.Left: return vector;
                            case Direction2.Foward: return vector.Rotate(90);
                            case Direction2.Back: return vector.Rotate(-90);
                        }
                        break;
                    case Direction2.Back:
                        switch (vectorDirection)
                        {
                            case Direction2.Right: return vector.Rotate(-90);
                            case Direction2.Left: return vector.Rotate(90);
                            case Direction2.Foward: return vector.Rotate(180);
                            case Direction2.Back: return vector;
                        }
                        break;
                    case Direction2.Foward:
                        switch (vectorDirection)
                        {
                            case Direction2.Right: return vector.Rotate(90);
                            case Direction2.Left: return vector.Rotate(-90);
                            case Direction2.Foward: return vector;
                            case Direction2.Back: return vector.Rotate(180);
                        }
                        break;
                }
                throw new InvalidOperationException();
            }
        }

        public static Vector3 GetRandomVector(Vector3 minRotation, Vector3 maxRotation)
        {
            return new Vector3(Random.Range(minRotation.x, maxRotation.x),
                Random.Range(minRotation.y, maxRotation.y),
                Random.Range(minRotation.z, maxRotation.z));
        }
        
        public static Vector2Int Rotate(this Vector2Int v, float degrees)
        { 
            float degToRad = (Mathf.PI/ 180) * degrees;
            var ca = Mathf.Cos(degToRad);
            var sa = Mathf.Sin(degToRad);
            return new Vector2(ca * v.x - sa * v.y, sa * v.x + ca * v.y).RoundToVector2Int();
        }

        public static Vector2Int RoundToVector2Int(this Vector2 vector)
        {
            return new Vector2Int((int)Mathf.Round(vector.x), (int)Mathf.Round(vector.y));
        }
        
        public static Vector3Int RoundToVector3Int(this Vector3 vector)
        {
            return new Vector3Int((int)Mathf.Round(vector.x), (int)Mathf.Round(vector.y), (int)Mathf.Round(vector.z));
        }
        
        public static Vector3Int ToVector3Int(this Vector2Int vector)
        {
            return new Vector3Int(vector.x,0, vector.y);
        }
    }
}