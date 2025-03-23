using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public enum EPlayerState
	{
		NotSelectable,
		Selectable,
		SelectTarget,
		ConfirmAttack
	}
public class PlayerController : PlayerComponent, IStats, IDamageable
{
    [Networked] public RoomPlayer roomPlayer {get; set;}
    [Networked] public float hp {get; set;}
    [Networked] public float maxHp {get; set;}
    [Networked] public NetworkBool IsDead {get; set;}
	[Networked] public EPlayerState playerState {get; set;} //오브젝트 선택이 가능한 상태인지를 정의
    public event Action OnDamage;
    private ChangeDetector _changeDetector {get; set;}
	public LayerMask selectable; // 감지할 레이어 설정 (인스펙터에서 설정 가능)
    public LayerMask selectTarget;
    public override void Spawned()
	{
		base.Spawned();
		_changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
	}
    public override void Render()
	{
		foreach (var change in _changeDetector.DetectChanges(this))
		{
			switch (change)
			{
				case nameof(hp):
                case nameof(maxHp):
					break;
			}
		}
	}

	public override void FixedUpdateNetwork()
	{

	}

	public void Update()
	{
        
        if (Input.GetMouseButtonDown(1))
        {
			AuthorityHandler authHandler = Gun.Instance.GetComponent<AuthorityHandler>();

			authHandler.RequestAuthority
            (
				onAuthorized: () =>
				{
					Gun.Instance.transform.SetParent(Object.transform);
					Gun.Instance.transform.SetPositionAndRotation(transform.position, transform.rotation);
				}
			);
        }
		if (!Object.HasInputAuthority) return;
		if(playerState == EPlayerState.NotSelectable || playerState == EPlayerState.ConfirmAttack) return;
		
		if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            switch (playerState)
            {
                case EPlayerState.Selectable:   
                    Selectable(ray);
                    break;
                case EPlayerState.SelectTarget: 
                    SelectTarget(ray);
                    break;
                default:
                    Debug.Log("선택된 오브젝트가 없습니다");
                    break;
            }
		}
	}
	private void Selectable(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, selectable))
        {
			//애니메이션 작동
        }

        playerState = EPlayerState.SelectTarget;
    }

    private void SelectTarget(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, selectTarget))
        {
            //애니메이션 작동
			hit.collider.GetComponent<IDamageable>().RPC_TakeDamage(10f);
        }

        playerState = EPlayerState.ConfirmAttack;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)] //호출 권한은 All, 타켓은 서버로  Hp의 변경을 위한 함수
    public void RPC_TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log($"System: -{damage}, 현재 HP: {hp}");
    }
}
