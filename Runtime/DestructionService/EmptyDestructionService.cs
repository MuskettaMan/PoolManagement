public class EmptyDestructionService<T> : IDestructionService<T>
{
	public void Destroy(T @object) { }
}
