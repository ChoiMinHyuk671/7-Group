using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;

public class WinStateBehaviour : StateBehaviour
{
    float stateLimit = 3f;
	protected override void OnFixedUpdate()
	{
		if (Machine.StateTime < stateLimit)
			return;

        if(GameTurnManager.Instance.CurrentRound < 4)
		    Machine.ForceActivateState(Machine.GetState<PlayStateBehaviour>());
        else    
        {
            //게임 종료 로비로 추방
        }
	}

	protected override void OnEnterStateRender()
	{
		
	}

	protected override void OnExitStateRender()
	{
		
	}
}
