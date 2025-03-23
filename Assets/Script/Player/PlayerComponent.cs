using Fusion;

public class PlayerComponent : NetworkBehaviour
{
    public PlayerEntity m_PlayerEntity { get; set; }
    public virtual void Initialize(PlayerEntity _playerEntity) 
    {
        m_PlayerEntity = _playerEntity;
    }
}
