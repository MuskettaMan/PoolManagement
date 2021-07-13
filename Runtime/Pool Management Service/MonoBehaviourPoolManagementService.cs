using UnityEngine;

public class MonoBehaviourPoolManagementService<T> : IPoolManagementService<T> where T : MonoBehaviour
{
	public void ObjectCreated(T @object)
	{
		@object.gameObject.SetActive(false);
	}

	public void ObjectRequested(T @object)
	{
		@object.gameObject.SetActive(true);
		@object.SendMessage("Requested");
	}

	public void ObjectReturned(T @object)
	{
		@object.gameObject.SetActive(false);
		@object.SendMessage("Returned");
	}

	public void ObjectDestroyed(T @object) { }
}
