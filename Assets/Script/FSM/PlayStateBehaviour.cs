using Fusion;
using Fusion.Addons.FSM;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State entered when a gameplay session starts.
/// </summary>
public class PlayStateBehaviour : StateBehaviour
{
    [Networked] private TickTimer TurnTimer { get; set; }
    private GameTurnManager turnManager;
    [SerializeField] private int turnIndex;
	protected override void OnEnterState() //턴 시작시 즉시 턴을 설정
	{
		int currentTurn = GameTurnManager.Instance.CurrentTurnPlayer;
		int nextTurn = GameTurnManager.Instance.isTurnDirection == TurnDirection.Forward ? (currentTurn + 1) % ServerInfo.maxPlayers : (currentTurn - 1 + ServerInfo.maxPlayers) % ServerInfo.maxPlayers;
		GameTurnManager.Instance.CurrentTurnPlayer = nextTurn; //턴 설정

		if (Machine.PreviousState is PregameStateBehaviour) // 만약 처음 시작이라면 시작 턴을 0으로 초기화
		{
			GameTurnManager.Instance.CurrentTurnPlayer = 0; //시작 턴 설정
			GameTurnManager.Instance.isTurnDirection = TurnDirection.Forward; //방향 설정
		}

		foreach(RoomPlayer player in RoomPlayer.Players) //모두의 상태를 변경(선택 불가 상태)
			    player.m_PlayerController.playerState = EPlayerState.NotSelectable; //모두 선택 불가 상태로 선언

		Debug.Log(RoomPlayer.Players[GameTurnManager.Instance.CurrentTurnPlayer]);
            
        RoomPlayer.Players[GameTurnManager.Instance.CurrentTurnPlayer].m_PlayerController.playerState = EPlayerState.Selectable; //그리고 선택 가능 플레이어

		// /Debug.Log($"현재 턴 : {GameTurnManager.Instance.CurrentTurnPlayer}\n현재 턴 방향 : {RoomPlayer.Players[GameTurnManager.Instance.CurrentTurnPlayer].m_PlayerController.playerState.ToString()}");
		// 너무 졸려
	}
	protected override void OnEnterStateRender() 
	{
		Debug.Log($"현재 턴 : {GameTurnManager.Instance.CurrentTurnPlayer}\n현재 턴 방향 : {RoomPlayer.Players[GameTurnManager.Instance.CurrentTurnPlayer].m_PlayerController.playerState.ToString()}");
	 } //나중에 애니메이션이나 UI 작업에 쓸걸?
}
