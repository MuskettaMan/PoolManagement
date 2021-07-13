using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSubstitute;
using NUnit.Framework;

public class TestObjectPool : MonoBehaviour
{
	[Test]
	public void RequestObject_FromPool_ReturnsObject()
	{
		// Arrange
		var dependencies = CreateDependencies();
		ObjectPool<Transform> objectPool = new ObjectPool<Transform>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService, 5);

		// Act
		Transform requestedObject = objectPool.RequestObject();

		// Assert
		dependencies.poolManagementService.Received().ObjectRequested(requestedObject);

		// Clean up
		objectPool.ReturnObject(requestedObject);
		objectPool.Dispose();
	}

	private Dependencies<Transform> CreateDependencies()
	{
		ICreationService<Transform> creationService = Substitute.For<ICreationService<Transform>>();
		creationService.Create().Returns(new GameObject().transform);
		IPoolManagementService<Transform> poolManagementService = Substitute.For<IPoolManagementService<Transform>>();
		IDestructionService<Transform> destructionService = Substitute.For<IDestructionService<Transform>>();
		Dependencies<Transform> dependencies = new Dependencies<Transform>(creationService, poolManagementService, destructionService);
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
