using System;
using UnityObject = UnityEngine.Object;

namespace Musketta.PoolManagement
{
	/// <summary>
	/// Destruction service that handles the destruction of <see cref="UnityObject"/>.
	/// </summary>
	/// <typeparam name="T">The type of <see cref="UnityObject"/> to destroy.</typeparam>
	public class UnityObjectDestructionService<T> : IDestructionService<T> where T : UnityObject
	{
		#region Methods
		#region Public
		/// <summary>
		/// Destroys the given <see cref="UnityObject"/>.
		/// </summary>
		/// <param name="object"><inheritdoc/></param>
		public void Destroy(T @object)
		{
			_ = @object ?? throw new ArgumentNullException(nameof(@object));

			UnityObject.Destroy(@object);
		}
		#endregion
		#endregion
	} 
}