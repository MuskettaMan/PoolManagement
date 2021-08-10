namespace Musketta.PoolManagement
{
	/// <summary>
	/// Defines behaviour for creating objects.
	/// </summary>
	/// <typeparam name="T">The type of objects to create.</typeparam>
	public interface ICreationService<T>
	{
		/// <summary>
		/// Should create an object.
		/// </summary>
		/// <returns>The object that was created.</returns>
		T Create();
	} 
}