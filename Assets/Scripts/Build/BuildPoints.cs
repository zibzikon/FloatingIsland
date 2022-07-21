using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildPoints : MonoBehaviour
{
    [SerializeField] private List<BuildPoint> _right;
    [SerializeField] private List<BuildPoint> _left;
    [SerializeField] private List<BuildPoint> _forward;
    [SerializeField] private List<BuildPoint> _back;
    [SerializeField] private List<BuildPoint> _up;
    [SerializeField] private List<BuildPoint> _down;

    private Neighbors3<List<BuildPoint>> _points;
    public Neighbors3<List<BuildPoint>> Points => _points;

    private IBuildingContainer _buildingContainer;

    public void Initialize(IBuildingContainer buildingContainer)
    {
        _buildingContainer = buildingContainer;
        _points = GetPoints();
    }
    
    private Neighbors3<List<BuildPoint>> GetPoints()
    {

        InitializeBuildPointsDirection(_right, Direction3.Right);
        InitializeBuildPointsDirection(_left, Direction3.Left);
        InitializeBuildPointsDirection(_forward, Direction3.Foward);
        InitializeBuildPointsDirection(_back, Direction3.Back);
        InitializeBuildPointsDirection(_up, Direction3.Up);
        InitializeBuildPointsDirection(_down, Direction3.Down);
        
        return new Neighbors3<List<BuildPoint>>
        { 
            Right = _right,
            Left = _left,
            Forward = _forward,
            Back = _back,
            Up = _up,
            Down = _down 
        };
    }

    private void InitializeBuildPointsDirection(List<BuildPoint> buildPoints, Direction3 direction)
    {
        foreach (var buildPoint in buildPoints)
        {
            buildPoint.Initialize(_buildingContainer, direction);
        }
    }
}
