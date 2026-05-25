using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

[System.Serializable]
public class DicTilePrefab
{
    public DicTilePrefab_Key[] keys;

    public Dictionary<RuleTile, GameObject> dic()
    {
        Dictionary<RuleTile, GameObject> result = new Dictionary<RuleTile, GameObject>();
        foreach (DicTilePrefab_Key key in keys)
        {
            result.Add(key.key, key.value);
        }
        return result;
    }
}

[System.Serializable]
public class DicTilePrefab_Key
{
    public RuleTile key = null;
    public GameObject value = null;
}

public class V3D_MapBuilder : MonoBehaviour
{
    [SerializeField] DicTilePrefab dic_TilePrefab;
    [SerializeField] Transform transformParent;
    [SerializeField] float flMapScale = 0.01f;
    [SerializeField] float flItemsScale = 0.6f;

    // public event Action AskForCollisionDetection; < - DESCARTADO

    MapData mapData = null;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return null;

        mapData = GameManager.scr.currentMapData;
        List<TileData> listTiles = mapData.tiles;

        Dictionary<RuleTile, GameObject> dicTilePrefab = dic_TilePrefab.dic();
        Dictionary<string, RuleTile> dicNameTile = new Dictionary<string, RuleTile>();

        float leftX = 0, rightX = 0, topY = 0, bottomY = 0;
        float flHalfTile = flItemsScale * 0.5f;

        // ------ Obtener informacion de tamaño del mapdata
        foreach (TileData tileData in listTiles)
        {
            // Guardar los datos de posicion de este tile para futuros calculos
            if (leftX == 0)
            { // si no se ha registrado nada todavia
                leftX = rightX = tileData.position.x;
                topY = bottomY = tileData.position.y;
            }
            else
            {
                var tilePos = tileData.position;

                if (tilePos.x < leftX) { leftX = tilePos.x; }
                if (tilePos.x > rightX) { rightX = tilePos.x; }
                if (tilePos.y < bottomY) { bottomY = tilePos.y; }
                if (tilePos.y > topY) { topY = tilePos.y; }
            }
            yield return null;
        }

        // ------ Ajustar valores
        leftX -= 1f;
        rightX += 2f;
        bottomY -= 1f;
        topY += 2f;

        // ------ Redimensionar
        float mapX = rightX - leftX;
        float mapY = topY - bottomY;
        float longestAxis = (mapX > mapY) ? mapX : mapY;

        float scaleVal = flMapScale / longestAxis;
        Vector3 v3Scale = new Vector3(scaleVal, scaleVal, scaleVal);
        transformParent.localScale = v3Scale;

        // ------ Reposicionar
        print($"{leftX}, {rightX} - {bottomY}, {topY}");
        float centerX = Mathf.Lerp(rightX, leftX, 0.5f);
        float centerY = Mathf.Lerp(bottomY, topY, 0.5f);
        Vector3 v3MapCenter = new Vector3(centerX, centerY);
        print("Posicion central en tiles: " + v3MapCenter);
        v3MapCenter *= flItemsScale;
        print("Posicion central real: " + v3MapCenter);
        transform.localPosition = -v3MapCenter;

        // ------ Construir modelo 3D
        foreach (TileData tileData in listTiles)
        {
            // - Obtener tile
            string tileName = tileData.tileName;
            RuleTile tile;
            if (dicNameTile.ContainsKey(tileName))
            {
                tile = dicNameTile[tileName];
            }
            else
            {
                string tilepath = "Tiles/AutoTiles/" + tileData.tileName;
                tile = Resources.Load<RuleTile>(tilepath);
            }

            // - Si no hau un prefab asignado para este tile, lo demas se ignora
            if (!dicTilePrefab.ContainsKey(tile))
            {
                continue;
            }

            GameObject prefab = dicTilePrefab[tile];
            if (prefab == null)
            {
                continue;
            }

            // - Instanciar el prefab
            GameObject obj = Instantiate(prefab, transform);

            Vector3 v3Euler = Vector3.Lerp(transformParent.eulerAngles, -transformParent.eulerAngles, 0.5f);
            obj.transform.eulerAngles = v3Euler;

            obj.transform.localPosition = new Vector3(
                (tileData.position.x * flItemsScale) + flHalfTile,
                (tileData.position.y * flItemsScale) + flHalfTile,
                0f);
            obj.transform.localScale = obj.transform.localScale * flItemsScale;

            yield return null;
        }

        // Finalizar
        GameManager.scr.V3DGenerated();
    }
}
