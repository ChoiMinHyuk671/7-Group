using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Addons.FSM;
public enum TurnDirection
{
    Forward,
    Reverse
}

[RequireComponent(typeof(StateMachineController))]
public class GameTurnManager : NetworkBehaviour
{
    public static GameState State { get; private set; }
    [Networked] public int CurrentTurnPlayer { get; set; }
    [Networked] public TurnDirection isTurnDirection { get; set; }
    [Networked] public int CurrentRound { get; set; }
    public static GameTurnManager Instance;
    public static Dictionary<RoomPlayer, int> playerList = new Dictionary<RoomPlayer, int>(); //필요에 의해 턴 순서에 따라 플레이어 호출을 위해
    private void Awake() 
    {
        // 인스턴스가 이미 존재하면 현재 오브젝트를 파괴
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        else
            Instance = this;
    }
    public override void Spawned()
    {
        State = GetComponent<GameState>();
        if (Runner.IsServer)
        {
            for(int i =0; i < RoomPlayer.Players.Count; i++)
            {
                playerList.Add(RoomPlayer.Players[i], i);
            }
        }
    }
    public void StartGame() //게임 시작시 실행
	{
		//if (State.ActiveState is not PregameStateBehaviour) return; //초기 세팅 상태를 만들예정

		State.SetState<PlayStateBehaviour>();
	}
}

