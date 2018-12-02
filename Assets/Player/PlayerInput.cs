using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour {

    public float _speedForward = 1.0f;
    public float _speedPitch = 180.0f;
    public float _speedYaw = 90.0f;

    public UnityEvent _onPlayerDeath;
    public Material _clonedMaterial;

    private Rigidbody _rigid;
    private float _rotX = 0;
    private float _rotY = 0;

	// Use this for initialization
	void Start () {
        _rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        _rigid.velocity = transform.forward * _speedForward;
        //transform.position = transform.position + transform.forward * _speedForward * Time.deltaTime;


        _rotX = _speedPitch * Input.GetAxis("Vertical") * Time.deltaTime;
        _rotY = _speedYaw * Input.GetAxis("Horizontal") * Time.deltaTime;
        //Quaternion rot = Quaternion.Euler(_rotX, _rotY, 0);
        //transform.rotation = transform.rotation * rot;
        //Vector3 localangularvelocity = transform.InverseTransformDirection(_rigid.angularVelocity);
        _rigid.AddRelativeTorque(_rotX, _rotY, 0);
        transform.localRotation = transform.localRotation * Quaternion.Euler(0, 0, -2*_rotY);
	}

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("player crashed into " + collision.gameObject.name);
        _onPlayerDeath.Invoke();
        GetComponent<Rigidbody>().isKinematic = true;
        this.enabled = false;
    }

    public void StopPlayerInput()
    {
        foreach (var m in GetComponentsInChildren<MeshRenderer>())
        {
            m.material = _clonedMaterial;
        }
        this.enabled = false;
    }
}
