using UnityEngine;

namespace Musketta.PoolManagement
{
	/// <summary>
	/// Implementation for <see cref="IPoolManagementService{T}"/> for Components.
	/// </summary>
	/// <typeparam name="T">The component type to manage.</typeparam>
	public class ComponentPoolManagementService<T> : IPoolManagementService<T> where T : Component
	{
		#region Variables
		#region Private
		/// <summary>
		/// The <see cref="GameObjectPoolManagementService"/> to base this service on.
		/// </summary>
		private readonly GameObjectPoolManagementService gameObjectPoolManagementService;
		#endregion
		#endregion

		#region Initialization
		/// <summary>
		/// Creates a new <see cref="ComponentPoolManagementService{T}"/> based on a <see cref="GameObjectPoolManagementService"/>.
		/// </summary>
		/// <param name="gameObjectPoolManagementService">Used to base this service on.</param>
		public ComponentPoolManagementService(GameObjectPoolManagementService gameObjectPoolManagementService) => this.gameObjectPoolManagementService = gameObjectPoolManagementService;
		#endregion

		#region Methods
		#region Public
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="object"><inheritdoc/></param>
		public virtual void ObjectCreated(T @object) => gameObjectPoolManagementService.ObjectCreated(@object.gameObject);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="object"><inheritdoc/></param>
		public virtual void ObjectRequested(T @object) => gameObjectPoolManagementService.ObjectRequested(@object.gameObject);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="object"><inheritdoc/></param>
		public virtual void ObjectReturned(T @object) => gameObjectPoolManagementService.ObjectReturned(@object.gameObject);

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="object"><inheritdoc/></param>
		public virtual void ObjectDestroyed(T @object) => gameObjectPoolManagementService.ObjectDestroyed(@object.gameObject);
		#endregion
		#endregion
	}
}