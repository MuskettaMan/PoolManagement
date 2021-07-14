using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[TestFixture]
public class TestPrefabCreationService
{
	[Test]
	public void Create_PrefabAsTransform_InstantiatesIntoScene()
	{
		// Arrange
		Transform testObject = new GameObject().transform;
		UnityObjectCreationService<Transform> prefabCreationService = new UnityObjectCreationService<Transform>(testObject);

		// Act
		Transform instantiatedObject = prefabCreationService.Create();

		// Assert
		Transform[] allTransforms = Object.FindObjectsOfType<Transform>();
		Assert.IsTrue(allTransforms.Any((Transform t) => t == instantiatedObject));

		// Clean up
		Object.Destroy(instantiatedObject.gameObject);
		Object.Destroy(testObject.gameObject);
	}
}
