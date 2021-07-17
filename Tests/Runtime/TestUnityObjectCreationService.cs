using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public class TestUnityObjectCreationService
{
	[Test]
	public void Create_WithNoParent_CanBeFoundInScene()
	{
		MeshRenderer prefab = new GameObject("Initial Object").AddComponent<MeshRenderer>();
		ICreationService<MeshRenderer> creationService = new UnityObjectCreationService<MeshRenderer>(prefab);

		var created = creationService.Create();

		var found = UnityObject.FindObjectsOfType<MeshRenderer>();
		Assert.IsTrue(Array.IndexOf(found, created) != -1);

		UnityObject.Destroy(prefab);
		UnityObject.Destroy(created);
	}

	[Test]
	public void Create_WithParent_IsUnderRightParent()
	{
		Transform parent = new GameObject("Parent").transform;
		MeshRenderer prefab = new GameObject("Initial Object").AddComponent<MeshRenderer>();
		ICreationService<MeshRenderer> creationService = new UnityObjectCreationService<MeshRenderer>(prefab, parent);

		var created = creationService.Create();

		Assert.AreEqual(parent, created.transform.parent);
	}

	[Test]
	public void Constructing_WithNullPrefab_ThrowsException()
	{
		TestDelegate testDelegate = () => new UnityObjectCreationService<MeshRenderer>(null);

		Assert.Throws<ArgumentNullException>(testDelegate);
	}
}
