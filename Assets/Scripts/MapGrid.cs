using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGrid : MonoBehaviour
{
    public Tilemap tilemap;

    private void Start()
    {
        MapData map = GameManager.scr.currentMapData;
        if (map != null)
        {
            SetupMap(map);
        }
    }

    public void SetupMap(MapData mapData)
    {
        tilemap.ClearAllTiles();

        print(mapData.tiles);
        foreach (var data in mapData.tiles)
        {
            // Nota: Esto requiere que los tiles estén en Resources o 
            // mapeados por nombre
            string tilepath = "Tiles/AutoTiles/" + data.tileName;
            RuleTile tile = Resources.Load<RuleTile>(tilepath);
            print(tilepath);
            print(tile);
            tilemap.SetTile(data.position, tile);
        }
        Debug.Log("Mapa cargado");
    }
}
