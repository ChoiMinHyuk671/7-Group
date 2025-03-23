using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private RoomPlayer roomPlayerPrefab;
    [SerializeField] private GameMode _gameMode; //플레이 모드 설정 (Host, Join 선택창에서 설정)
    [SerializeField] private SceneController sceneController;
    public static NetworkRunner runner { get; private set; } //현재 생성된 러너를 쉽게 접근 하도록 설정

    void Start()
    {
        ServerInfo.playerName = "154";//$"Host{UnityEngine.Random.Range(0, 100)}";
        Application.runInBackground = true; //백그라운드 상태일때도 작동
        Application.targetFrameRate = Screen.currentResolution.refreshRate; //모니터 주사율에 맞게 프레임 설정
        QualitySettings.vSyncCount = 1; //1 프레임 동안 1번 그래픽 생성

        DontDestroyOnLoad(gameObject);

        JoinLobby(); // 시작 후 로비로 접속
    }

    public void SetCreateLobby() => _gameMode = GameMode.Host;
    public void SetJoinLobby() => _gameMode = GameMode.Client;

    public void JoinLobby()
    {
        GameObject go = new GameObject("Session");
        DontDestroyOnLoad(go);

        runner = go.AddComponent<NetworkRunner>();

        runner.ProvideInput = _gameMode != GameMode.Server;
        runner.AddCallbacks(this);

        runner.JoinSessionLobby(SessionLobby.ClientServer);
    }
    public void JoinOrCreateLobby()
    {
        runner.StartGame(new StartGameArgs
        {
            GameMode = _gameMode,
            SessionName = ServerInfo.roomName,
            SceneManager = sceneController,
            PlayerCount = ServerInfo.maxPlayers,
            EnableClientSessionCreation = false
        });
    }
    public void Test()
    {
        SceneController.SceneTransition(SceneType.InGame);
    }
    public void OnPlayerJoined(NetworkRunner _runner, PlayerRef player)
    {
        if (_runner.IsServer)
        {
            if (_gameMode==GameMode.Host)
            {
                var roomPlayer = _runner.Spawn(roomPlayerPrefab, Vector3.zero, Quaternion.identity, player);
                Debug.Log(roomPlayer.playerName);
            }
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
		RoomPlayer.RemovePlayer(runner, player);
	}
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}
