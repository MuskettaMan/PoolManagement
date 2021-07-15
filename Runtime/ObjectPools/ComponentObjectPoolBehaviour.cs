using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentObjectPoolBehaviour<T> : ObjectPoolBehaviour<T> where T : Component
{
	[SerializeField]
	protected bool useSendMessages;
	protected override IPoolManagementService<T> InitializePoolManagementService()
	{
		return new ComponentPoolManagementService<T>(new GameObjectPoolManagementService(useSendMessages));
	}

	protected override IDestructionService<T> InitializeDestructionService()
	{
		return new ComponentDestructionService<T>(new UnityObjectDestructionService<GameObject>());
	}
}
