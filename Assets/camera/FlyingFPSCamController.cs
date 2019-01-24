using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingFPSCamController : MonoBehaviour
{

    public float turn_speed = 1;
    public float pitch_speed = 1;
    public float forward_speed = 10;
    public float strafe_speed = 10;

    private float _currentRotX = 0;
    private float _currentRotY = 0;

    private void Start()
    {
        _currentRotX = transform.rotation.eulerAngles.x;
        _currentRotY = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        _currentRotX += Input.GetAxis("Mouse Y") * -pitch_speed;
        _currentRotY += Input.GetAxis("Mouse X") * turn_speed;

        this.transform.rotation = Quaternion.Euler(_currentRotX, _currentRotY, 0);

        this.transform.Translate(Input.GetAxis("Horizontal") * strafe_speed, 0, Input.GetAxis("Vertical") * forward_speed, Space.Self);
    }
}
