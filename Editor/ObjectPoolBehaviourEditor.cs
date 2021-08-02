#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

[CanEditMultipleObjects]
public class ObjectPoolBehaviourEditor<T> : Editor where T : Object
{
	private ObjectPoolBehaviour<T> objectPoolBehaviour;
	private ObjectPool<T> objectPool;

	private const int COMBINED_POOL_OBJECT_TOTAL_CAP = 100;

	private void OnEnable()
	{
		objectPoolBehaviour = (ObjectPoolBehaviour<T>)target;
		Type type = typeof(ObjectPoolBehaviour<T>);
		PropertyInfo propertyInfo = type.GetProperty("ObjectPool", BindingFlags.NonPublic | BindingFlags.Instance);
		objectPool = (ObjectPool<T>)propertyInfo.GetValue(objectPoolBehaviour);
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUIStyle boldLabel = new GUIStyle(GUI.skin.label);
		boldLabel.fontStyle = FontStyle.Bold;

		if (Application.isPlaying)
		{
			if (objectPool == null)
				return;

			EditorGUILayout.Space();

			if (objectPool.InUse.Count + objectPool.Pooled.Count > COMBINED_POOL_OBJECT_TOTAL_CAP)
			{
				EditorGUILayout.HelpBox($"Because there are more than {COMBINED_POOL_OBJECT_TOTAL_CAP} {typeof(T).Name}s being pooled the extended editor won't be shown.", MessageType.Warning);
				return;
			}

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 2 - 13));
				{
					GUILayout.Label($"In Use {typeof(T).Name}{(objectPool.InUse.Count > 1 ? "s" : string.Empty)}", boldLabel);

					if (objectPool.InUse.Count == 0)
						EditorGUILayout.HelpBox($"Currently there are no {typeof(T).Name}s in use.", MessageType.Info);

					for (int i = 0; i < objectPool.InUse.Count; i++)
					{
						EditorGUILayout.BeginHorizontal();
						{
							EditorGUI.BeginDisabledGroup(true);
							{
								EditorGUILayout.ObjectField(objectPool.InUse[i], typeof(T), true);
							}
							EditorGUI.EndDisabledGroup();
							if (GUILayout.Button("\u2192"))
							{
								objectPool.ReturnObject(objectPool.InUse[i]);
								return;
							}
						}
						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 2 - 13));
				{
					GUILayout.Label($"Pooled {typeof(T).Name}{(objectPool.Pooled.Count > 1 ? "s" : string.Empty)}", boldLabel);

					if (objectPool.Pooled.Count == 0)
						EditorGUILayout.HelpBox($"Currently there are no {typeof(T).Name}s in the pool.", MessageType.Info);

					for (int i = 0; i < objectPool.Pooled.Count; i++)
					{
						EditorGUI.BeginDisabledGroup(true);
						{
							EditorGUILayout.ObjectField(objectPool.Pooled[i], typeof(T), true, GUILayout.Height(19));
						}
						EditorGUI.EndDisabledGroup();
					}
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}

#endif