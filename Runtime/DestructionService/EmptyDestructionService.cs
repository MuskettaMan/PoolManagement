namespace Musketta.PoolManagement
{
	/// <summary>
	/// Used as null object for <see cref="IDestructionService{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the object that should be destroyed (but won't since this is a null object).</typeparam>
	public sealed class EmptyDestructionService<T> : IDestructionService<T>
	{
		#region Methods
		#region Public
		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="object">The object that nothing will happen to.</param>
		public void Destroy(T @object) { }
		#endregion
		#endregion
	} 
}
