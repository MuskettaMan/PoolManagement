using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructionService<T>
{
	void Destroy(T @object);
}