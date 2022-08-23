
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
        _meshRenderer.material.color = _cell.IsBlocked ? Color.red : Color.white;
    }

    private void OnDisable()
    {
        _cell.Changed -= ChangeView;
    }
}
