#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ObjectPool<>.ObjectPoolConfig))]
public class ObjectPoolConfigDrawer : PropertyDrawer
{
	private SerializedProperty initialCapacityProperty;
	private SerializedProperty shouldDestroyInUseProperty;
	private SerializedProperty shouldDestroyPooledProperty;

	private const int MARGIN = 10;
	private const int HEADER_SPACING = 3;
	private const int HEADER_SIZE = 20;
	private const int PROPERTY_FIELD_SIZE = 20;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => MARGIN + HEADER_SIZE + HEADER_SPACING + PROPERTY_FIELD_SIZE * 3 + MARGIN;
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		initialCapacityProperty = property.FindPropertyRelative("initialCapacity");
		shouldDestroyInUseProperty = property.FindPropertyRelative("shouldDestroyInUseObjects");
		shouldDestroyPooledProperty = property.FindPropertyRelative("shouldDestroyPooledObjects");

		position.y += MARGIN;
		Rect labelRect = new Rect(position.x, position.y, position.width, HEADER_SIZE);
		GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
		labelStyle.fontStyle = FontStyle.Bold;
		GUI.Label(labelRect, "Objectpool Configuration", labelStyle);

		EditorGUI.BeginDisabledGroup(Application.isPlaying);
		{
			EditorGUI.BeginProperty(position, label, property);
			{
				position.y += HEADER_SIZE + HEADER_SPACING;
				Rect initialCapacityRect = new Rect(position.x, position.y, position.width, PROPERTY_FIELD_SIZE);
				Rect shouldDestroyInUseRect = new Rect(position.x, position.y + PROPERTY_FIELD_SIZE, position.width, PROPERTY_FIELD_SIZE);
				Rect shouldDestroyPooledRect = new Rect(position.x, position.y + PROPERTY_FIELD_SIZE * 2, position.width, PROPERTY_FIELD_SIZE);

				EditorGUI.PropertyField(initialCapacityRect, initialCapacityProperty, new GUIContent("Initial Capacity", "The amount of objects the pool will instantiate with."));
				EditorGUI.PropertyField(shouldDestroyInUseRect, shouldDestroyInUseProperty, new GUIContent("Should Destroy In Use", "When the ObjectPool is destroyed should all the objects that are in use be destroyed as well."));
				EditorGUI.PropertyField(shouldDestroyPooledRect, shouldDestroyPooledProperty, new GUIContent("Should Destroy Pooled", "When the ObjectPool is destroyed should all the objects that are in the pool be destroyed as well."));
			}
			EditorGUI.EndProperty();
		}
		EditorGUI.EndDisabledGroup();
	}
}


#endif