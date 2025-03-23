using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerInfo
{
    public const int MaximumRoomSize = 4; //the actual hard limit

    public static string playerName { get => PlayerPrefs.GetString("S_PlayerName", "NoneName"); set => PlayerPrefs.SetString("S_PlayerName", value); }
    public static string roomName { get => PlayerPrefs.GetString("S_RoomName", "NoneName"); set => PlayerPrefs.SetString("S_RoomName", value); }
    public static int GameMode { get => PlayerPrefs.GetInt("S_GameMode", 0); set => PlayerPrefs.SetInt("S_GameMode", value); }
    public static int maxPlayers { get => PlayerPrefs.GetInt("S_MaxPlayer", 4);set => PlayerPrefs.SetInt("S_MaxPlayer", Mathf.Clamp(value, 1, MaximumRoomSize)); }
}
