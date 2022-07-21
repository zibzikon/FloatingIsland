using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class GridLayout : MonoBehaviour
{
    private RectTransform _rectTransform;
    public int Columns { get; private set; }
    public int Rows { get; private set; }

    private RectTransform[,] _elements;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(Vector2Int size)
    {
        Columns = size.x;
        Rows = size.y;
        
        _elements = new RectTransform[Columns, Rows];
    }

    public void SetElement(RectTransform layoutElement , Vector2Int position)
    {
        _elements[position.x, position.y] = layoutElement;
        
        CorrectElementsPositions();
    }

    private void CorrectElementsPositions()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        var localScale = _rectTransform.sizeDelta;
        
        for (var x = 0; x < Columns; x++)
        {
            for (var y = 0; y < Rows; y++)
            {
                var element = _elements[x, y];

                if (element == null) continue;
                
                var correctElementScale = new Vector3(localScale.x / Columns, localScale.y / Rows);

                element.sizeDelta = correctElementScale;
                
                var position = new Vector2( (correctElementScale.x * x) - localScale.x / 2+ correctElementScale.x / 2,
                    (correctElementScale.y  * y) - localScale.y /2 + correctElementScale.y /2);
                
                element.localPosition = position;
            }
        }
    }
}
