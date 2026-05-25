using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DrawingManager : MonoBehaviour
{
    public static DrawingManager scr;
    
    enum DrawingTool
    {
        PAINT, ERASE, MOVE
    }
    DrawingTool drawingTool = DrawingTool.PAINT;

    [SerializeField] Menu_MapEditor mapEditor;
    [SerializeField] Camera camera;
    public Tilemap tilemap;
    public MapGrid mapGrid;
    public RuleTile penTile; // El sprite que se va a pintar

    bool blActing = false;

    private void OnEnable()
    {
        scr = this;
    }

    void Update()
    {
        if (Input.touchCount > 0) // Clic sostenido
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector3Int gridPos = tilemap.WorldToCell(mouseWorldPos);

            switch (drawingTool)
            {
                case DrawingTool.PAINT:
                    tilemap.SetTile(gridPos, penTile);
                    mapEditor.SetTile(gridPos, penTile);
                    break;

                case DrawingTool.ERASE:
                    tilemap.SetTile(gridPos, null);
                    mapEditor.SetTile(gridPos, null);
                    break;

                case DrawingTool.MOVE:
                    StartCoroutine(ienMoveCamera());
                    break;
            }
        }
    }

    public void OnSelectedTile(RuleTile tile)
    {
        penTile = tile;
    }

    public void DrawingTool_Paint()
    {
        drawingTool = DrawingTool.PAINT;
    }

    public void DrawingTool_Erase()
    {
        drawingTool = DrawingTool.ERASE;
    }

    public void DrawingTool_Move()
    {
        drawingTool = DrawingTool.MOVE;
    }

    IEnumerator ienMoveCamera()
    {
        if (blActing)
        {
            yield break;
        }
        blActing = true;

        float flDragSpeed = 0.5f;
        Vector3 mouseOriginalPos = camera.ScreenToWorldPoint(Input.GetTouch(0).position);
        while (Input.touchCount > 0)
        {
            Vector3 mousePos = camera.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector3 direction = mouseOriginalPos - mousePos;
            camera.transform.position += direction * flDragSpeed;
            yield return null;
        }

        blActing = false;
    }
}
