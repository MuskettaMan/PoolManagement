using System;
using UnityEngine;

namespace Musketta.PoolManagement
{
	/// <summary>
	/// Implementation of <see cref="IDestructionService{T}"/> that can destroy <see cref="Component"/>s.
	/// </summary>
	/// <typeparam name="T">The component to be destroyed. (This can be any <see cref="MonoBehaviour"/>.)</typeparam>
	public class ComponentDestructionService<T> : IDestructionService<T> where T : Component
	{
		#region Variables
		#region Private
		/// <summary>
		/// The unity object destruction service used to destroy the components.
		/// </summary>
		private readonly UnityObjectDestructionService<GameObject> unityObjectDestructionService;
		#endregion
		#endregion

		#region Initialization
		/// <summary>
		/// Creates a <see cref="ComponentDestructionService{T}"/> based on the <see cref="UnityObjectDestructionService{T}"/>.
		/// </summary>
		/// <param name="unityObjectDestructionService">The unity object destruction service to base this destruction service on.</param>
		public ComponentDestructionService(UnityObjectDestructionService<GameObject> unityObjectDestructionService)
		{
			_ = unityObjectDestructionService ?? throw new ArgumentNullException(nameof(unityObjectDestructionService));

			this.unityObjectDestructionService = unityObjectDestructionService;
		}
		#endregion

		#region Methods
		#region Public
		/// <summary>
		/// Destroys the given <paramref name="object"/>.
		/// </summary>
		/// <param name="object">The object to destroy.</param>
		public void Destroy(T @object)
		{
			if (@object == null)
				throw new ArgumentNullException(nameof(@object));

			unityObjectDestructionService.Destroy(@object.gameObject);
		}
		#endregion
		#endregion
	}
}