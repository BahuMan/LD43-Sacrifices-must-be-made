using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    public Transform _target;
    public Rigidbody _projectilePrefab;
    public float _shootInterval = 1f;
    public float _targetInterval = 5f;

    [SerializeField]
    private Transform _cupola;
    [SerializeField]
    private Transform _barrel;
    [SerializeField]
    private Transform _barrelEnd;

    private float _projectileSpeed;
    private float _nextShotTime;
    private float _nextTargetTime;
    public List<Transform> _potentialTargets;

    public void Start()
    {
        _projectileSpeed = _projectilePrefab.GetComponent<ProjectileController>()._speed;
        _potentialTargets = new List<Transform>();
    }

    public void Reset()
    {
        _target = null;
        _cupola = this.transform.Find("Cupola");
        _barrel = _cupola.Find("Barrel");
        _barrelEnd = _barrel.Find("BarrelEnd");
    }

	// Update is called once per frame
	void Update () {
        if (!_target) return;

        Vector3 Aimpoint = FirstOrderIntercept(transform.position, Vector3.zero, _projectileSpeed, _target.position, _target.GetComponent<Rigidbody>().velocity);
        Vector3 angles = Quaternion.LookRotation(Aimpoint - transform.position).eulerAngles;

        _cupola.rotation = Quaternion.Euler(0f, angles.y, 0f);
        _barrel.localRotation = Quaternion.Euler(angles.x, 0f, 0f);

        if (Time.time > _nextShotTime)
        {
            Rigidbody rb = Instantiate<Rigidbody>(_projectilePrefab, _barrelEnd.position, _barrelEnd.rotation);
            rb.velocity = _barrelEnd.forward * _projectileSpeed;
            _nextShotTime += _shootInterval;
        }

        if (Time.time > _nextTargetTime)
        {
            if (_potentialTargets.Count == 0)
            {
                _target = null;
            }
            else
            {
                _target = _potentialTargets[Random.Range(0, _potentialTargets.Count)];
                _nextTargetTime += _targetInterval;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rootRigid = other.GetComponentInParent<Rigidbody>();
        if (rootRigid == null) return;

        GameObject rootObject = rootRigid.gameObject;

        if (rootObject.CompareTag("Player"))
        {
            Debug.Log("Player detected :" + rootObject.name);
            if (!_potentialTargets.Contains(rootObject.transform))
            {
                _potentialTargets.Add(rootObject.transform);
            }
            if (_target == null || !_target.gameObject.activeSelf)
            {
                _target = rootObject.transform;
                _nextShotTime = Time.time;
                _nextTargetTime = Time.time + _targetInterval;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rootRigid = other.GetComponentInParent<Rigidbody>();
        if (rootRigid == null) return;

        GameObject rootObject = rootRigid.gameObject;
        Debug.Log("Lost track of " + rootObject.name);
        _potentialTargets.Remove(rootObject.transform);
        if (rootObject.transform == _target) _nextTargetTime = Time.time-1;
    }

    //first-order intercept using absolute target position
    private static Vector3 FirstOrderIntercept
    (
        Vector3 shooterPosition,
        Vector3 shooterVelocity,
        float shotSpeed,
        Vector3 targetPosition,
        Vector3 targetVelocity
    )
    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }

    //first-order intercept using relative target position
    private static float FirstOrderInterceptTime
    (
        float shotSpeed,
        Vector3 targetRelativePosition,
        Vector3 targetRelativeVelocity
    )
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }
}
