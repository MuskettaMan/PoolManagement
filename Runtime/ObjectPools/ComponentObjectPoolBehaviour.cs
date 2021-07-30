using UnityEngine;

public abstract class ComponentObjectPoolBehaviour<T> : ObjectPoolBehaviour<T> where T : Component
{
	[SerializeField]
	protected bool useSendMessages;
	protected override IPoolManagementService<T> InitializePoolManagementService() => new ComponentPoolManagementService<T>(new GameObjectPoolManagementService(useSendMessages));

	protected override IDestructionService<T> InitializeDestructionService() => new ComponentDestructionService<T>(new UnityObjectDestructionService<GameObject>());
}
