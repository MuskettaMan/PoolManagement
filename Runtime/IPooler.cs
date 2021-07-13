public interface IPooler<T>
{
	T RequestObject();
	void ReturnObject(T @object);
}
