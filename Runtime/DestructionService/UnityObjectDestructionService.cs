using UnityObject = UnityEngine.Object;

public class UnityObjectDestructionService<T> : IDestructionService<T> where T : UnityObject
{
	public void Destroy(T @object)
	{
		UnityObject.Destroy(@object);
	}
}