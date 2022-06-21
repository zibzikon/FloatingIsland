using System;
using UnityEngine;
public class GameOfLifeCellView : MonoBehaviour
{
    private GameOfLifeCell _model;
   [SerializeField] private MeshRenderer _meshRenderer;
   
    public void Initialize(GameOfLifeCell model)
    {
        _model = model;
        _model.Changed += Visualize;
    }

    private void Visualize()
    {
        if (_model.IsFilled)
        {
            _meshRenderer.material.color = Color.black;;
        }
        else
        {
            _meshRenderer.material.color = Color.white;;
        }
    }

    private void OnMouseDown()
    {
        if (_model == null) return;

        _model.IsFilled = !_model.IsFilled;
    }

    private void OnDisable()
    {
        if (_model == null) return;
        _model.Changed -= Visualize;
    }
}
