using UnityEngine;
using UnityEngine.Events;

public class HQScript : MonoBehaviour {

    public UnityEvent _onDeath;

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HQ checking collision with " + collision.gameObject.name);
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
}
