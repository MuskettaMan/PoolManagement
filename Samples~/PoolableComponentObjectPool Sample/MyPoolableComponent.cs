using Musketta.PoolManagement;
using UnityEngine;

public class MyPoolableComponent : MonoBehaviour, IPoolable
{
	public void Requested()
	{
		Debug.Log("I've been requested :)!");
	}

	public void Returned()
	{
		Debug.Log("I've been returned :(!");
	}
}