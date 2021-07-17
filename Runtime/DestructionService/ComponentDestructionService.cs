using System;
using UnityEngine;

public class ComponentDestructionService<T> : IDestructionService<T> where T : Component
{
	private readonly UnityObjectDestructionService<GameObject> unityObjectDestructionService;

	public ComponentDestructionService(UnityObjectDestructionService<GameObject> unityObjectDestructionService)
	{
		if (unityObjectDestructionService == null)
			throw new ArgumentNullException(nameof(unityObjectDestructionService));

		this.unityObjectDestructionService = unityObjectDestructionService;
	}

	public void Destroy(T @object)
	{
		if (@object == null)
			throw new ArgumentNullException(nameof(@object));

		unityObjectDestructionService.Destroy(@object.gameObject);
	}
}