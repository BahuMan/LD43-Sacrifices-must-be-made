using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

    public GameObject _explosionPrefab;

    public void StartExplosion()
    {
        GameObject go = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }

}
