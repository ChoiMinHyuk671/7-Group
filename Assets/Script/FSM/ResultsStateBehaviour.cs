using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion.Addons.FSM;

public class ResultsStateBehaviour : StateBehaviour
{
    private StateBehaviour nextState;
	private float nextStateDelay;

	protected override void OnEnterState()
	{
        List<RoomPlayer> survivor = new List<RoomPlayer>();
        foreach (var player in RoomPlayer.Players)
        {
            if(player.gameObject.GetComponent<IStats>().IsDead == false && player.gameObject.GetComponent<IStats>().hp <= 0)
            {
                player.gameObject.GetComponent<IStats>().IsDead  = true;
            }
            else if(player.gameObject.GetComponent<IStats>().IsDead == true)
            {
                survivor.Add(player);
            }
        }

        int survivorNum = survivor.Count;
        StateBehaviour nextState = survivorNum <= 1 ? Machine.GetState<WinStateBehaviour>() : Machine.GetState<PlayStateBehaviour>();
        Machine.ForceActivateState(nextState);
	}
	protected override void OnEnterStateRender()
	{
		
	}
}
