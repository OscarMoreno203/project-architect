using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject buttonPrefab; 
    public Transform container;

    [SerializeField] Button btnNew, btnEdit, btnDelete, btnV3D;

    private Dictionary<string, MainMenu_Btn_Map> dicButtons = new Dictionary<string, MainMenu_Btn_Map>();

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        CreateButtonsFromJSON();
        EnableEditButtons(false);
    }

    private void OnEnable()
    {
        StartCoroutine(ienOnEnable());
    }

    IEnumerator ienOnEnable()
    {
        do
        {
            yield return null;
        } while (GameManager.scr == null);
        GameManager.scr.OnMapDataSelected += OnSelectedMap;
    }

    private void OnDisable()
    {
        GameManager.scr.OnMapDataSelected -= OnSelectedMap;
    }

    void CreateButtonsFromJSON()
    {
        // 1. Obtener ruta
        string path = GameManager.scr.saveFolderPath;

        // 2. Verificar si la carpeta existe
        if (!Directory.Exists(path))
        {
            Debug.LogError("Carpeta no encontrada: " + path);
            return;
        }

        // 3. Obtener todos los archivos .json
        string[] files = Directory.GetFiles(path, "*.json");

        foreach (string file in files)
        {
            string jsonString = File.ReadAllText(file);
            MapData mapData = JsonUtility.FromJson<MapData>(jsonString);
            MainMenu_Btn_Map newButton = Instantiate(buttonPrefab, container).GetComponent<MainMenu_Btn_Map>();
            newButton.Setup(mapData);
            dicButtons.Add(mapData.name, newButton);
        }
    }

    void EnableEditButtons(bool bl)
    {
        btnEdit.enabled = btnDelete.enabled = btnV3D.enabled = bl;
    }

    public void OnSelectedMap()
    {
        EnableEditButtons(true);
    }

    public void btnPress_New()
    {
        GameManager.scr.SetMapData(null);
        GameManager.scr.LoadScene_Editor();
    }

    public void btnPress_Delete()
    {
        if (GameManager.scr.currentMapData != null)
        {
            var map = GameManager.scr.currentMapData;
            var name = map.name;
            MainMenu_Btn_Map btn = dicButtons[name];
            Destroy(btn.gameObject);
            File.Delete(map.filepath);

            GameManager.scr.SetMapData(null);
            EnableEditButtons(false);
        }
    }

    public void LoadScene_Edit()
    {
        GameManager.scr.LoadScene_Editor();
    }

    public void LoadScene_3DView()
    {
        GameManager.scr.LoadScene_3DView();
    }
}
