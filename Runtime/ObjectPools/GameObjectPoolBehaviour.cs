using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolBehaviour : ObjectPoolBehaviour<GameObject>
{
	[SerializeField]
	private bool useSendMessages;

	protected override IPoolManagementService<GameObject> InitializePoolManagementService()
	{
		return new GameObjectPoolManagementService(useSendMessages);
	}
}