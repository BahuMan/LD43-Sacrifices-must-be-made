using UnityEngine;

public class OverviewCamera : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0, Time.deltaTime * 5f, 0, Space.World);
	}
}
