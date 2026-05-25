using UnityEngine;
using TMPro;

public class MainMenu_Btn_Map : MonoBehaviour
{
    public MapData myMapData;
    public TMP_Text tmpName;

    public void Setup(MapData map)
    {
        myMapData = map;
        tmpName.text = myMapData.name;
    }

    public void OnPress()
    {
        GameManager.scr.SetMapData(myMapData);
    }
}
