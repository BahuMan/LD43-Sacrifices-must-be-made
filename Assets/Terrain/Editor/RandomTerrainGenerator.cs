using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RandomTerrainGenerator : ScriptableWizard
{

    //The higher the numbers, the more hills/mountains there are
    private float HM;

    //The lower the numbers in the number range, the higher the hills/mountains will be...
    private float divRange;

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
            //The higher the numbers, the more hills/mountains there are
            HM = 4;

            //The lower the numbers in the number range, the higher the hills/mountains will be...
            divRange = 2;
            GenerateTerrain(G.GetComponent<Terrain>(), HM);
        }
    }

    //Our Generate Terrain function
    public void GenerateTerrain(Terrain t, float tileSize)
    {

        //Heights For Our Hills/Mountains
        float[,] hts = new float[t.terrainData.heightmapWidth, t.terrainData.heightmapHeight];
        for (int i = 0; i < t.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < t.terrainData.heightmapHeight; k++)
            {
                hts[i, k] = Mathf.PerlinNoise(((float)i / (float)t.terrainData.heightmapWidth) * tileSize, ((float)k / (float)t.terrainData.heightmapHeight) * tileSize) / divRange;
            }
        }
        Debug.LogWarning("DivRange: " + divRange + " , " + "HTiling: " + HM);
        t.terrainData.SetHeights(0, 0, hts);
    }
}