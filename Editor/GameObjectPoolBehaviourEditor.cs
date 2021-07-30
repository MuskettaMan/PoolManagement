#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameObjectPoolBehaviour))]
public class GameObjectPoolBehaviourEditor : ObjectPoolBehaviourEditor<GameObject> { }

#endif