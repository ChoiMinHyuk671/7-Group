using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomPlayerUI : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    private RoomPlayer _player;

    public void SetPlayer(RoomPlayer player) {
        _player = player;
    }

    private void Update() {
        if (_player.Object != null && _player.Object.IsValid)
        {
            playerName.text = _player.playerName.Value;
        }
    }
}
