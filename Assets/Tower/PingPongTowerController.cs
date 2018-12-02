using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PingPongTowerController : MonoBehaviour
{

    public Rigidbody _projectilePrefab;
    public float _shootInterval = 1f;
    public float _pingPongSpeed = 2f;
    public float _maxGunDepression = 5f;
    public float _maxGunElevation = 70f;
    public float _minTurretRotation = 0f;
    public float _maxturretRotation = 20f;

    private float _turretDelta;
    private float _gunDelta;

    public UnityEvent _onDeath;

    [SerializeField]
    private Transform _cupola;
    [SerializeField]
    private Transform _barrel;
    [SerializeField]
    private Transform _barrelEnd;

    private float _projectileSpeed;
    private float _nextShotTime;

    public void Start()
    {
        _projectileSpeed = _projectilePrefab.GetComponent<ProjectileController>()._speed;
        _turretDelta = Mathf.DeltaAngle(_minTurretRotation, _maxturretRotation);
        _gunDelta = Mathf.DeltaAngle(-_maxGunDepression, _maxGunElevation);
    }

    public void Reset()
    {
        _cupola = this.transform.Find("Cupola");
        _barrel = _cupola.Find("Barrel");
        _barrelEnd = _barrel.Find("BarrelEnd");
    }

    // Update is called once per frame
    void Update()
    {

        if (_turretDelta > 0.1f) _cupola.localRotation = Quaternion.Euler(0f, Mathf.PingPong(Time.time * _pingPongSpeed, _turretDelta) + _minTurretRotation, 0f);
        if (_gunDelta > 0.1f) _barrel.localRotation = Quaternion.Euler(-Mathf.PingPong(Time.time * _pingPongSpeed, _gunDelta) + _maxGunDepression, 0f, 0f);

        if (Time.time > _nextShotTime)
        {
            Rigidbody rb = Instantiate<Rigidbody>(_projectilePrefab, _barrelEnd.position, _barrelEnd.rotation);
            rb.velocity = _barrelEnd.forward * _projectileSpeed;
            _nextShotTime += _shootInterval;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Tower checking collision with " + collision.gameObject.name);
        Rigidbody RootRigid = collision.transform.GetComponentInParent<Rigidbody>();
        GameObject RootObject;

        //even though plane has a rigidbody, the GetComponentInParent can't find it
        //because only active components on active objects are found
        //I guess the collision trigger on plane caused it to be deactivated before this trigger was called
        if (RootRigid == null) RootObject = collision.gameObject;
        else RootObject = RootRigid.gameObject;


        if (RootObject.CompareTag("Player"))
        {
            RequestDeath();
        }
    }

    public void RequestDeath()
    {
        _onDeath.Invoke();
        this.gameObject.SetActive(false);
    }

    public void OnStartReplay()
    {
        _nextShotTime = Time.time;
        this.gameObject.SetActive(true);
    }

}
