using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildPoints : MonoBehaviour
{
    [SerializeField] private List<BuildPoint> _right;
    [SerializeField] private List<BuildPoint> _left;
    [SerializeField] private List<BuildPoint> _foward;
    [SerializeField] private List<BuildPoint> _back;
    [SerializeField] private List<BuildPoint> _up;
    [SerializeField] private List<BuildPoint> _down;
    
    public Neighbors3<List<BuildPoint>> Points { get; private set; }

    private void Awake()
    {
        Points = new Neighbors3<List<BuildPoint>>
        { 
            Right = _right,
            Left = _left,
            Foward = _foward,
            Back = _back,
            Up = _up,
            Down = _down 
        };
    }
}
