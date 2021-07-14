using UnityEngine;

public class PoolableComponentPoolManagementService<T> : ComponentPoolManagementService<T> where T : Component, IPoolable
{

	public PoolableComponentPoolManagementService(GameObjectPoolManagementService gameObjectPoolManagementService) : base(gameObjectPoolManagementService)
	{ }

	public override void ObjectCreated(T @object)
	{
		base.ObjectCreated(@object);
	}

	public override void ObjectDestroyed(T @object)
	{
		base.ObjectDestroyed(@object);
	}

	public override void ObjectRequested(T @object)
	{
		base.ObjectRequested(@object);
		@object.Requested();
	}

	public override void ObjectReturned(T @object)
	{
		@object.Returned();
		base.ObjectReturned(@object);
	}
}