/// <summary>
/// Used as null object for <see cref="IPoolManagementService{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the object that should be destroyed (but won't since this is a null object).</typeparam>
public sealed class EmptyPoolManagementService<T> : IPoolManagementService<T>
{
	#region Methods
	#region Public
	/// <summary>
	/// Does nothing when the object is created.
	/// </summary>
	/// <param name="object">The object nothing will happen to.</param>
	public void ObjectCreated(T @object) { }

	/// <summary>
	/// Does nothing when the object is requested.
	/// </summary>
	/// <param name="object">The object nothing will happen to.</param>
	public void ObjectRequested(T @object) { }

	/// <summary>
	/// Does nothing when the object is returned.
	/// </summary>
	/// <param name="object">The object nothing will happen to.</param>
	public void ObjectReturned(T @object) { }

	/// <summary>
	/// Does nothing when the object is destroyed.
	/// </summary>
	/// <param name="object">The object nothing will happen to.</param>
	public void ObjectDestroyed(T @object) { }
	#endregion
	#endregion
}
