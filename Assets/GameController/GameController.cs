using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Transform _cloneSpawn;
    public float _timeBetweenSpawn;
    public ReplayTransform _playerPrefab;
    public List<ReplayTransform> _cloneList;

    private ReplayTransform _currentPlayer;
    private CameraFollowPlane _camera;



	// Use this for initialization
	void Start ()
    {
        _camera = GameObject.FindObjectOfType<CameraFollowPlane>();
        _cloneList.Clear();
        StartGame();
    }


    public void OnPlayerDied()
    {
        //temporarily move camera to spawn (so we will see previous clones launch as well)
        _camera._target = _cloneSpawn;

        //turn current player into a simple clone:
        _currentPlayer.GetComponent<PlayerInput>()._onPlayerDeath.RemoveListener(OnPlayerDied);
        _currentPlayer.gameObject.SetActive(false);
        _cloneList.Add(_currentPlayer);

        //launch all clones and instantiate new player:
        StartGame();
    }

    private void SpawnPlayer()
    {
        _currentPlayer = Instantiate<ReplayTransform>(_playerPrefab);
        _camera._target = _currentPlayer.transform;
        _currentPlayer.GetComponent<PlayerInput>()._onPlayerDeath.AddListener(OnPlayerDied);
        _currentPlayer.transform.position = _cloneSpawn.position;
        _currentPlayer.transform.rotation = _cloneSpawn.rotation;
        _currentPlayer.StartRecording();
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        for (int i=0; i<_cloneList.Count; ++i)
        {
            Debug.Log("Countdown " + (_cloneList.Count - i));
            _cloneList[i].gameObject.SetActive(true);
            _cloneList[i].transform.transform.position = _cloneSpawn.position;
            _cloneList[i].transform.transform.rotation = _cloneSpawn.rotation;
            _cloneList[i].StartReplay();
            yield return new WaitForSeconds(_timeBetweenSpawn);
        }

        Debug.Log("GO!");
        SpawnPlayer();
    }
}
