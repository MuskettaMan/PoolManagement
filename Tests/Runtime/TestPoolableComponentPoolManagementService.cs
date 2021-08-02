using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestPoolableComponentPoolManagementService
{
	public class PoolableObject : MonoBehaviour, IPoolable
	{
		public void Requested() { }
		public void Returned() { }
	}

	[Test]
	public void OnRequest_OfPoolableObject_RequestedIsCalledOnObject()
	{
		// Arrange
		var dependencies = CreateDependencies();
		var pool = new ObjectPool<PoolableObject>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService);

		// Act
		var @object = pool.RequestObject();

		// Assert
		dependencies.poolableObjectPoolManagement.Received().ObjectRequested(@object);

		// Clean up
		pool.Dispose();
	}

	[Test]
	public void OnReturnOfPoolableObject_ReturnedIsCalledOnObject()
	{
		// Arrange
		var dependencies = CreateDependencies();
		var pool = new ObjectPool<PoolableObject>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService);

		// Act
		var @object = pool.RequestObject();
		pool.ReturnObject(@object);

		// Assert
		dependencies.poolableObjectPoolManagement.Received().ObjectReturned(@object);

		// Clean up
		pool.Dispose();
	}

	private Dependencies<PoolableObject> CreateDependencies()
	{
		// Creation service.
		ICreationService<PoolableObject> creationService = Substitute.For<ICreationService<PoolableObject>>();
		creationService.Create().Returns(new GameObject().AddComponent<PoolableObject>());

		// Pool management service.
		GameObjectPoolManagementService gameObjectPoolManagementService = Substitute.For<GameObjectPoolManagementService>(false, new GameObject().transform);
		PoolableObjectPoolManagementService<PoolableObject> poolableObjectPoolManagementService = Substitute.For<PoolableObjectPoolManagementService<PoolableObject>>();
		IPoolManagementService<PoolableObject> poolManagementService = new PoolableComponentPoolManagementService<PoolableObject>(gameObjectPoolManagementService, poolableObjectPoolManagementService);

		// Destruction service.
		IDestructionService<PoolableObject> destructionService = Substitute.For<IDestructionService<PoolableObject>>();


		Dependencies<PoolableObject> dependencies = new Dependencies<PoolableObject>(creationService, poolManagementService, poolableObjectPoolManagementService, destructionService);
		return dependencies;
	}

	private class Dependencies<T> where T : MonoBehaviour, IPoolable
	{
		public readonly ICreationService<T> creationService;
		public readonly IPoolManagementService<T> poolManagementService;
		public readonly PoolableObjectPoolManagementService<T> poolableObjectPoolManagement;
		public readonly IDestructionService<T> destructionService;

		public Dependencies(ICreationService<T> creationService, IPoolManagementService<T> poolManagementService, PoolableObjectPoolManagementService<T> poolableObjectPoolManagement, IDestructionService<T> destructionService)
		{
			this.creationService = creationService;
			this.poolManagementService = poolManagementService;
			this.poolableObjectPoolManagement = poolableObjectPoolManagement;
			this.destructionService = destructionService;
		}
	}
}
