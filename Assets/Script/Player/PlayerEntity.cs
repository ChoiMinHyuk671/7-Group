using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerEntity : PlayerComponent
{
    public static event Action<PlayerEntity> OnPlayerSpawned;
	public static event Action<PlayerEntity> OnPlayerDespawned;    
	//public PlayerAnimator Animator { get; private set; }
	public PlayerCamera Camera { get; private set; }
	[Networked] public PlayerController m_PlayerController { get; private set; }
	//public PlayerAudio Audio { get; private set; }
    private bool _despawned;
    private ChangeDetector _changeDetector;
	private void Awake()
	{
		// // Set references before initializing all components
		// //Animator = GetComponentInChildren<PlayerAnimator>();
		// Camera = GetComponentInChildren<PlayerCamera>();
		// m_PlayerController = GetComponentInChildren<PlayerController>();
		// //Audio = GetComponentInChildren<PlayerAudio>();

		// // Initializes all KartComponents on or under the Kart prefab
		// var components = GetComponentsInChildren<PlayerComponent>();
		// foreach (var component in components) component.Initialize(this);
	}

	public static readonly List<PlayerEntity> players = new List<PlayerEntity>();

	public override void Spawned()
	{
		base.Spawned();
		Camera = GetComponentInChildren<PlayerCamera>();
		m_PlayerController = GetComponent<PlayerController>();
		//Audio = GetComponentInChildren<PlayerAudio>();

		// Initializes all KartComponents on or under the Kart prefab
		var components = GetComponentsInChildren<PlayerComponent>();
		foreach (var component in components) component.Initialize(this);
		
		_changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

		players.Add(this);
		OnPlayerSpawned?.Invoke(this);
	}
	
	public override void Render() {	}

	public override void Despawned(NetworkRunner runner, bool hasState)
	{
		base.Despawned(runner, hasState);
		players.Remove(this);
		_despawned = true;
		OnPlayerDespawned?.Invoke(this);
	}

	private void OnDestroy()
	{
		players.Remove(this);
		if (!_despawned)
		{
			OnPlayerDespawned?.Invoke(this);
		}
	}
}
