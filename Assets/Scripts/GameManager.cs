using UnityEngine;
using System.IO;
//using UnityEditor.Overlays;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TileData
{
    public Vector3Int position;
    public string tileName;
}

[System.Serializable]
public class MapData
{
    public string name;
    public string filepath;
    public List<TileData> tiles = new List<TileData>();

    public MapData()
    {
        // nada xdxd
    }

    public void setName(String name)
    {
        this.name = name;
        filepath = String.Format(GameManager.scr.saveFolderPath + "/{0}.json", name);
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager scr;
    public string saveFolderPath;
    public MapData currentMapData;

    public event Action OnMapDataSelected;
    public event Action OnV3DGenerated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (scr) {
            Destroy(this);
            return;
        }
        else
        {
            scr = this;
            DontDestroyOnLoad(this);
        }

        saveFolderPath = Application.persistentDataPath + "/maps";
    }

    public void SetMapData(MapData map)
    {
        currentMapData = map;
        OnMapDataSelected?.Invoke();
    }

    public void V3DGenerated()
    {
        OnV3DGenerated.Invoke();
    }

    public void SaveMap(MapData mapData)
    {
        currentMapData = mapData;
        
        // Serializar
        string json = JsonUtility.ToJson(mapData);

        // Guardar archivo
        string filepath = mapData.filepath;
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
            Debug.Log("Directorio creado: " + saveFolderPath);
        }

        using (var stream = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        using (var writer = new StreamWriter(stream))
        {
            writer.Write(json);
        }
        Debug.Log("Mapa guardado en: " + saveFolderPath);
    }

    public MapData LoadMap(String path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MapData mapData = JsonUtility.FromJson<MapData>(json);
            return mapData;
        }
        return null;
    }

    public RuleTile GetTile_ByName(string name)
    {
        string tilepath = "Tiles/AutoTiles/" + name;
        RuleTile tile = Resources.Load<RuleTile>(tilepath);
        return tile;
    }
    public void LoadScene(String scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadScene_Editor()
    {
        LoadScene("scn_Editor");
    }

    public void LoadScene_3DView()
    {
        LoadScene("scn_3DView");
    }

    public void LoadScene_MainMenu()
    {
        LoadScene("scn_MainMenu");
    }


}
