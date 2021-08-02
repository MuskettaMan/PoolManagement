#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public static class EditorCreator
{
	private const string TYPE_NAME = "TYPE_NAME";
	private const string GENERIC_NAME = "GENERIC_NAME";
	private const string PACKAGE_PATH = "Packages/musketta.poolmanagement";

	[MenuItem("Assets/PoolManagement/Create Editor Script")]
	public static void MenuThing()
	{
		MonoScript scriptAsset = (MonoScript)Selection.activeObject;
		Type systemType = scriptAsset.GetClass();

		TextAsset template = AssetDatabase.LoadAssetAtPath<TextAsset>($"{PACKAGE_PATH}/Editor/PoolTemplateEditor.txt");
		string scriptContents = template.text;
		scriptContents = scriptContents.Replace(TYPE_NAME, systemType.Name);
		scriptContents = scriptContents.Replace(GENERIC_NAME, FindObjectPoolGeneric(systemType).Name);
		string scriptAssetPath = AssetDatabase.GetAssetPath(scriptAsset);
		string editorScriptPath = $"{scriptAssetPath.Remove(scriptAssetPath.LastIndexOf('/'))}/{systemType.Name}Editor.cs";
		File.WriteAllText(Path.GetFullPath(editorScriptPath), scriptContents);
		AssetDatabase.Refresh();
	}

	[MenuItem("Assets/PoolManagement/Create Editor Script", true)]
	public static bool MenuThingValidation()
	{
		MonoScript scriptAsset = Selection.activeObject as MonoScript;
		if (scriptAsset == null)
			return false;

		Type systemType = scriptAsset.GetClass();
		if (!IsDerivedFrom(typeof(ObjectPoolBehaviour<>), systemType))
			return false;

		return true;
	}

	private static Type FindObjectPoolGeneric(Type sample)
	{
		Type ongoing = sample;
		while (ongoing != null)
		{
			ongoing = ongoing.BaseType;
			if (ongoing.GetGenericTypeDefinition() == typeof(ObjectPoolBehaviour<>))
				return ongoing.GetGenericArguments()[0];
		}

		return null;
	}

	private static bool IsDerivedFrom(Type baseType, Type sample)
	{
		Type ongoing = sample;
		while (ongoing != null)
		{
			ongoing = ongoing.BaseType;
			if (ongoing.GetGenericTypeDefinition() == baseType)
				return true;
		}

		return false;
	}
}

#endif