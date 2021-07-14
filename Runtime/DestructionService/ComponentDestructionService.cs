using UnityEngine;

public class ComponentDestructionService<T> : IDestructionService<T> where T : Component
{
	private readonly UnityObjectDestructionService<GameObject> unityObjectDestructionService;

	public ComponentDestructionService(UnityObjectDestructionService<GameObject> unityObjectDestructionService)
	{
		this.unityObjectDestructionService = unityObjectDestructionService;
	}

	public void Destroy(T @object)
	{
		unityObjectDestructionService.Destroy(@object.gameObject);
	}
}