using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ReplayTower : MonoBehaviour
{
    public enum RecordStatus { IDLE, RECORDING, REPLAY };
    public RecordStatus _status = RecordStatus.RECORDING;
    public UnityEvent _onStartReplay;
    public UnityEvent _onStopReplay;

    private float _startTime;
    private float _deathTimeRelative;

    public bool WasAlive()
    {
        switch (_status)
        {
            case RecordStatus.IDLE:
                StartReplay();
                return false;
            case RecordStatus.RECORDING:
                StartRecording();
                return true;
            case RecordStatus.REPLAY:
                StartReplay();
                return false;
            default:
                return false;
        }
    }

    public void StartRecording()
    {
        _startTime = Time.time;
        _status = RecordStatus.RECORDING;
    }

    public void StopRecording()
    {
        _deathTimeRelative = Time.time - _startTime;
        _status = RecordStatus.IDLE;
    }

    public void StartReplay()
    {
        this.gameObject.SetActive(true);
        _startTime = Time.time;
        _status = RecordStatus.REPLAY;
        _onStartReplay.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
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
        //for now, we're not recording the actual shots fired; the tower can just use its own logic
    }

    void UpdateReplay()
    {
        //for now, we're not replaying the actual shots fired; the tower can just use its own logic

        //check if the tower is past its time of death:
        if ((Time.time - _startTime) > _deathTimeRelative)
        {
            _status = RecordStatus.IDLE;
            _onStopReplay.Invoke();
        }
    }

}
