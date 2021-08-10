using UnityEngine;

namespace Musketta.PoolManagement
{
	/// <summary>
	/// Defines an implementation of the <see cref="ObjectPool{T}"/> but for <see cref="Component"/>s.
	/// </summary>
	/// <typeparam name="T">The component type to pool.</typeparam>
	public abstract class ComponentObjectPoolBehaviour<T> : ObjectPoolBehaviour<T> where T : Component
	{
		#region Variables
		#region Editor
		/// <summary>
		/// Whether to make use of Unity's message system.
		/// With this enabled Components on the GameObjects that are being pooled will get callbacks for when they are requested and returned.
		/// </summary>
		[SerializeField]
		[Tooltip("Whether to make use of Unity's message system. " +
				 "With this enabled Components on the GameObjects that are being pooled will get callbacks for when they are requested and returned. " +
				 "To make use of this you add the methods `void Requested() {...}` and `void Returned() {...}`. " +
				 "(This works in a similar manner as the `Awake`, `Start` and `Update` methods.)")]
		protected bool useSendMessages;
		#endregion
		#endregion

		#region Methods
		#region Protected
		/// <summary>
		/// Initializes the pool management service to <see cref="ComponentPoolManagementService{T}"/>.
		/// </summary>
		/// <returns><inheritdoc/></returns>
		protected override IPoolManagementService<T> InitializePoolManagementService()
			=> new ComponentPoolManagementService<T>(new GameObjectPoolManagementService(useSendMessages, transform));

		/// <summary>
		/// Initializes the pool management service to <see cref="ComponentDestructionService{T}"/>.
		/// </summary>
		/// <returns><inheritdoc/></returns>
		protected override IDestructionService<T> InitializeDestructionService()
			=> new ComponentDestructionService<T>(new UnityObjectDestructionService<GameObject>());
		#endregion
		#endregion
	}
}
