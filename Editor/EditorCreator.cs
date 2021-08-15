#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Musketta.PoolManagement.Editor
{
	public static class EditorCreator
	{
		private const string TYPE_NAME = "TYPE_NAME";
		private const string GENERIC_NAME = "GENERIC_NAME";
		private const string NAMESPACE_NAME = "NAMESPACE";
		private const string PACKAGE_PATH = "Packages/musketta.poolmanagement";
		private const string SCRIPT_FILE_SUFFIX = "Editor";

		[MenuItem("Assets/PoolManagement/Create Editor Script")]
		public static void CreateObjectPoolBehaviourEditor()
		{
			MonoScript scriptAsset = (MonoScript)Selection.activeObject;
			Type systemType = scriptAsset.GetClass();

			TextAsset template = AssetDatabase.LoadAssetAtPath<TextAsset>($"{PACKAGE_PATH}/Editor/PoolTemplateEditor.txt");
			string scriptContents = template.text;
			Type genericType = FindObjectPoolGeneric(systemType);
			scriptContents = scriptContents.Replace(NAMESPACE_NAME, genericType.Namespace ?? "UnityEngine");
			scriptContents = scriptContents.Replace(TYPE_NAME, systemType.Name);
			scriptContents = scriptContents.Replace(GENERIC_NAME, genericType.Name);
			CreateEditorFile(scriptAsset, systemType, scriptContents);
		}

		[MenuItem("Assets/Pool Management/Create Editor Script", true)]
		public static bool CreateObjectPoolBehaviourEditorValidation()
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
				if (ongoing.IsGenericType && ongoing.GetGenericTypeDefinition() == baseType)
					return true;
				ongoing = ongoing.BaseType;
			}

			return false;
		}

		private static void CreateEditorFile(MonoScript scriptAsset, Type systemType, string scriptContents)
		{
			string scriptAssetPath = AssetDatabase.GetAssetPath(scriptAsset);
			scriptAssetPath = scriptAssetPath.Remove(scriptAssetPath.LastIndexOf('/'));
			string editorScriptPath = $"{scriptAssetPath}/{systemType.Name}{SCRIPT_FILE_SUFFIX}.cs";
			File.WriteAllText(Path.GetFullPath(editorScriptPath), scriptContents);
			AssetDatabase.Refresh();
		}
	}
}

#endif