using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyDestructionService<T> : IDestructionService<T>
{
	public void Destroy(T @object) { }
}
