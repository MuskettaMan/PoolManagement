using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Musketta.PoolManagement.Tests
{
	public class TestComponentPoolManagementService : MonoBehaviour
	{
		public TestableComponentObjectPoolBehaviour componentObjectPoolBehaviour;

		[SetUp]
		public void SetUp() => componentObjectPoolBehaviour = new GameObject().AddComponent<TestableComponentObjectPoolBehaviour>();

		[TearDown]
		public void TearDown() => Destroy(componentObjectPoolBehaviour.gameObject);

		[Test]
		public void ObjectCreated_WhenRequestingEmptyPool_GameObjectPoolManagementServiceCreatedIsCalled()
		{
			var @object = componentObjectPoolBehaviour.RequestObject();

			componentObjectPoolBehaviour.GameObjectPoolManagementService.Received().ObjectCreated(@object.gameObject);
		}

		[Test]
		public void ObjectDestroyed_WhenDestroyingPool_GameObjectPoolManagementServiceDestroyedIsCalled()
		{
			var @object = componentObjectPoolBehaviour.RequestObject();
			componentObjectPoolBehaviour.ReturnObject(@object);
			Destroy(componentObjectPoolBehaviour);

			componentObjectPoolBehaviour.GameObjectPoolManagementService.Received().ObjectDestroyed(@object.gameObject);
		}

		[Test]
		public void ObjectRequested_WhenRequestingEmptyPool_GameObjectPoolManagementServiceRequestedIsCalled()
		{
			var @object = componentObjectPoolBehaviour.RequestObject();

			componentObjectPoolBehaviour.GameObjectPoolManagementService.Received().ObjectRequested(@object.gameObject);
		}

		[Test]
		public void ObjectReturned_WhenReturningToPool_GameObjectPoolManagementServiceReturnedIsCalled()
		{
			var @object = componentObjectPoolBehaviour.RequestObject();
			componentObjectPoolBehaviour.ReturnObject(@object);

			componentObjectPoolBehaviour.GameObjectPoolManagementService.Received().ObjectReturned(@object.gameObject);
		}
	}
}
