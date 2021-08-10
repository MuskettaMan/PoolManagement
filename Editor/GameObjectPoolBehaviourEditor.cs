#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Musketta.PoolManagement.Editor
{
	[CustomEditor(typeof(GameObjectPoolBehaviour))]
	public class GameObjectPoolBehaviourEditor : ObjectPoolBehaviourEditor<GameObject> { }
}

#endif