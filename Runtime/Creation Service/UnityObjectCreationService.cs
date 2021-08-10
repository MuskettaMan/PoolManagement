using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Musketta.PoolManagement
{
	/// <summary>
	/// The creation service the creates <see cref="UnityObject"/>.
	/// </summary>
	/// <typeparam name="T">The <see cref="UnityObject"/> to be created.</typeparam>
	public class UnityObjectCreationService<T> : ICreationService<T> where T : UnityObject
	{
		#region Variables
		#region Private
		/// <summary>
		/// The original instance to create new ones of.
		/// </summary>
		private readonly T prefab;

		/// <summary>
		/// The parent to instantiate the objects into.
		/// </summary>
		private readonly Transform parent;
		#endregion
		#endregion

		#region Initialization
		/// <summary>
		/// Creates a new <see cref="UnityObjectCreationService{T}"/> that can create new <see cref="UnityObject"/>.
		/// </summary>
		/// <param name="prefab">The original instance to create new ones of.</param>
		/// <param name="parent">The parent to instantiate the objects into. If null passed it will create in the root of the active scene.</param>
		public UnityObjectCreationService(T prefab, Transform parent = null)
		{
			_ = prefab ?? throw new ArgumentNullException(nameof(prefab));

			this.prefab = prefab;
			this.parent = parent;
		}
		#endregion

		#region Methods
		#region Public
		/// <summary>
		/// Instantiates a new <see cref="T"/>.
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public T Create() => UnityObject.Instantiate(prefab, parent);
		#endregion
		#endregion
	} 
}
