using UnityEngine;

namespace Musketta.PoolManagement
{
	/// <summary>
	/// An implementation of <see cref="ObjectPool{T}"/> but used for components that implement the <see cref="IPoolable"/> interface for state callbacks.
	/// </summary>
	/// <typeparam name="T">The type of the component that implements the <see cref="IPoolable"/> interface.</typeparam>
	public class PoolableComponentObjectPoolBehaviour<T> : ComponentObjectPoolBehaviour<T> where T : Component, IPoolable
	{
		#region Methods
		#region Protected
		/// <summary>
		/// Initializes the pool management service to <see cref="PoolableComponentPoolManagementService{T}"/>.
		/// </summary>
		/// <returns><inheritdoc/></returns>
		protected override IPoolManagementService<T> InitializePoolManagementService()
		{
			return new PoolableComponentPoolManagementService<T>(
				  new GameObjectPoolManagementService(useSendMessages, transform),
				  new PoolableObjectPoolManagementService<T>()
			);
		}
		#endregion
		#endregion
	} 
}