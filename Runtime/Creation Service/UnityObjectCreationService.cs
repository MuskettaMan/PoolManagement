using UnityEngine;
using UnityObject = UnityEngine.Object;

public class UnityObjectCreationService<T> : ICreationService<T> where T : UnityObject
{
	private readonly T prefab;
	private readonly Transform parent;

	public UnityObjectCreationService(T prefab, Transform parent = null)
	{
		this.prefab = prefab;
		this.parent = parent;
	}

	public T Create()
	{
		return UnityObject.Instantiate(prefab, parent);
	}
}
