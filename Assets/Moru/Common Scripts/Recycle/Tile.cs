using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Grass = 500, Sand,Charge, Snow, Obstacle, Boundary
    }
    [SerializeField] private TileType type;
    private Collider2D Tilecollider;
    
    public void Start()
    {
        Tilecollider = GetComponent<Collider2D>();

        //타일의 소팅레이어 설정
        var renderer = GetComponent<TilemapRenderer>();
        renderer.sortingOrder = (int)type;

        privateInit();
    }

    private void privateInit()
    {
        switch (type)
        {
            case TileType.Grass:
                //Tilecollider.enabled = false;
                Tilecollider.isTrigger = true;
                    break;
            case TileType.Sand:
                Tilecollider.isTrigger = true;
                break;
            case TileType.Snow:
                Tilecollider.isTrigger = true;
                break;
            case TileType.Obstacle:
                Tilecollider.isTrigger = false ;
                Tilecollider.enabled = true;
                break;
            case TileType.Boundary:
                Tilecollider.enabled = true;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
