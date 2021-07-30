using UnityEngine;

public class PoolableComponentPoolManagementService<T> : ComponentPoolManagementService<T> where T : Component, IPoolable
{
	private readonly PoolableObjectPoolManagementService<T> poolableObjectPoolManagementService;

	public PoolableComponentPoolManagementService(
		GameObjectPoolManagementService gameObjectPoolManagementService,
		PoolableObjectPoolManagementService<T> poolableObjectPoolManagementService
	) : base(gameObjectPoolManagementService) => this.poolableObjectPoolManagementService = poolableObjectPoolManagementService;

	public override void ObjectCreated(T @object) => base.ObjectCreated(@object);

	public override void ObjectDestroyed(T @object) => base.ObjectDestroyed(@object);

	public override void ObjectRequested(T @object)
	{
		base.ObjectRequested(@object);
		poolableObjectPoolManagementService.ObjectRequested(@object);
	}

	public override void ObjectReturned(T @object)
	{
		@object.Returned();
		poolableObjectPoolManagementService.ObjectReturned(@object);
	}
}