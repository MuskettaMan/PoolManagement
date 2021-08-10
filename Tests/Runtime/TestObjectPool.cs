using NSubstitute;
using NUnit.Framework;
using System;

namespace Musketta.PoolManagement.Tests
{
	public class TestObjectPool
	{
		public class PoolableObject { }

		[Test]
		public void Constructing_WithMoreThanZeroInitialSize_NotifiesCreationService()
		{
			// Arrange
			// Create pool
			var dependencies = CreateDependencies();
			var config = new ObjectPool<PoolableObject>.ObjectPoolConfig(initialCapacity: 1);

			// Act
			var objectPool = new ObjectPool<PoolableObject>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService, config);

			// Assert
			PoolableObject p = objectPool.RequestObject();
			dependencies.poolManagementService.Received().ObjectCreated(p);
		}

		[Test]
		public void Constructing_WithNullCreationService_ThrowsArgumentNullException()
		{
			// Arrange
			var dependencies = CreateDependencies();

			// Act
			TestDelegate code = () => new ObjectPool<PoolableObject>(null, dependencies.poolManagementService, dependencies.destructionService);

			// Assert
			Assert.Throws<ArgumentNullException>(code);
		}

		[Test]
		public void Constructing_WithNullPoolManagementService_ThrowsArgumentNullException()
		{
			// Arrange
			var dependencies = CreateDependencies();

			// Act
			TestDelegate code = () => new ObjectPool<PoolableObject>(dependencies.creationService, null, dependencies.destructionService);

			// Assert
			Assert.Throws<ArgumentNullException>(code);
		}

		[Test]
		public void Constructing_WithNullDestructionService_ThrowsArgumentNullException()
		{
			// Arrange
			var dependencies = CreateDependencies();

			// Act
			TestDelegate code = () => new ObjectPool<PoolableObject>(dependencies.creationService, dependencies.poolManagementService, null);

			// Assert
			Assert.Throws<ArgumentNullException>(code);
		}

		[Test]
		public void RequestObject_AtAll_ReturnsNotNullObject()
		{
			// Arrange
			var dependencies = CreateDependencies();
			var config = new ObjectPool<PoolableObject>.ObjectPoolConfig(initialCapacity: 1, true, true);
			ObjectPool<PoolableObject> objectPool = new ObjectPool<PoolableObject>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService, config);

			// Act
			PoolableObject requestedObject = objectPool.RequestObject();

			// Assert
			Assert.IsNotNull(requestedObject);

			// Clean up
			objectPool.Dispose();
		}

		[Test]
		public void RequestObject_FromPool_DoesntCreateObject()
		{
			// Arrange
			var dependencies = CreateDependencies();
			var config = new ObjectPool<PoolableObject>.ObjectPoolConfig(initialCapacity: 1, true, true);
			ObjectPool<PoolableObject> objectPool = new ObjectPool<PoolableObject>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService, config);

			dependencies.creationService.ClearReceivedCalls();

			// Act
			objectPool.RequestObject();

			// Assert
			dependencies.creationService.DidNotReceive().Create();

			// Clean up
			objectPool.Dispose();
		}

		[Test]
		public void RequestObject_FromEmptyPool_CreatesObject()
		{
			// Arrange
			var dependencies = CreateDependencies();
			var config = new ObjectPool<PoolableObject>.ObjectPoolConfig(initialCapacity: 0, true, true);
			ObjectPool<PoolableObject> objectPool = new ObjectPool<PoolableObject>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService, config);

			// Act
			objectPool.RequestObject();

			// Assert
			dependencies.creationService.Received().Create();

			// Clean up
			objectPool.Dispose();
		}

		[Test]
		public void ReturnObject_AfterRequesting_NotifiesPoolManagement()
		{
			// Arrange
			var dependencies = CreateDependencies();
			var config = new ObjectPool<PoolableObject>.ObjectPoolConfig(initialCapacity: 5, true, true);
			ObjectPool<PoolableObject> objectPool = new ObjectPool<PoolableObject>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService, config);

			// Act
			var instance = objectPool.RequestObject();
			objectPool.ReturnObject(instance);

			// Assert
			dependencies.poolManagementService.Received().ObjectReturned(instance);

			// Clean up
			objectPool.Dispose();
		}

		[Test]
		public void Dispose_WithPooledAndInUseObject_NotifiesDestructionService([Values(true, false)] bool inUse, [Values(true, false)] bool pooled)
		{
			// Arrange
			// Create pool
			var dependencies = CreateDependencies();
			var config = new ObjectPool<PoolableObject>.ObjectPoolConfig(initialCapacity: 5, inUse, pooled);
			ObjectPool<PoolableObject> objectPool = new ObjectPool<PoolableObject>(dependencies.creationService, dependencies.poolManagementService, dependencies.destructionService, config);

			// Create pool with half the elements active and the other dormant.
			PoolableObject[] all = new PoolableObject[2];
			for (int i = 0; i < all.Length; i++)
			{
				all[i] = objectPool.RequestObject();
			}
			objectPool.ReturnObject(all[1]);

			// Act
			objectPool.Dispose();

			// Assert
			if (inUse)
				dependencies.poolManagementService.Received().ObjectDestroyed(all[0]);
			if (pooled)
				dependencies.poolManagementService.Received().ObjectDestroyed(all[1]);
		}

		private Dependencies<PoolableObject> CreateDependencies()
		{
			ICreationService<PoolableObject> creationService = Substitute.For<ICreationService<PoolableObject>>();
			creationService.Create().Returns(new PoolableObject());
			IPoolManagementService<PoolableObject> poolManagementService = Substitute.For<IPoolManagementService<PoolableObject>>();
			IDestructionService<PoolableObject> destructionService = Substitute.For<IDestructionService<PoolableObject>>();
			Dependencies<PoolableObject> dependencies = new Dependencies<PoolableObject>(creationService, poolManagementService, destructionService);
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
}
