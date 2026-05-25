using System.Collections;
using UnityEngine;

public class V3D_WallPrint : MonoBehaviour
{
    [SerializeField] Transform transformTarget;

    private void OnEnable()
    {
        GameManager.scr.OnV3DGenerated += CheckWalls;
    }

    private void OnDisable()
    {
        GameManager.scr.OnV3DGenerated -= CheckWalls;
    }

    void CheckWalls()
    {
        Vector3 origin = transformTarget.position;
        float flSize = transform.lossyScale.x;

        Collider[] paredes = Physics.OverlapBox(origin, Vector3.one * 0.2f * flSize);

        if (paredes.Length > 0)
        {
            print($"Posicion de la pared: {transform.position}, Posicion del overlap: {origin}, Paredes detectadas: {paredes.Length}");
            //print("colision detectada !!!!!");
            Destroy(gameObject);
        }
    }
}
