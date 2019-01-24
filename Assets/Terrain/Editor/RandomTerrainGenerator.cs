using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RandomTerrainGenerator : ScriptableWizard
{

    [Tooltip("The higher the numbers, the more hills/mountains there are")]
    public float HM = 4;

    [Tooltip("The lower the numbers in the number range, the higher the hills/mountains will be...")]
    public float divRange = 2;

    [Tooltip("False to overwrite terrain, true to put layer on top of existing terrain")]
    public bool additive = false;

    public AnimationCurve frequencyDropOff;
    public AnimationCurve AmplitudeDropOff;

    [MenuItem("Terrain/Generate Random Terrain")]
    public static void CreateWizard(MenuCommand command)
    {
        ScriptableWizard.DisplayWizard("Generate Random Terrain", typeof(RandomTerrainGenerator));
    }

    void OnWizardCreate()
    {
        GameObject G = Selection.activeGameObject;
        if (G.GetComponent<Terrain>())
        {
            GenerateTerrain(G.GetComponent<Terrain>(), HM);
        }
    }

    //Our Generate Terrain function
    public void GenerateTerrain(Terrain t, float tileSize)
    {

        //Heights For Our Hills/Mountains
        float[,] hts;
        if (additive)
        {
            hts = t.terrainData.GetHeights(0, 0, t.terrainData.heightmapWidth, t.terrainData.heightmapHeight);
        }
        else
        {
            hts = new float[t.terrainData.heightmapWidth, t.terrainData.heightmapHeight];
        }

        for (int i = 0; i < t.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < t.terrainData.heightmapHeight; k++)
            {
                hts[i, k] += Mathf.PerlinNoise(((float)i / (float)t.terrainData.heightmapWidth) * tileSize, ((float)k / (float)t.terrainData.heightmapHeight) * tileSize) / divRange;
            }
        }
        Debug.LogWarning("DivRange: " + divRange + " , " + "HTiling: " + HM);
        t.terrainData.SetHeights(0, 0, hts);
    }
}