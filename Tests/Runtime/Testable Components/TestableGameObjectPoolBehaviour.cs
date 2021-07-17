using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestableGameObjectPoolBehaviour : GameObjectPoolBehaviour
{
	public GameObjectPoolManagementService GameObjectPoolManagementService { get; private set; } = new GameObjectPoolManagementService(true);

	protected override void Awake()
	{
		name = nameof(TestableGameObjectPoolBehaviour);
		prefab = new GameObject().AddComponent<TestableComponent>().gameObject;
		base.Awake();
	}

	protected override IPoolManagementService<GameObject> InitializePoolManagementService() => GameObjectPoolManagementService;
}