using UnityEngine;

public sealed class GameObjectPoolBehaviour : ObjectPoolBehaviour<GameObject>
{
	[SerializeField]
	private bool useSendMessages;

	protected override IPoolManagementService<GameObject> InitializePoolManagementService() => new GameObjectPoolManagementService(useSendMessages);
}