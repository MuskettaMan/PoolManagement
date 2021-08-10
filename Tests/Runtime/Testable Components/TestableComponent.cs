using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Musketta.PoolManagement.Tests
{
	public class TestableComponent : MonoBehaviour
	{
		public bool RequestedCalled { get; private set; }
		public bool ReturnedCalled { get; private set; }

		public void Requested() => RequestedCalled = true;
		public void Returned() => ReturnedCalled = true;

		private void Awake()
		{
			name = nameof(TestableComponent);
		}
	} 
}
