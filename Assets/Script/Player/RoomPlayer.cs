using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RoomPlayer : NetworkBehaviour
{
    public enum EGameState
	{
		Lobby,
		GameCutscene,
		GameReady
	}
	public static readonly List<RoomPlayer> Players = new List<RoomPlayer>();
	public static Action<RoomPlayer> playerJoined;
	public static Action<RoomPlayer> playerLeft;
	public static RoomPlayer Local;
	[Networked] public NetworkBool IsReady { get; set; }
	[Networked] public NetworkString<_32> playerName { get; set; }
	[Networked] public EGameState GameState { get; set; }
	public bool IsLeader => Object!=null && Object.IsValid && Object.HasStateAuthority;
	private ChangeDetector _changeDetector;
	[Networked] public PlayerController m_PlayerController{ get; set; }
	public override void Spawned()
	{
		base.Spawned();
		
		_changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

		if (Object.HasInputAuthority)
		{
			Local = this;
			RPC_SetPlayerStats(ServerInfo.playerName);
		}

		Players.Add(this);
		playerJoined?.Invoke(this);

		DontDestroyOnLoad(gameObject);
	}
	public override void Render()
	{
		foreach (var change in _changeDetector.DetectChanges(this))
		{
			switch (change)
			{
				case nameof(IsReady):
				case nameof(playerName):
					break;
			}
		}
	}

	[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
	private void RPC_SetPlayerStats(NetworkString<_32> _playerName)
	{
		playerName = _playerName;
	}

	[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
	public void RPC_ChangeReadyState(NetworkBool state)
	{
		IsReady = state;
	}
	private void OnDisable()
	{
		playerLeft?.Invoke(this);
		Players.Remove(this);
	}
	public static void RemovePlayer(NetworkRunner runner, PlayerRef player)
	{
		var roomPlayer = Players.FirstOrDefault(x => x.Object.InputAuthority == player);
		if (roomPlayer != null)
		{
			Players.Remove(roomPlayer);
			runner.Despawn(roomPlayer.Object);
		}
	}
}
