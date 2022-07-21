using System.Collections.Generic;
using UnityEngine;
public class BuildingDesignator : MonoBehaviour
{
    public BuildingType BuildingType => _buildingType;
    [SerializeField] private BuildingType _buildingType;
    
    [SerializeField] private List<BuildingDesignator> _right;
    [SerializeField] private List<BuildingDesignator> _left;
    [SerializeField] private List<BuildingDesignator> _forward;
    [SerializeField] private List<BuildingDesignator> _back;
    [SerializeField] private List<BuildingDesignator> _up;
    [SerializeField] private List<BuildingDesignator> _down;

    public Neighbors3<List<BuildingDesignator>> Neighbors => GetNeighbours();

    private Neighbors3<List<BuildingDesignator>> GetNeighbours()
    {
        var neighbour = new Neighbors3<List<BuildingDesignator>>()
        {
            Right = _right,
            Left = _left,
            Forward = _forward,
            Back = _back,
            Up = _up,
            Down = _down
        };
        return neighbour;
    }
}