using UnityEngine;
using System.Collections.Generic;

public class DestructionParticles : MonoBehaviour
{

    public List<GameObject> _prefabsToInstantiateOnDestruction;

    public void OnDeath()
    {
        foreach (GameObject prefab in _prefabsToInstantiateOnDestruction)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}
