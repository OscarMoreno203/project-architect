using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Btn_MapEditorTile : MonoBehaviour
{
    DrawingManager drawingManager;
    Image img;
    [SerializeField] RuleTile myRTile;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return null;
        drawingManager = DrawingManager.scr;
        img = GetComponent<Image>();
        img.sprite = myRTile.m_DefaultSprite;
    }

    public void SelectTile()
    {
        drawingManager.OnSelectedTile(myRTile);
    }
}
