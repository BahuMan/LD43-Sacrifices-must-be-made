using UnityEngine;
using System.Collections;

public class ChangeMaterial : MonoBehaviour
{
    public Material _clonedMaterial;

    public void ChangeToMaterial()
    {
        foreach (var m in GetComponentsInChildren<MeshRenderer>())
        {
            m.material = _clonedMaterial;
        }
    }
}
