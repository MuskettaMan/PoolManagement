using UnityEngine;

/// <summary>
/// Management service for <see cref="GameObject"/>s. Handles setting the parents, the <see cref="GameObject.activeSelf"/> state and messaging.
/// </summary>
public class GameObjectPoolManagementService : IPoolManagementService<GameObject>
{
	#region Variables
	#region Private
	/// <summary>
	/// Whether to make use of Unity's message system.
	/// With this enabled Components on the GameObjects that are being pooled will get callbacks for when they are requested and returned.
	/// </summary>
	private readonly bool useSendMessages;

	/// <summary>
	/// The original parent of object pool so we can use this to place the returned objects back as children.
	/// </summary>
	private readonly Transform originalParent;
	#endregion
	#endregion

	#region Initialization
	/// <summary>
	/// Creates a new <see cref="GameObjectPoolManagementService"/> that handles setting the parents, the <see cref="GameObject.activeSelf"/> state and messaging.
	/// </summary>
	/// <param name="useSendMessages">Whether to make use of Unity's message system.</param>
	/// <param name="originalParent">The original parent of object pool so we can use this to place the returned objects back as children.</param>
	public GameObjectPoolManagementService(bool useSendMessages, Transform originalParent)
	{
		this.useSendMessages = useSendMessages;
		this.originalParent = originalParent;
	}
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// When the <see cref="GameObject"/> is created it sets it to inactive.
	/// </summary>
	/// <param name="object"><inheritdoc/></param>
	public void ObjectCreated(GameObject @object)
	{
		@object.SetActive(false);
	}

	/// <summary>
	/// When the <see cref="GameObject"/> is requested it's set to active and, if enabled, a message gets send.
	/// </summary>
	/// <param name="object"><inheritdoc/></param>
	public void ObjectRequested(GameObject @object)
	{
		@object.SetActive(true);
		if (useSendMessages)
			@object.SendMessage("Requested", SendMessageOptions.DontRequireReceiver);
	}

	/// <summary>
	/// When the <see cref="GameObject"/> is returned it's set to inactive and, if enabled, a message gets send.
	/// </summary>
	/// <param name="object"><inheritdoc/></param>
	public void ObjectReturned(GameObject @object)
	{
		if (useSendMessages)
			@object.SendMessage("Returned", SendMessageOptions.DontRequireReceiver);
		@object.transform.SetParent(originalParent);
		@object.SetActive(false);
	}

	/// <summary>
	/// Nothing happens when the object is destroyed.
	/// </summary>
	/// <param name="object"><inheritdoc/></param>
	public void ObjectDestroyed(GameObject @object) { }
	#endregion
	#endregion
}
