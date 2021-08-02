/// <summary>
/// A pool management service for objects that implement <see cref="IPoolable"/>.
/// This is useful for receiving callbacks on the objects that are being pooled.
/// </summary>
/// <typeparam name="T">The poolable object.</typeparam>
public class PoolableObjectPoolManagementService<T> : IPoolManagementService<T> where T : IPoolable
{
	#region Methods
	#region Public
	/// <summary>
	/// Does nothing, since objects already have callbacks for when they are created.
	/// </summary>
	/// <param name="object"><inheritdoc/></param>
	public void ObjectCreated(T @object) { }

	/// <summary>
	/// Does nothing, since objects already have callback for when they are destroyed.
	/// </summary>
	/// <param name="object"><inheritdoc/></param>
	public void ObjectDestroyed(T @object) { }

	/// <summary>
	/// Gives the given <paramref name="object"/> a callback that it was requested.
	/// </summary>
	/// <param name="object"><inheritdoc/></param>
	public void ObjectRequested(T @object) => @object.Requested();

	/// <summary>
	/// Gives the given <paramref name="object"/> a callback that it was returned.
	/// </summary>
	/// <param name="object"><inheritdoc/></param>
	public void ObjectReturned(T @object) => @object.Returned();
	#endregion
	#endregion
}
