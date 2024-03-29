﻿
using UnityEngine;

public static class GeneralGameSettings
{
    public const bool DebugMode = false;
    public static bool RayCastIsBlocked;
    public static int Difficulty = 1;
    public static class ContainerSettings
    {
        public const int MaxHorizontalSize = 9;
        public const int DefaultRowsCount = 4;
    }
    public static class GameFieldSettings
    {
        public const int WorldPositionMultiplier = 2;
    }
    public static class Tilemap
    {
        public static readonly Vector2 TileSize = new (1, 0.5f);
    }
}
