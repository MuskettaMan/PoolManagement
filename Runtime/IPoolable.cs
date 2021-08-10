namespace Musketta.PoolManagement
{
	/// <summary>
	/// Defines behaviour for objects that are able to be pooled.
	/// </summary>
	public interface IPoolable
	{
		/// <summary>
		/// Should be called when the object has been requested from the <see cref="ObjectPool{T}"/>.
		/// </summary>
		/// <remarks>Should only be called by the <see cref="PoolableObjectPoolManagementService{T}"/> that was passed to the <see cref="ObjectPool{T}"/> this object belongs to.</remarks>
		void Requested();

		/// <summary>
		/// Should be called when the object has been returned to the <see cref="ObjectPool{T}"/>.
		/// </summary>
		/// <remarks>Should only be called by the <see cref="PoolableObjectPoolManagementService{T}"/> that was passed to the <see cref="ObjectPool{T}"/> this object belongs to.</remarks>
		void Returned();
	}
}