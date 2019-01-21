using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{

    public UnityEvent _onPlayerDeath;

    void OnCollisionEnter(Collision collision)
    {
        if (!this.enabled)
        {
            Debug.Log("ignoring collision during replay");
            return;
        }

        Debug.Log("player crashed into " + collision.gameObject.name);
        _onPlayerDeath.Invoke();
        GetComponent<Rigidbody>().isKinematic = true;
        this.enabled = false;
    }
}
