using Fusion;
using Fusion.Addons.FSM;
using System.Collections.Generic;
using UnityEngine;
public class GameState : NetworkBehaviour, IStateMachineOwner
{
	public StateBehaviour ActiveState => stateMachine.ActiveState;
	[Networked] TickTimer Delay { get; set; }
	[Networked] int DelayedStateId { get; set; }

	[Header("Game States Reference")]
    public PregameStateBehaviour pregameState;
	public PlayStateBehaviour playState;
	public ResultsStateBehaviour resultsState;
	public WinStateBehaviour winState;
	private StateMachine<StateBehaviour> stateMachine;

	public override void FixedUpdateNetwork()
	{
		if (DelayedStateId >= 0 && Delay.ExpiredOrNotRunning(Runner))
		{
			stateMachine.ForceActivateState(DelayedStateId);
			DelayedStateId = -1;
		}
	}

	public void SetState<T>() where T : StateBehaviour
	{
		Assert.Check(HasStateAuthority, "Clients cannot set GameState");

		Delay = TickTimer.None;
		DelayedStateId = stateMachine.GetState<T>().StateId;
	}

	public void DelaySetState<T>(float delay) where T : StateBehaviour
	{
		Delay = TickTimer.CreateFromSeconds(Runner, delay);
		DelayedStateId = stateMachine.GetState<T>().StateId;
	}

	public void CollectStateMachines(List<IStateMachine> stateMachines)
	{
		stateMachine = new StateMachine<StateBehaviour>("Game State" , playState, resultsState, winState);
		stateMachines.Add(stateMachine);
	}
}