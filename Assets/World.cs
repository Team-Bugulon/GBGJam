using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    [Header("Geometry")]
    public int depth = 3;
    public int difficulty = 0;

    [InspectorButton("GenerateWorld")]
    public bool generate;

    [Header("References")]
    Tilemap tilemap;


    private void GenerateWorld()
    {

    }

}
