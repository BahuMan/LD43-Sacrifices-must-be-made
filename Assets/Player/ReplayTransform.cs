using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReplayTransform : MonoBehaviour {

    [System.Serializable]
    private class SnapShot
    {
        public SnapShot() { }
        public SnapShot(float stime, Vector3 sposition, Quaternion srotation)
        {
            time = stime;
            pos = sposition;
            rot = srotation;
        }
        public float time;
        public Vector3 pos;
        public Quaternion rot;
    }

    public enum RecordStatus { IDLE, RECORDING, REPLAY};
    public RecordStatus _status = RecordStatus.IDLE;
    public float _snapShotInterval = 0.1f;
    public UnityEvent _onStartReplay;
    public UnityEvent _onStopReplay;

    private float _startTime;
    private float _nextSnapShotTime;
    private int _replayIndex = 0;
    private List<SnapShot> _snapShotList = new List<SnapShot>();
	
    public void StartRecording()
    {
        _nextSnapShotTime = _startTime = Time.time;
        _snapShotList.Clear();
        _status = RecordStatus.RECORDING;
    }

    public void StopRecording()
    {
        _status = RecordStatus.IDLE;
    }

    public void StartReplay()
    {
        _onStartReplay.Invoke();
        _replayIndex = 1;
        _startTime = Time.time;
        _status = RecordStatus.REPLAY;
    }

	// Update is called once per frame
	void Update () {
        switch (_status)
        {
            case RecordStatus.RECORDING:
                UpdateRecord();
                break;
            case RecordStatus.REPLAY:
                UpdateReplay();
                break;
            default:
                break;
        }
    }

    void UpdateRecord()
    {
        if (Time.time >  _nextSnapShotTime)
        {
            _snapShotList.Add(new SnapShot(Time.time - _startTime, transform.position, transform.rotation));
            _nextSnapShotTime += _snapShotInterval;
        }
    }

    void UpdateReplay()
    {
        //calculate current timestamp, relative to start of the replay:
        float relativeTime = Time.time - _startTime;

        //advance index for snapshots if time has moved on:
        while (_replayIndex < _snapShotList.Count && _snapShotList[_replayIndex].time < relativeTime)
        {
            _replayIndex++;
        }

        //stop replay if we're at the end:
        if (_replayIndex >= _snapShotList.Count)
        {
            _status = RecordStatus.IDLE;
            _onStopReplay.Invoke();
            return;
        }

        SnapShot last = _snapShotList[_replayIndex - 1];
        SnapShot next = _snapShotList[_replayIndex];

        float interpolation = Mathf.InverseLerp(last.time, next.time, relativeTime);
        transform.position = Vector3.Lerp(last.pos, next.pos, interpolation);
        transform.rotation = Quaternion.Slerp(last.rot, next.rot, interpolation);

    }
}
