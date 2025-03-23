using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SessionItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sessionName;
    [SerializeField] private TextMeshProUGUI playerCount;

    public void Init(string _sessionName, int _playerCount, int _maxPlayers, bool IsOpen)
    {
        sessionName.text = _sessionName;
        playerCount.text = $"{_playerCount} / {_maxPlayers}";
        GetComponent<Button>().onClick.AddListener(() => {ServerInfo.roomName = _sessionName; ServerInfo.maxPlayers = _maxPlayers;});
    }
}
