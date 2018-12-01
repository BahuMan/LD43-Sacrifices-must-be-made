using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlane : MonoBehaviour {

    public Transform _target;
    public Vector3 _offset;
    public Vector3 _curVelocity;
    public float _smoothTime = 0.5f;
    public float _rotationSmoothing = 0.5f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (!_target) return;

        Vector3 localOffset = _target.forward * _offset.z + _target.right * _offset.x + _target.up * _offset.y;
        transform.position = Vector3.SmoothDamp(transform.position, _target.position + localOffset, ref _curVelocity, _smoothTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, _rotationSmoothing);
	}
}
