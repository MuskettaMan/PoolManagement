using UnityEngine;
using UnityObject = UnityEngine.Object;
using System;

public class UnityObjectCreationService<T> : ICreationService<T> where T : UnityObject
{
	private readonly T prefab;
	private readonly Transform parent;

	public UnityObjectCreationService(T prefab, Transform parent = null)
	{
		if (prefab == null)
			throw new ArgumentNullException(nameof(prefab));

		this.prefab = prefab;
		this.parent = parent;
	}

	public T Create()
	{
		return UnityObject.Instantiate(prefab, parent);
	}
}
