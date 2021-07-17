using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoolableObjectPoolManagementService : MonoBehaviour
{
	[Test]
	public void OnRequest_OfPoolableObject_RequestedIsCalledOnObject()
	{
		// Arrange
		var dependencies = CreateDependencies();
		var pool = new ObjectPool<IPoolable>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService);

		// Act
		var @object = pool.RequestObject();

		// Arrange
		@object.Received().Requested();
	}

	[Test]
	public void OnReturn_OfPoolableObject_ReturnedIsCalledOnObject()
	{
		// Arrange
		var dependencies = CreateDependencies();
		var pool = new ObjectPool<IPoolable>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService);

		// Act
		var @object = pool.RequestObject();
		pool.ReturnObject(@object);

		// Arrange
		@object.Received().Returned();
	}

	private Dependencies<IPoolable> CreateDependencies()
	{
		ICreationService<IPoolable> creationService = Substitute.For<ICreationService<IPoolable>>();
		creationService.Create().Returns(Substitute.For<IPoolable>());
		IPoolManagementService<IPoolable> poolManagementService = new PoolableObjectPoolManagementService<IPoolable>();
		IDestructionService<IPoolable> destructionService = Substitute.For<IDestructionService<IPoolable>>();
		Dependencies<IPoolable> dependencies = new Dependencies<IPoolable>(creationService, poolManagementService, destructionService);
		return dependencies;
	}

	private class Dependencies<T>
	{
		public readonly ICreationService<T> creationService;
		public readonly IPoolManagementService<T> poolManagementService;
		public readonly IDestructionService<T> destructionService;

		public Dependencies(ICreationService<T> creationService, IPoolManagementService<T> poolManagementService, IDestructionService<T> destructionService)
		{
			this.creationService = creationService;
			this.poolManagementService = poolManagementService;
			this.destructionService = destructionService;
		}
	}
}
