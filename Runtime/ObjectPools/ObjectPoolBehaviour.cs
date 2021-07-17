using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class ObjectPoolBehaviour<T> : MonoBehaviour, IPooler<T> where T : Object
{
	[SerializeField]
	protected T prefab;

	[SerializeField]
	private ObjectPool<T>.ObjectPoolConfig config;

	[SerializeField]
	private Transform pooledObjectsParent;

	public ObjectPool<T> ObjectPool { get; private set; }

	protected ICreationService<T> CreationService { get; private set; }
	protected IPoolManagementService<T> PoolManagementService { get; private set; }
	protected IDestructionService<T> DestructionService { get; private set; }

	public int InitialCapacity => config.initialCapacity;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	protected virtual void Awake()
	{
		InitializeServices();
		ObjectPool = new ObjectPool<T>(CreationService, PoolManagementService, DestructionService, config);
	}

	protected virtual void OnDestroy()
	{
		ObjectPool.Dispose();
	}

	public T RequestObject() => ObjectPool.RequestObject();
	public void ReturnObject(T @object) => ObjectPool.ReturnObject(@object);

	protected virtual ICreationService<T> InitializeCreationService()
	{
		if (pooledObjectsParent == null)
			pooledObjectsParent = transform;
		return new UnityObjectCreationService<T>(prefab, pooledObjectsParent);
	}
	protected virtual IPoolManagementService<T> InitializePoolManagementService()
	{
		return new EmptyPoolManagementService<T>();
	}
	protected virtual IDestructionService<T> InitializeDestructionService()
	{
		return new UnityObjectDestructionService<T>();
	}

	private void InitializeServices()
	{
		CreationService = InitializeCreationService();
		PoolManagementService = InitializePoolManagementService();
		DestructionService = InitializeDestructionService();
	}
}