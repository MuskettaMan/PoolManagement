using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSubstitute;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Musketta.PoolManagement.Tests
{
	public class TestObjectPoolBehaviour
	{
		public class PoolableObject : MonoBehaviour { }
		public class TestableObjectPoolBehaviour : ObjectPoolBehaviour<PoolableObject>
		{
			protected override void Awake()
			{
				prefab = new GameObject().AddComponent<PoolableObject>();
				base.Awake();
			}
		}

		[Test]
		public void RequestObject_AtAll_IsNotNull()
		{
			// Arrange
			var pool = CreateDefaultObjectPoolBehaviour();

			// Act
			var @object = pool.RequestObject();

			// Assert
			Assert.IsNotNull(@object);

			// Clean up
			Object.Destroy(pool);
		}

		[UnityTest]
		public IEnumerator Destroy_ObjectPoolBehaviour_Disposes()
		{
			// Arrange
			var pool = CreateDefaultObjectPoolBehaviour();
			var @object = pool.RequestObject();
			pool.ReturnObject(@object);

			// Act
			Object.Destroy(pool);

			yield return null;

			// Assert
			Assert.IsTrue(@object == null);
		}

		[Test]
		public void Instantiation_PoolBehaviour_GetsPlacedUnderPoolParent()
		{
			// Arrange
			var pool = CreateDefaultObjectPoolBehaviour();

			// Act
			var @object = pool.RequestObject();
			pool.ReturnObject(@object);

			// Assert
			Assert.AreEqual(pool.transform, @object.transform.parent);

			// Clean up
			Object.Destroy(pool);
		}


		private ObjectPoolBehaviour<PoolableObject> CreateDefaultObjectPoolBehaviour()
		{
			return new GameObject().AddComponent<TestableObjectPoolBehaviour>();
		}
	} 
}
