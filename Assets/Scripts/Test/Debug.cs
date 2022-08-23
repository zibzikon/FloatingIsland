using System;
using UnityEngine;

namespace Test
{
    public class Debug : MonoBehaviour
    {
        [SerializeField] private IsometricTilemap2_5D _tilemap;
        [SerializeField] private Tile _testTilePrefab;

        [SerializeField] private bool _spawnAndSetTile;
        [SerializeField] private Vector3Int _position;

        private void Update()
        {
            if (!_spawnAndSetTile) return;
            _spawnAndSetTile = false;
            _tilemap.SetTile(Instantiate(_testTilePrefab), _position);
        }
    }
}