/// <summary>
/// Define behaviour for pooling objects.
/// </summary>
/// <typeparam name="T">The type of objects to pool.</typeparam>
public interface IPooler<T>
{
	/// <summary>
	/// Should return an object from the pool that the user can use.
	/// </summary>
	/// <returns>The object that was requested from the pool.</returns>
	T RequestObject();

	/// <summary>
	/// Should return the given <paramref name="object"/> so it can be used later.
	/// </summary>
	/// <param name="object">The object to return to the pool.</param>
	void ReturnObject(T @object);
}
