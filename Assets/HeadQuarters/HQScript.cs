using UnityEngine;
using UnityEngine.Events;

public class HQScript : MonoBehaviour {

    public UnityEvent _onDeath;

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("HQ checking collision with " + collision.gameObject.name);
        GameObject RootObject = collision.gameObject.transform.root.gameObject;

        if (RootObject.CompareTag("Player"))
        {
            Debug.LogError("player found!");
            RequestDeath();
        }
    }

    public void RequestDeath()
    {
        _onDeath.Invoke();
        this.gameObject.SetActive(false);
    }
}
