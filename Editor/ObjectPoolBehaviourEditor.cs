#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
public class ObjectPoolBehaviourEditor<T> : Editor where T : Object
{
	private ObjectPoolBehaviour<T>[] objectPools;
	private ObjectPoolBehaviour<T> objectPool => objectPools[0];

	private const int COMBINED_POOL_OBJECT_TOTAL_CAP = 100;

	private void OnEnable()
	{
		objectPools = new ObjectPoolBehaviour<T>[targets.Length];
		for (int i = 0; i < objectPools.Length; i++)
		{
			objectPools[i] = (ObjectPoolBehaviour<T>)targets[i];
		}
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUIStyle boldLabel = new GUIStyle(GUI.skin.label);
		boldLabel.fontStyle = FontStyle.Bold;

		if (Application.isPlaying)
		{
			if (objectPool.ObjectPool == null)
				return;

			EditorGUILayout.Space();

			if (objectPool.ObjectPool.InUse.Count + objectPool.ObjectPool.Pooled.Count > COMBINED_POOL_OBJECT_TOTAL_CAP)
			{
				EditorGUILayout.HelpBox($"Because there are more than {COMBINED_POOL_OBJECT_TOTAL_CAP} {typeof(T).Name}s being pooled the extended editor won't be shown.", MessageType.Warning);
				return;
			}

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width / 2 - 13));
				{
					GUILayout.Label($"In Use {typeof(T).Name}{(objectPool.ObjectPool.InUse.Count > 1 ? "s" : string.Empty)}", boldLabel);

					if (objectPool.ObjectPool.InUse.Count == 0)
						EditorGUILayout.HelpBox($"Currently there are no {typeof(T).Name}s in use.", MessageType.Info);

					for (int i = 0; i < objectPool.ObjectPool.InUse.Count; i++)
					{
						EditorGUILayout.BeginHorizontal();
						{
							EditorGUI.BeginDisabledGroup(true);
							{
								EditorGUILayout.ObjectField(objectPool.ObjectPool.InUse[i], typeof(T), true);
							}
							EditorGUI.EndDisabledGroup();
							if (GUILayout.Button("\u2192"))
							{
								objectPool.ObjectPool.ReturnObject(objectPool.ObjectPool.InUse[i]);
								return;
							}
						}
						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width / 2 - 13));
				{
					GUILayout.Label($"Pooled {typeof(T).Name}{(objectPool.ObjectPool.Pooled.Count > 1 ? "s" : string.Empty)}", boldLabel);

					if (objectPool.ObjectPool.Pooled.Count == 0)
						EditorGUILayout.HelpBox($"Currently there are no {typeof(T).Name}s in the pool.", MessageType.Info);

					for (int i = 0; i < objectPool.ObjectPool.Pooled.Count; i++)
					{
						EditorGUI.BeginDisabledGroup(true);
						{
							EditorGUILayout.ObjectField(objectPool.ObjectPool.Pooled[i], typeof(T), true, GUILayout.Height(19));
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