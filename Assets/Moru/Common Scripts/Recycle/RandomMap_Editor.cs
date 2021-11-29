using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMap_Editor : SingleTone<RandomMap_Editor>
{
    public Tilemap BackgroundMap;
    public List<GameObject> Tiles;

    public Tilemap BoundaryTileMap;
    public List<GameObject> BoundaryTiles;

    public Tilemap ObstacleMap;
    public List<GameObject> Obstacles;

    [Header("맵 사이즈")]
    public int Width;
    public int Height;

    public int obstacle_Min_Count;
    public int obstacle_Max_Count;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        //Vector3int vector = new Vector3(0, 0);
        ////타일 스폰////
        Vector3Int vector = new Vector3Int(0, 0, 0);
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                Vector3 Postion = new Vector3((-Width * 0.5f + 0.5f) + x, (Height * 0.5f - 0.5f) - y, 0);
                if (x == Width - 1 || y == Height -1 || x ==0 || y == 0)
                {
                    SpawnTile(Tile.TileType.Boundary, Postion, BoundaryTileMap.transform);
                }
                else
                {
                    SpawnTile(Tile.TileType.Grass, Postion, BackgroundMap.transform);
                }
            }
        }
        ///랜덤장애물 스폰///
        SetObstacle();
    }

    private void SpawnTile(Tile.TileType type, Vector3 Position, Transform _parent)
    {
        GameObject clon = null;
        if (type == Tile.TileType.Grass)
        {
            clon = Instantiate(Tiles[0], Position, Quaternion.identity);
        }
        else if (type == Tile.TileType.Boundary)
        {
            clon = Instantiate(BoundaryTiles[0], Position, Quaternion.identity);
        }
        else if ( type == Tile.TileType.Obstacle)
        {
            clon = Instantiate(Obstacles[0], Position, Quaternion.identity);
        }

        clon.name = $"Tile {type.ToString()}";
    }

    private void SetObstacle()
    {
        var RandomValue = Random.Range(obstacle_Min_Count, obstacle_Max_Count);
        for(int i = 0; i < RandomValue; i++)
        {
            int RandomX = Random.Range(3, Width-3);
            int RandomY = Random.Range(3, Height-3);
            Vector3 ObstaclePost = new Vector3((-Width * 0.5f + 0.5f) + RandomX, (Height * 0.5f - 0.5f)-RandomY, 0);
            SpawnTile(Tile.TileType.Obstacle, ObstaclePost, ObstacleMap.transform);
        }
    }

}
