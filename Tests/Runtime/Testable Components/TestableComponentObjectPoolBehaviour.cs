using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestableComponentObjectPoolBehaviour : ComponentObjectPoolBehaviour<Transform>
{
	public GameObjectPoolManagementService GameObjectPoolManagementService { get; private set; } = Substitute.For<GameObjectPoolManagementService>(true);
	public ComponentPoolManagementService<Transform> ComponentPoolManagementService { get; private set; }

	protected override void Awake()
	{
		prefab = new GameObject().transform;
		ComponentPoolManagementService = new ComponentPoolManagementService<Transform>(GameObjectPoolManagementService);
		base.Awake();
	}

	protected override IPoolManagementService<Transform> InitializePoolManagementService() => ComponentPoolManagementService;
}
