using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestableComponentObjectPoolBehaviour : ComponentObjectPoolBehaviour<Transform>
{
	public GameObjectPoolManagementService GameObjectPoolManagementService { get; private set; }
	public ComponentPoolManagementService<Transform> ComponentPoolManagementService { get; private set; }

	protected override void Awake()
	{
		GameObjectPoolManagementService = Substitute.For<GameObjectPoolManagementService>(true, transform);
		ComponentPoolManagementService = new ComponentPoolManagementService<Transform>(GameObjectPoolManagementService);
		prefab = new GameObject().transform;
		base.Awake();
	}

	protected override IPoolManagementService<Transform> InitializePoolManagementService() => ComponentPoolManagementService;
}
