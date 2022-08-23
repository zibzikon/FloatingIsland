using System;
using Extentions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class GridLayout : MonoBehaviour
{
    private RectTransform _rectTransform;
    public int Columns { get; private set; }
    public int Rows { get; private set; }

    private RectTransform[,] _elements;

    public void Initialize(Vector2Int size)
    {
        _rectTransform = GetComponent<RectTransform>();
        //_rectTransform.pivot = new Vector2(0, 0);
        Columns = size.x;
        Rows = size.y;
        
        _elements = new RectTransform[Columns, Rows];
    }

    public void SetElement(RectTransform layoutElement , Vector2Int position)
    {
        _elements[position.x, position.y] = layoutElement;
        
        CorrectElementPosition(layoutElement, position);
    }

    private void CorrectElementsPositions()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        for (var x = 0; x < Columns; x++)
        {
            for (var y = 0; y < Rows; y++)
            {
                var element = _elements[x, y];

                if (element == null) continue;

                CorrectElementPosition(element, new Vector2Int(x, y));
            }
        }
    }

    private void CorrectElementPosition(RectTransform element, Vector2Int position)
    {
        element.pivot = new Vector2(0, 0);
        
        var localScale = _rectTransform.sizeDelta;

        var correctElementScale = new Vector3(localScale.x / Columns, localScale.y / Rows);

        var correctPosition = new Vector2(correctElementScale.x * position.x,
            correctElementScale.y * position.y);
        element.localPosition = correctPosition;

        element.sizeDelta = correctElementScale;
    }
}
