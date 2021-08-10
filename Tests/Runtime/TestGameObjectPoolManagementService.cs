using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Musketta.PoolManagement.Tests
{
	public class TestGameObjectPoolManagementService
	{
		private GameObjectPoolBehaviour gameObjectPoolBehaviour;

		[SetUp]
		public void SetUp() => gameObjectPoolBehaviour = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObjectPoolBehaviour>("Packages/musketta.poolmanagement/Tests/Runtime/Prefabs/GameObjectPoolBehaviour.prefab"));

		[TearDown]
		public void TearDown() => Object.Destroy(gameObjectPoolBehaviour.gameObject);

		[Test]
		public void RequestGameObject_FromPool_IsActive()
		{
			GameObject gameObject = gameObjectPoolBehaviour.RequestObject();

			Assert.IsTrue(gameObject.activeSelf);
		}

		[Test]
		public void ReturnGameObject_ToPool_BecomesInactive()
		{
			GameObject gameObject = gameObjectPoolBehaviour.RequestObject();
			gameObjectPoolBehaviour.ReturnObject(gameObject);

			Assert.IsFalse(gameObject.activeSelf);
		}

		[Test]
		public void ReturnGameObject_ToPool_IsSetToOriginalParent()
		{
			GameObject gameObject = gameObjectPoolBehaviour.RequestObject();
			gameObjectPoolBehaviour.ReturnObject(gameObject);

			Assert.AreEqual(gameObjectPoolBehaviour.transform, gameObject.transform.parent);
		}

		[Test]
		public void RequestGameObject_FromPool_UnityMessageCalled()
		{
			GameObject gameObject = gameObjectPoolBehaviour.RequestObject();

			Assert.IsTrue(gameObject.GetComponent<TestableComponent>().RequestedCalled);
		}

		[Test]
		public void ReturnGameObject_ToPool_UnityMessageCalled()
		{
			GameObject gameObject = gameObjectPoolBehaviour.RequestObject();
			gameObjectPoolBehaviour.ReturnObject(gameObject);

			Assert.IsTrue(gameObject.GetComponent<TestableComponent>().ReturnedCalled);
		}
	}
}