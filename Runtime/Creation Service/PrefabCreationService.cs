using Object = UnityEngine.Object;

public class PrefabCreationService<T> : ICreationService<T> where T : Object
{
	private readonly T prefab;

	public PrefabCreationService(T prefab)
	{
		this.prefab = prefab;
	}
	public T Create()
	{
		return Object.Instantiate(prefab);
	}
}
