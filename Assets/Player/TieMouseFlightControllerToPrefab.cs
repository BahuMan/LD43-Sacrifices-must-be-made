using UnityEngine;
using System.Collections;

public class TieMouseFlightControllerToPrefab : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        MFlight.MouseFlightController mfc = GameObject.FindObjectOfType<MFlight.MouseFlightController>();
        mfc.aircraft = this.transform;
        MFlight.Demo.Plane p = this.GetComponent<MFlight.Demo.Plane>();
        p.controller = mfc;
        p.enabled = true;
    }

}
