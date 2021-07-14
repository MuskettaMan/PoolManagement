﻿using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T> : IPooler<T>, IDisposable
{
	[Serializable]
	public class ObjectPoolConfig
	{
		public int initialCapacity;
		public bool shouldDestroyPooledObjects = true;
		public bool shouldDestroyInUseObjects = false;

		public ObjectPoolConfig(int initialCapacity = 0, bool shouldDestroyPooledObjects = true, bool shouldDestroyInUseObjects = false)
		{
			// Clamps value to contain only positive or zero.
			this.initialCapacity = Mathf.Max(initialCapacity, 0);
			this.shouldDestroyPooledObjects = shouldDestroyPooledObjects;
			this.shouldDestroyInUseObjects = shouldDestroyInUseObjects;
		}
	}

	private readonly ICreationService<T> creationService;
	private readonly IPoolManagementService<T> poolManagementService;
	private readonly IDestructionService<T> destructionService;
	private readonly ObjectPoolConfig config;
	private Stack<T> pooled;
	private HashSet<T> inUse;

	public ObjectPool(ICreationService<T> creationService, 
					  IPoolManagementService<T> poolManagementService, 
					  IDestructionService<T> destructionService,
					  ObjectPoolConfig config = null)
	{
		if (creationService == null)
			throw new ArgumentNullException("creationService");
		if (poolManagementService == null)
			throw new ArgumentNullException("poolManagementService");
		if (destructionService == null)
			throw new ArgumentNullException("destructionService");
		if (config == null)
			config = new ObjectPoolConfig();

		// Clamp max value to positive.
		this.config = config;
		this.pooled = new Stack<T>(this.config.initialCapacity);
		this.inUse = new HashSet<T>();

		this.creationService = creationService;
		this.poolManagementService = poolManagementService;
		this.destructionService = destructionService;

		for (int i = 0; i < this.config.initialCapacity; i++)
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
		if (config.shouldDestroyPooledObjects)
		{
			foreach (T @object in pooled)
			{
				poolManagementService.ObjectDestroyed(@object);
				destructionService.Destroy(@object);
			}
			pooled.Clear();
		}

		if(config.shouldDestroyInUseObjects)
		{
			foreach (T @object in inUse)
			{
				poolManagementService.ObjectDestroyed(@object);
				destructionService.Destroy(@object);
			}
			inUse.Clear();
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