namespace Musketta.PoolManagement
{
	/// <summary>
	/// Defines behaviour for destroying a given object.
	/// </summary>
	/// <typeparam name="T">The type of object to destroy</typeparam>
	public interface IDestructionService<T>
	{
		/// <summary>
		/// Should destroy the given <paramref name="object"/>.
		/// </summary>
		/// <param name="object">The object that should be destroyed.</param>
		void Destroy(T @object);
	} 
}