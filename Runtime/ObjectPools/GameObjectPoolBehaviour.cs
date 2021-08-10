using UnityEngine;

namespace Musketta.PoolManagement
{
	/// <summary>
	/// This is an object pool used to pool <see cref="GameObject"/> objects.
	/// </summary>
	public sealed class GameObjectPoolBehaviour : ObjectPoolBehaviour<GameObject>
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
		private bool useSendMessages;
		#endregion
		#endregion

		#region Methods
		#region Protected
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		protected override IPoolManagementService<GameObject> InitializePoolManagementService() => new GameObjectPoolManagementService(useSendMessages, transform);
		#endregion
		#endregion
	} 
}