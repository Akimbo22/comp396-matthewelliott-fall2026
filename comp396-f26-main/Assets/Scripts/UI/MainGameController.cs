using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class MainGameController : MonoBehaviour
{
    [SerializeField] private BoolEventChannel _boolEvent;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private TMPro.TMP_Text _uiText;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerFollowCam;
    [SerializeField] private GameObject _mainCam;

    private float timer = 0f;
    private bool _chasePlayer = false;

    private void OnEnable()
    {
        GameObject playerBody = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
        GameObject playerFollowCam = Instantiate(_playerFollowCam, Vector3.zero, Quaternion.identity);
        playerFollowCam.GetComponent<CinemachineCamera>().Target = new CameraTarget
        {
            TrackingTarget = playerBody.transform.GetChild(0).transform
        };
        Instantiate(_mainCam, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 15f)
        {
            _chasePlayer = !_chasePlayer;
            _boolEvent.RaiseEvent(_chasePlayer);
            UpdateUiText(_chasePlayer);
            timer = 0f;
        }
    }

    private void UpdateUiText(bool status)
    {
        _uiText.text = _playerData.Status.ToString();
    }
}
