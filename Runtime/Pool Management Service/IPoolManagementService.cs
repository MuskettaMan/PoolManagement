namespace Musketta.PoolManagement
{
	/// <summary>
	/// Defines behaviour for how the objects inside an object pool can be managed.
	/// </summary>
	/// <typeparam name="T">The type of the objects to be managed.</typeparam>
	public interface IPoolManagementService<T>
	{
		/// <summary>
		/// Will be called when the given <paramref name="object"/> is created in the <see cref="ObjectPool{T}"/>.
		/// </summary>
		/// <param name="object">The object that was created.</param>
		/// <remarks>This should only be called by the <see cref="ObjectPool{T}"/>.</remarks>
		void ObjectCreated(T @object);

		/// <summary>
		/// Will be called when the given <paramref name="object"/> is requested from it's pool in the <see cref="ObjectPool{T}"/>.
		/// </summary>
		/// <param name="object">The object that was requested.</param>
		/// <remarks>This should only be called by the <see cref="ObjectPool{T}"/>.</remarks>
		void ObjectRequested(T @object);

		/// <summary>
		/// Will be called when the given <paramref name="object"/> is returned to it's pool in the <see cref="ObjectPool{T}"/>.
		/// </summary>
		/// <param name="object">The object that was returned.</param>
		/// <remarks>This should only be called by the <see cref="ObjectPool{T}"/>.</remarks>
		void ObjectReturned(T @object);

		/// <summary>
		/// Will be called when the given <paramref name="object"/> is destroyed in the <see cref="ObjectPool{T}"/>.
		/// </summary>
		/// <param name="object">The object that was destroyed.</param>
		/// <remarks>This should only be called by the <see cref="ObjectPool{T}"/>.</remarks>
		void ObjectDestroyed(T @object);
	}
}