using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomScreenUI : ScreenUICore
{
    public Transform playerUIListParent;  // 세션 목록 UI 부모 오브젝트
    public GameObject playerUIPrefab; // 세션 UI 프리팹
    private Queue<GameObject> playerUIpool = new Queue<GameObject>(); // 오브젝트 풀
    private List<GameObject> activeplayerUI = new List<GameObject>(); // 활성화된 세션 목록
	private static readonly Dictionary<RoomPlayer, RoomPlayerUI> ListItems = new Dictionary<RoomPlayer, RoomPlayerUI>();
    private void Awake()
	{
		RoomPlayer.playerJoined += AddPlayer;
		RoomPlayer.playerLeft += RemovePlayer;
	}

	private void OnDestroy()
	{
		RoomPlayer.playerJoined -= AddPlayer;
		RoomPlayer.playerLeft -= RemovePlayer;
	}

	private void AddPlayer(RoomPlayer player)
	{
		if (ListItems.ContainsKey(player))
		{
			var toRemove = ListItems[player];
			Destroy(toRemove.gameObject);

			ListItems.Remove(player);
		}

		var obj = Instantiate(playerUIPrefab, playerUIListParent).GetComponent<RoomPlayerUI>();
		obj.SetPlayer(player);

		ListItems.Add(player, obj);
	}

	private void RemovePlayer(RoomPlayer player)
	{
		if (!ListItems.ContainsKey(player))
			return;

		var obj = ListItems[player];
		if (obj != null)
		{
			Destroy(obj.gameObject);
			ListItems.Remove(player);
		}
	}
}
