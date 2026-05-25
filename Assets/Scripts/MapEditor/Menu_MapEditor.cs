using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using NUnit.Framework;
//using UnityEngine.WSA;

public class Menu_MapEditor : MonoBehaviour
{
    [SerializeField] MapGrid mapGrid;
    [SerializeField] TMP_InputField inputField;

    Dictionary<Vector3Int, RuleTile> dicTiles = new Dictionary<Vector3Int, RuleTile> ();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MapData data = GameManager.scr.currentMapData;

        if (data == null)
        {
            data = new MapData();
            GameManager.scr.currentMapData = data;
        }
        if (data.name == "")
        {
            data.setName($"Map_{UnityEngine.Random.Range(100000, 999999)}");
        }

        print(data.name);
        inputField.text = data.name;

        foreach (TileData tile in data.tiles)
        {
            SetTile(tile.position, GameManager.scr.GetTile_ByName(tile.tileName));
        }
    }

    public void SetTile(Vector3Int pos, RuleTile rtile)
    {
        if (rtile == null)
        {
            dicTiles.Remove(pos);
            print(dicTiles.Count);
            return;
        }
        
        if (dicTiles.ContainsKey(pos))
        {
            dicTiles.Remove(pos);
        }
        
        dicTiles.Add(pos, rtile);
        print(dicTiles.Count);
    }

    public void Save()
    {
        MapData mapData = GameManager.scr.currentMapData;
        mapData.setName(inputField.text);
        
        // recorrer el diccionario
        mapData.tiles.Clear();
        foreach (Vector3Int pos in dicTiles.Keys)
        {
            var tile = dicTiles[pos];
            if (tile == null)
            {
                continue;
            }
            
            TileData data = new TileData
            {
                position = pos,
                tileName = tile.name
            };
            mapData.tiles.Add(data);
        }

        // 2. Recorrer el Tilemap (OBSOLETO, LO CONSERVO POR SI ACASO)
        /*
         * var tilemap = mapGrid.tilemap;
        mapData.tiles.Clear();
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                TileData data = new TileData
                {
                    position = pos,
                    tileName = tile.name
                };
                mapData.tiles.Add(data);
            }
        }*/
        GameManager.scr.SaveMap(mapData);
    }

    public void LoadScene_MainMenu()
    {
        GameManager.scr.LoadScene_MainMenu();
    }
}
