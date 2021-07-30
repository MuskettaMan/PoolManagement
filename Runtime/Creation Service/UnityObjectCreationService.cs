using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

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

	public T Create() => UnityObject.Instantiate(prefab, parent);
}
