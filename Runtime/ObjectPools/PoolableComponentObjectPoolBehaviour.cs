using UnityEngine;

public class PoolableComponentObjectPoolBehaviour<T> : ComponentObjectPoolBehaviour<T> where T : Component, IPoolable
{

	protected override IPoolManagementService<T> InitializePoolManagementService()
	{
		return new PoolableComponentPoolManagementService<T>(new GameObjectPoolManagementService(useSendMessages), new PoolableObjectPoolManagementService<T>());
	}
}