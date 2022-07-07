
using UnityEngine;

public class GameFieldCellView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    private Cell _cell;

    public void Initialize(Cell cell)
    {
        _cell = cell;
        _cell.Changed += ChangeView;
        ChangeView();
    }
    
    private void ChangeView()
    {
        if (_cell.Capacity <= 0)
        {
            _meshRenderer.material.color = Color.red;
        }
        else if (_cell.Capacity < 25)
        {
            _meshRenderer.material.color = Color.yellow;
        }
        else if (_cell.Capacity < 50)
        {
            _meshRenderer.material.color = Color.green;
        }
        else if (_cell.Capacity < 75)
        {
            _meshRenderer.material.color = Color.cyan;
        }
        else if (_cell.Capacity < 100)
        {            
            _meshRenderer.material.color = Color.gray;
        }
        else if (_cell.Capacity >= 100)
        {            
            _meshRenderer.material.color = Color.white;
        }
    }

    private void OnDisable()
    {
        _cell.Changed -= ChangeView;
    }
}
