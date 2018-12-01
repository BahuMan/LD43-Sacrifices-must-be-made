using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    public float _speed = 20f;
    public float _fuseTime = 10f;

    private float _destructionTime;

	// Use this for initialization
	void Start () {
        _destructionTime = Time.time + _fuseTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > _destructionTime)
        {
            Destroy(this.gameObject);
        }
	}
}
