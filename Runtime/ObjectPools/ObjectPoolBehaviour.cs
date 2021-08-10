using UnityEngine;

namespace Musketta.PoolManagement
{
	/// <summary>
	/// An implementation for <see cref="ObjectPool"/> that's usable as MonoBehaviour.
	/// This can pool any <see cref="Object"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DisallowMultipleComponent]
	public abstract class ObjectPoolBehaviour<T> : MonoBehaviour, IPooler<T> where T : Object
	{
		#region Variables
		#region Editor
		/// <summary>
		/// The prefab to pool, this will be the original for all the objects that will be inside the pool.
		/// </summary
		[SerializeField]
		[Tooltip("The prefab to pool, this will be the original for all the objects that will be inside the pool.")]
		protected T prefab;

		/// <summary>
		/// The parent <see cref="Transform"/> in which all the pooled objects will be stored.
		/// </summary>
		[SerializeField]
		[Tooltip("The parent Transform in which all the pooled objects will be stored.")]
		private Transform pooledObjectsParent;

		/// <summary>
		/// The configuration for this object pool. This contains any specific behaviour details, that can be tweaked.
		/// </summary>
		[SerializeField]
		[Tooltip("The configuration for this object pool. This contains any specific behaviour details, that can be tweaked.")]
		private ObjectPool<T>.ObjectPoolConfig config;
		#endregion

		#region Protected
		/// <summary>
		/// The service used for creating the <see cref="Object"/>s.
		/// </summary>
		protected ICreationService<T> CreationService { get; private set; }

		/// <summary>
		/// The service used for creating the <see cref="Object"/>s.
		/// </summary>
		protected IPoolManagementService<T> PoolManagementService { get; private set; }

		/// <summary>
		/// The service used for creating the <see cref="Object"/>s.
		/// </summary>
		protected IDestructionService<T> DestructionService { get; private set; }

		/// <summary>
		/// The inner <see cref="ObjectPool{T}"/> that's used for managing the object pooling.
		/// </summary>
		/// <remarks>This is implemented as a composition instead of an inheritance is due to that this object has to inherit from <see cref="MonoBehaviour"/></remarks>
		protected ObjectPool<T> ObjectPool { get; private set; }
		#endregion
		#endregion

		#region Methods
		#region Private
		/// <summary>
		/// Initializes the services, done by calling the specific initialization methods and assigning them to the fields that store these.
		/// </summary>
		private void InitializeServices()
		{
			CreationService = InitializeCreationService();
			PoolManagementService = InitializePoolManagementService();
			DestructionService = InitializeDestructionService();
		}
		#endregion

		#region Protected
		/// <summary>
		/// Initializes the services and creates an instance of the <see cref="ObjectPool{T}"/>.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		protected virtual void Awake()
		{
			if (pooledObjectsParent == null)
				pooledObjectsParent = transform;

			try
			{
				InitializeServices();
			}
			catch
			{
				Debug.LogError(
					"There was an issue during the initialization of the object pool services. " +
					"Please make sure that during the override of any services that no exceptions are thrown. " +
					"Do check your other logs and errors to resolve the current issue."
				);
				throw;
			}

			ObjectPool = new ObjectPool<T>(
				CreationService ?? new UnityObjectCreationService<T>(prefab, pooledObjectsParent),
				PoolManagementService ?? new EmptyPoolManagementService<T>(),
				DestructionService ?? new UnityObjectDestructionService<T>(),
				config
			);
		}

		/// <summary>
		/// Disposes the <see cref="ObjectPool"/> to make sure all the objects that were pooled and in use also get properly clean up.
		/// </summary>
		protected virtual void OnDestroy()
		{
			if (ObjectPool != null)
				ObjectPool.Dispose();
		}

		/// <summary>
		/// Resets the component to make sure it's in the right state.
		/// </summary>
		protected virtual void Reset()
		{
			config = new ObjectPool<T>.ObjectPoolConfig(0, true, false);
			pooledObjectsParent = transform;
		}

		/// <summary>
		/// Initializes the creation service. If not overridden this will default to <see cref="UnityObjectCreationService{T}"/>.
		/// </summary>
		/// <returns>The <see cref="ICreationService{T}"/> that should be responsible for creating new objects used by the pool.</returns>
		protected virtual ICreationService<T> InitializeCreationService() => new UnityObjectCreationService<T>(prefab, pooledObjectsParent);

		/// <summary>
		/// Initializes the pool management service. If not overridden this will default to <see cref="EmptyPoolManagementService{T}"/>.
		/// </summary>
		/// <returns>The <see cref="IPoolManagementService{T}"/> that should be responsible for managing objects used by the pool.</returns>
		protected virtual IPoolManagementService<T> InitializePoolManagementService() => new EmptyPoolManagementService<T>();

		/// <summary>
		/// Initializes the destruction service. If not overridden this will default to <see cref="UnityObjectDestructionService{T}"/>.
		/// </summary>
		/// <returns>The <see cref="IDestructionService{T}"/> that should be responsible for destructing objects used by the pool.</returns>
		protected virtual IDestructionService<T> InitializeDestructionService() => new UnityObjectDestructionService<T>();
		#endregion

		#region Public
		/// <summary>
		/// Returns an object from the pool so it can be used.
		/// </summary>
		/// <returns>The object from the pool that can be used.</returns>
		public T RequestObject() => ObjectPool.RequestObject();

		/// <summary>
		/// Returns the given <paramref name="object"/> so it can be reused.
		/// </summary>
		/// <param name="object">The object to return, so it can be reused.</param>
		public void ReturnObject(T @object) => ObjectPool.ReturnObject(@object);
		#endregion
		#endregion
	} 
}