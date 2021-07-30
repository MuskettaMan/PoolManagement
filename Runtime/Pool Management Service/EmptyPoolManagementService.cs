public class EmptyPoolManagementService<T> : IPoolManagementService<T>
{
	public void ObjectCreated(T @object) { }

	public void ObjectRequested(T @object) { }

	public void ObjectReturned(T @object) { }

	public void ObjectDestroyed(T @object) { }
}
