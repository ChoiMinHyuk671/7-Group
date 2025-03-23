using UnityEngine;
using Fusion;

public class StageManager : NetworkBehaviour
{
    public Transform[] spawnPoints; // 스폰 위치를 저장
    public PlayerEntity playerPrefab;
    public override void Spawned()
    {
        base.Spawned();

        if (Runner.GameMode == GameMode.Host)
        {
            foreach (var player in RoomPlayer.Players)
            {
                player.GameState = RoomPlayer.EGameState.GameCutscene;
                PlayerSpawn(Runner, player);
            }
            GameTurnManager.State.DelaySetState<PlayStateBehaviour>(5f);
        }
    }

    public void PlayerSpawn(NetworkRunner runner, RoomPlayer player)
    {
        var index = RoomPlayer.Players.IndexOf(player);
        var point = spawnPoints[index];

        var entity = runner.Spawn(
            playerPrefab,
            point.position,
            point.rotation,
            player.Object.InputAuthority
        );

        if (entity != null)
        {
            entity.m_PlayerController.roomPlayer = player;
            player.m_PlayerController = entity.m_PlayerController;
        }
    }
}
