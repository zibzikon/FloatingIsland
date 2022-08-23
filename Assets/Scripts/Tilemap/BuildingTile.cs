using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Space]
    [SerializeField]
    private Vector2 _tileSize;
    
    [SerializeField] 
    private Vector2 _anchorPosition;

    [SerializeField]private bool _correctTile;

    [SerializeField] private BuildingType buildingType;
    public BuildingType BuildingType => buildingType;
    
    private void OnValidate()
    {
        if (_correctTile)
        {
            CorrectSpritePosition();
            CorrectTileSize();
            _correctTile = false;
        }
    }

    public void SetTileDepth(int depth)
    {
        _spriteRenderer.sortingOrder = depth;
    }

    private void CorrectSpritePosition()
    {
        var path = AssetDatabase.GetAssetPath(_spriteRenderer.sprite.texture);
        var textureImporter = (TextureImporter)AssetImporter.GetAtPath(path);
        textureImporter.spritePivot = _anchorPosition;
        var texSettings = new TextureImporterSettings();
        textureImporter.ReadTextureSettings(texSettings);
        texSettings.spriteAlignment = (int)SpriteAlignment.Custom;
        textureImporter.SetTextureSettings(texSettings);
        textureImporter.SaveAndReimport();
    }

    private void CorrectTileSize()
    {
        _spriteRenderer.transform.localScale = new Vector3(0.2f, 0.2f);
    }
    
    public void SetWorldPosition(Vector3 position)
    {
        transform.position = position;
    }
    
    public void Destroy()
    {
       Destroy(gameObject);
    }
}