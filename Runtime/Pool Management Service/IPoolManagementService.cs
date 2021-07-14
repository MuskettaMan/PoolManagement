public interface IPoolManagementService<T>
{
	void ObjectCreated(T @object);
	void ObjectRequested(T @object);
	void ObjectReturned(T @object);
	void ObjectDestroyed(T @object);
}