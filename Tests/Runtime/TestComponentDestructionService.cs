using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Musketta.PoolManagement.Tests
{
	public class TestComponentDestructionService
	{
		[UnityTest]
		public IEnumerator Destroy_Component_GameObjectIsDestroyed()
		{
			IDestructionService<Transform> destructionService = new ComponentDestructionService<Transform>(new UnityObjectDestructionService<GameObject>());
			Transform @object = new GameObject().transform;

			destructionService.Destroy(@object);

			yield return null;

			Assert.IsTrue(@object == null);
		}

		[Test]
		public void Destroy_Null_ThrowsException()
		{
			IDestructionService<Transform> destructionService = new ComponentDestructionService<Transform>(new UnityObjectDestructionService<GameObject>());

			TestDelegate testCode = () => destructionService.Destroy(null);

			Assert.Throws<ArgumentNullException>(testCode);
		}

		[Test]
		public void Construct_WithNullUnityObjectDestructionService_ThrowsException()
		{
			TestDelegate testCode = () => new ComponentDestructionService<Transform>(null);

			Assert.Throws<ArgumentNullException>(testCode);
		}
	}
}
