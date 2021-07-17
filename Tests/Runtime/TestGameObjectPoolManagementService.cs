using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class TestGameObjectPoolManagementService
{


	public TestableGameObjectPoolBehaviour gameObjectPoolBehaviour;

	[SetUp]
	public void SetUp()
	{
		gameObjectPoolBehaviour = new GameObject().AddComponent<TestableGameObjectPoolBehaviour>();
	}
	
	[TearDown]
	public void TearDown()
	{
		Object.Destroy(gameObjectPoolBehaviour);
	}

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
