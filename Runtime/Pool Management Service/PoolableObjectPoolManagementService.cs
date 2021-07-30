public class PoolableObjectPoolManagementService<T> : IPoolManagementService<T> where T : IPoolable
{
	public void ObjectCreated(T @object) { }
	public void ObjectDestroyed(T @object) { }
	public void ObjectRequested(T @object) => @object.Requested();
	public void ObjectReturned(T @object) => @object.Returned();
}
