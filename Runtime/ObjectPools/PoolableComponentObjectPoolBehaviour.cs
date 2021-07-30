using UnityEngine;

public class PoolableComponentObjectPoolBehaviour<T> : ComponentObjectPoolBehaviour<T> where T : Component, IPoolable
{

	protected override IPoolManagementService<T> InitializePoolManagementService() => new PoolableComponentPoolManagementService<T>(new GameObjectPoolManagementService(useSendMessages), new PoolableObjectPoolManagementService<T>());
}