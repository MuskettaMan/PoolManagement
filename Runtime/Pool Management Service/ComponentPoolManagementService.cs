using UnityEngine;

public class ComponentPoolManagementService<T> : IPoolManagementService<T> where T : Component
{
	private readonly GameObjectPoolManagementService gameObjectPoolManagementService;

	public ComponentPoolManagementService(GameObjectPoolManagementService gameObjectPoolManagementService) => this.gameObjectPoolManagementService = gameObjectPoolManagementService;

	public virtual void ObjectCreated(T @object) => gameObjectPoolManagementService.ObjectCreated(@object.gameObject);

	public virtual void ObjectRequested(T @object) => gameObjectPoolManagementService.ObjectRequested(@object.gameObject);

	public virtual void ObjectReturned(T @object) => gameObjectPoolManagementService.ObjectReturned(@object.gameObject);

	public virtual void ObjectDestroyed(T @object) => gameObjectPoolManagementService.ObjectDestroyed(@object.gameObject);
}