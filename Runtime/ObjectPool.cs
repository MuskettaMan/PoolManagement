using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T> : IPooler<T>, IDisposable
{
	private readonly ICreationService<T> creationService;
	private readonly IPoolManagementService<T> poolManagementService;
	private readonly IDestructionService<T> destructionService;
	private Stack<T> pooled = new Stack<T>();
	private HashSet<T> inUse = new HashSet<T>();

	public ObjectPool(ICreationService<T> creationService, IPoolManagementService<T> poolManagementService, IDestructionService<T> destructionService, int initialSize)
	{
		if (creationService == null)
			throw new ArgumentNullException("creationService");
		if (poolManagementService == null)
			throw new ArgumentNullException("poolManagementService");
		if (destructionService == null)
			throw new ArgumentNullException("destructionService");
		initialSize = Mathf.Max(initialSize, 0);

		this.creationService = creationService;
		this.poolManagementService = poolManagementService;
		this.destructionService = destructionService;

		for (int i = 0; i < initialSize; i++)
		{
			T @object = creationService.Create();
			pooled.Push(@object);
			poolManagementService.ObjectCreated(@object);
		}
	}

	public T RequestObject()
	{
		T objectToBeSend;
		if(pooled.Count == 0)
			objectToBeSend = creationService.Create();
		else
			objectToBeSend = pooled.Pop();

		inUse.Add(objectToBeSend);
		poolManagementService.ObjectRequested(objectToBeSend);
		return objectToBeSend;
	}


	public void ReturnObject(T @object)
	{
		if (@object == null)
			throw new ArgumentNullException("@object");
		if (!inUse.Contains(@object))
			throw new ArgumentException("Returned object doesn't belong in this pool.");

		inUse.Remove(@object);
		pooled.Push(@object);
		poolManagementService.ObjectReturned(@object);
	}

	public void Dispose()
	{
		foreach (T @object in pooled)
		{
			poolManagementService.ObjectDestroyed(@object);
			destructionService.Destroy(@object);
		}
	}

	public bool IsObjectInPool(T @object)
	{
		if (@object == null)
			throw new ArgumentNullException("@object");

		return pooled.Contains(@object);
	}

	public bool IsObjectInUse(T @object)
	{
		if (@object == null)
			throw new ArgumentNullException("@object");

		return inUse.Contains(@object);
	}
}

public interface IDestructionService<T>
{
	void Destroy(T @object);
}
