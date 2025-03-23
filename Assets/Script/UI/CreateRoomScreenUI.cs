using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateRoomScreenUI : ScreenUICore
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_InputField inputField; 

    private void Awake()
    {
        dropdown.onValueChanged.AddListener((int index) => {ServerInfo.maxPlayers = index + 1; Debug.Log($"System : MaxPlayer Change => : {ServerInfo.maxPlayers}");});
        inputField.onValueChanged.AddListener(x => {ServerInfo.roomName = x; Debug.Log($"System : RoomName Change => {ServerInfo.roomName}");});
    }
}