using UnityEngine;

public class GameObjectPoolManagementService : IPoolManagementService<GameObject>
{
	private bool useSendMessages;
	private Transform originalParent;

	public GameObjectPoolManagementService(bool useSendMessages)
	{
		this.useSendMessages = useSendMessages;
	}

	public void ObjectCreated(GameObject @object)
	{
		originalParent = @object.transform;
		@object.SetActive(false);
	}

	public void ObjectRequested(GameObject @object)
	{
		@object.SetActive(true);
		if(useSendMessages)
			@object.SendMessage("Requested", SendMessageOptions.DontRequireReceiver);
	}

	public void ObjectReturned(GameObject @object)
	{
		if (useSendMessages)
			@object.SendMessage("Returned", SendMessageOptions.DontRequireReceiver);
		@object.transform.SetParent(originalParent);
		@object.SetActive(false);
	}

	public void ObjectDestroyed(GameObject @object) { }
}
