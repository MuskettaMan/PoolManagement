using System;
using UnityObject = UnityEngine.Object;

public class UnityObjectDestructionService<T> : IDestructionService<T> where T : UnityObject
{
	public void Destroy(T @object)
	{
		if (@object == null)
			throw new ArgumentNullException(nameof(@object));

		UnityObject.Destroy(@object);
	}
}