using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using TMPro;

public class JoinRoomScreenUI : ScreenUICore, INetworkRunnerCallbacks
{
    public Transform sessionListParent;  // 세션 목록 UI 부모 오브젝트
    public GameObject sessionItemPrefab; // 세션 UI 프리팹
    [SerializeField] private TMP_InputField inputField; 
    private Queue<GameObject> sessionPool = new Queue<GameObject>(); // 오브젝트 풀
    private List<GameObject> activeSessions = new List<GameObject>(); // 활성화된 세션 목록

    private void Start()
    {
        inputField.onValueChanged.AddListener(x => {ServerInfo.roomName = x; Debug.Log($"System : RoomName Change => {ServerInfo.roomName}");});
        // 풀 크기 초기화 (최대 10개 정도 미리 생성해둠)
        for (int i = 0; i < 10; i++)
        {
            GameObject sessionItem = Instantiate(sessionItemPrefab, sessionListParent);
            sessionItem.SetActive(false);
            sessionPool.Enqueue(sessionItem);
        }
        GameNetworkManager.runner.AddCallbacks(this); // 콜백 등록
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

            foreach (var session in sessionList)
            {
                Debug.Log( $"- {session.Name} (플레이어 {session.PlayerCount}/{session.MaxPlayers})\n");
            }
        UpdateSessionUI(sessionList);
    }

    private void UpdateSessionUI(List<SessionInfo> sessionList)
    {
        // 기존 활성 세션들을 반환하기 전에 모두 풀로 되돌리기
        foreach (var session in activeSessions)
        {
            session.SetActive(false);
            sessionPool.Enqueue(session);
        }
        activeSessions.Clear();

        // 필요한 개수만큼 세션 가져와서 UI 업데이트
        for (int i = 0; i < sessionList.Count; i++)
        {
            GameObject sessionItem = GetSessionItem();
            sessionItem.GetComponent<SessionItemUI>().Init(
                sessionList[i].Name, 
                sessionList[i].PlayerCount, 
                sessionList[i].MaxPlayers, 
                sessionList[i].IsOpen
            );
            sessionItem.SetActive(true);
            activeSessions.Add(sessionItem);
        }
    }

    private GameObject GetSessionItem()
    {
        // 풀에서 사용 가능한 오브젝트 가져오기
        if (sessionPool.Count > 0)
        {
            return sessionPool.Dequeue();
        }

        // 풀이 부족하면 새로 생성
        GameObject newSessionItem = Instantiate(sessionItemPrefab, sessionListParent);
        return newSessionItem;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}
