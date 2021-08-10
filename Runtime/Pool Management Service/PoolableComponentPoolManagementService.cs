using UnityEngine;

namespace Musketta.PoolManagement
{
	/// <summary>
	/// A pool management service for <see cref="Component"/>s that can give callback if they implement the <see cref="IPoolable"/> interface.
	/// </summary>
	/// <typeparam name="T">The <see cref="Component"/> that also implements the <see cref="IPoolable"/> interface.</typeparam>
	public class PoolableComponentPoolManagementService<T> : ComponentPoolManagementService<T> where T : Component, IPoolable
	{
		#region Variables
		#region Private
		/// <summary>
		/// The <see cref="PoolableObjectPoolManagementService{T}"/> that handles the management for the <see cref="IPoolable"/>s.
		/// </summary>
		private readonly PoolableObjectPoolManagementService<T> poolableObjectPoolManagementService;
		#endregion
		#endregion

		#region Initialization
		/// <summary>
		/// Creates a new <see cref="PoolableComponentPoolManagementService{T}"/> by combining 
		/// <see cref="GameObjectPoolManagementService"/> and <see cref="PoolableObjectPoolManagementService{T}"/>.
		/// </summary>
		/// <param name="gameObjectPoolManagementService">Used for the management of the <see cref="Component"/>.</param>
		/// <param name="poolableObjectPoolManagementService">The <see cref="PoolableObjectPoolManagementService{T}"/> that handles the management for the <see cref="IPoolable"/>s.</param>
		public PoolableComponentPoolManagementService(
			GameObjectPoolManagementService gameObjectPoolManagementService,
			PoolableObjectPoolManagementService<T> poolableObjectPoolManagementService
		) : base(gameObjectPoolManagementService) => this.poolableObjectPoolManagementService = poolableObjectPoolManagementService;
		#endregion

		#region Methods
		#region Public
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="object"><inheritdoc/></param>
		public override void ObjectCreated(T @object) => base.ObjectCreated(@object);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="object"><inheritdoc/></param>
		public override void ObjectDestroyed(T @object) => base.ObjectDestroyed(@object);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="object"><inheritdoc/></param>
		public override void ObjectRequested(T @object)
		{
			base.ObjectRequested(@object);
			poolableObjectPoolManagementService.ObjectRequested(@object);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="object"><inheritdoc/></param>
		public override void ObjectReturned(T @object)
		{
			@object.Returned();
			poolableObjectPoolManagementService.ObjectReturned(@object);
		}
		#endregion
		#endregion
	}
}