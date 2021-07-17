﻿using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityObject = UnityEngine.Object;

public class TestUnityObjectDestructionService
{
	[UnityTest]
	public IEnumerator Destroy_GameObject_IsNull()
	{
		GameObject @object = new GameObject();
		IDestructionService<UnityObject> destructionService = new UnityObjectDestructionService<UnityObject>();

		destructionService.Destroy(@object);
		yield return null;
		Assert.IsTrue(@object == null);
	}

	[Test]
	public void Destroy_WithNullParameter_ThrowsException()
	{
		IDestructionService<UnityObject> destructionService = new UnityObjectDestructionService<UnityObject>();
		TestDelegate testDelegate = () => destructionService.Destroy(null);

		Assert.Throws<ArgumentNullException>(testDelegate);
	}
}