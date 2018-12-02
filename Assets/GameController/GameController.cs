﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Transform _cloneSpawn;
    public float _timeBetweenSpawn;
    public ReplayTransform _playerPrefab;
    private List<ReplayTransform> _cloneList;
    private ReplayTower[] _towerList;
    //private HQScript[] _hqList;

    private ReplayTransform _currentPlayer;
    private CameraFollowPlane _camera;
    private OverviewCamera _overview;

	// Use this for initialization
	void Start ()
    {
        _camera = GameObject.FindObjectOfType<CameraFollowPlane>();
        _overview = GameObject.FindObjectOfType<OverviewCamera>();
        _overview.gameObject.SetActive(false);

        _cloneList = new List<ReplayTransform>();
        _towerList = GameObject.FindObjectsOfType<ReplayTower>();
        //_hqList = GameObject.FindObjectsOfType<HQScript>();
        StartGame();
    }


    public void OnPlayerDied()
    {
        //temporarily move camera to spawn (so we will see previous clones launch as well)
        _camera._target = null;
        _camera.transform.position = _cloneSpawn.position + _camera._offset * _cloneList.Count;
        _camera.transform.rotation = Quaternion.identity;

        //turn current player into a simple clone:
        _currentPlayer.GetComponent<PlayerInput>()._onPlayerDeath.RemoveListener(OnPlayerDied);
        _currentPlayer.gameObject.SetActive(false);
        _cloneList.Add(_currentPlayer);

        StartGame();
    }

    private void StartOverview()
    {
        _camera.gameObject.SetActive(false);
        _overview.gameObject.SetActive(true);
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
        //launch all clones and lastly the player with a 1 second pause inbetween:
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        //wait one frame so all tower statuses are up to date:
        yield return null;

        if (_cloneList.Count == 0)
        {
            MessageUI.Showtext("You are an AI", 2);
            yield return new WaitForSeconds(2);
            MessageUI.Showtext("Destroy the AA guns by flying into them", 2);
            yield return new WaitForSeconds(2);
            MessageUI.Showtext("You will Die.\nRepeatedly", 2);
            yield return new WaitForSeconds(2);
            MessageUI.Showtext("But sacrifices must be made", 2);
            yield return new WaitForSeconds(2);
        }

        //restart all towers:
        bool gameover = true;
        foreach (var t in _towerList) if (t.WasAlive()) gameover = false;

        for (int i=0; i<_cloneList.Count; ++i)
        {
            MessageUI.Showtext("Countdown " + (_cloneList.Count - i), _timeBetweenSpawn*0.8f);
            _cloneList[i].gameObject.SetActive(true);
            _cloneList[i].transform.transform.position = _cloneSpawn.position;
            _cloneList[i].transform.transform.rotation = _cloneSpawn.rotation;
            _cloneList[i].StartReplay();
            yield return new WaitForSeconds(_timeBetweenSpawn);
        }

        if (!gameover)
        {
            MessageUI.Showtext("GO!", _timeBetweenSpawn);
            SpawnPlayer();
        }
        else
        {
            _camera.gameObject.SetActive(false);
            _overview.gameObject.SetActive(true);
            MessageUI.Showtext("Game over, you won! You're also dead.", 10);
        }
    }
}
