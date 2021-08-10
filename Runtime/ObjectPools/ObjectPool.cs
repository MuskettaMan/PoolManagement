using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Musketta.PoolManagement
{
	/// <summary>
	/// An object that 'pools' other objects. The idea being that this reuses the same object without actually removing it from memory.
	/// This is an optimization so it should be noted that this should only be uses after concluding that it can improve performance, 
	/// otherwise it will unnecessary complex.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ObjectPool<T> : IPooler<T>, IDisposable
	{
		#region Inner Types
		/// <summary>
		/// Contains all the properties that can be configured for an <see cref="ObjectPool{T}"/>.
		/// This contains data about how the ObjectPool should be configured.
		/// </summary>
		[Serializable]
		public class ObjectPoolConfig
		{
			/// <summary>
			/// The initial amount of objects an <see cref="ObjectPool{T}"/> will start off with.
			/// </summary>
			public int initialCapacity;

			/// <summary>
			/// Whether the <see cref="ObjectPool{T}"/> should destroy all it's pooled objects when it gets disposed.
			/// </summary>
			public bool shouldDestroyPooledObjects = true;

			/// <summary>
			/// Whether the <see cref="ObjectPool{T}"/> should destroy all the objects that have been retrieved from the pool before when it gets disposed.
			/// </summary>
			public bool shouldDestroyInUseObjects = false;

			/// <summary>
			/// Creates a new <see cref="ObjectPoolConfig"/> with the data to populate it.
			/// </summary>
			/// <param name="initialCapacity">
			/// The initial amount of objects an <see cref="ObjectPool{T}"/> will start off with.
			/// </param>
			/// <param name="shouldDestroyPooledObjects">
			/// Whether the <see cref="ObjectPool{T}"/> should destroy all it's pooled objects when it gets disposed.
			/// </param>
			/// <param name="shouldDestroyInUseObjects">
			/// Whether the <see cref="ObjectPool{T}"/> should destroy all the objects that have been retrieved from the pool before when it gets disposed.
			/// </param>
			public ObjectPoolConfig(int initialCapacity = 0, bool shouldDestroyPooledObjects = true, bool shouldDestroyInUseObjects = false)
			{
				// Clamps value to contain only positive or zero.
				this.initialCapacity = Mathf.Max(initialCapacity, 0);
				this.shouldDestroyPooledObjects = shouldDestroyPooledObjects;
				this.shouldDestroyInUseObjects = shouldDestroyInUseObjects;
			}
		}
		#endregion

		#region Variables
		#region Public
		/// <summary>
		/// All the objects that currently are being pooled inside this class.
		/// </summary>
		public ReadOnlyCollection<T> Pooled => new ReadOnlyCollection<T>(pooled.ToArray());

		/// <summary>
		/// All the objects that currently are in use, but originally belong to this pool.
		/// </summary>
		public ReadOnlyCollection<T> InUse { get; private set; }
		#endregion

		#region Private
		/// <summary>
		/// The service used to create the objects used inside the ObjectPool.
		/// </summary>
		private readonly ICreationService<T> creationService;

		/// <summary>
		/// The service used to manage the objects inside the pool. This should receive callbacks based on what happens inside the ObjectPool.
		/// </summary>
		private readonly IPoolManagementService<T> poolManagementService;

		/// <summary>
		/// The service used to destruct the objects inside the pool when it's disposed.
		/// </summary>
		private readonly IDestructionService<T> destructionService;

		/// <summary>
		/// The configuration used to manage specific behaviour details for the pool.
		/// </summary>
		private readonly ObjectPoolConfig config;

		/// <summary>
		/// Collection that tracks all the object that are currently pooled and are available to be requested from the pool.
		/// </summary>
		private Stack<T> pooled;

		/// <summary>
		/// Collection that tracks all the objects that have been requested from the pool and are now in use.
		/// </summary>
		private List<T> inUse;
		#endregion
		#endregion

		#region Initialization
		/// <summary>
		/// Creates a new <see cref="ObjectPool{T}"/> with the services and configuration to manage it.
		/// </summary>
		/// <param name="creationService">
		/// The service used to create the objects used inside the ObjectPool.
		/// </param>
		/// <param name="poolManagementService">
		/// The service used to manage the objects inside the pool. This should receive callbacks based on what happens inside the ObjectPool.
		/// </param>
		/// <param name="destructionService">
		/// The service used to destruct the objects inside the pool when it's disposed.
		/// </param>
		/// <param name="config">
		/// The configuration used to manage specific behaviour details for the pool.
		/// </param>
		public ObjectPool(ICreationService<T> creationService,
						  IPoolManagementService<T> poolManagementService,
						  IDestructionService<T> destructionService,
						  ObjectPoolConfig config = null)
		{
			_ = creationService ?? throw new ArgumentNullException(nameof(creationService));
			_ = poolManagementService ?? throw new ArgumentNullException(nameof(poolManagementService));
			_ = destructionService ?? throw new ArgumentNullException(nameof(destructionService));

			config = config ?? new ObjectPoolConfig();

			this.config = config;
			this.pooled = new Stack<T>(this.config.initialCapacity);
			this.inUse = new List<T>(this.config.initialCapacity);

			this.InUse = inUse.AsReadOnly();

			this.creationService = creationService;
			this.poolManagementService = poolManagementService;
			this.destructionService = destructionService;

			for (int i = 0; i < this.config.initialCapacity; i++)
			{
				T @object = creationService.Create();
				pooled.Push(@object);
				poolManagementService.ObjectCreated(@object);
			}
		}
		#endregion

		#region Methods
		#region Public
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public T RequestObject()
		{
			T objectToBeSend;
			if (pooled.Count == 0)
			{
				objectToBeSend = creationService.Create();
				poolManagementService.ObjectCreated(objectToBeSend);
			}
			else
			{
				objectToBeSend = pooled.Pop();
			}

			inUse.Add(objectToBeSend);
			poolManagementService.ObjectRequested(objectToBeSend);
			return objectToBeSend;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="object"><inheritdoc/></param>
		public void ReturnObject(T @object)
		{
			_ = @object ?? throw new ArgumentNullException(nameof(@object));
			int index = inUse.IndexOf(@object);
			if (index == -1)
				throw new ArgumentException("Returned object doesn't belong in this pool.");

			inUse.RemoveAt(index);
			pooled.Push(@object);
			poolManagementService.ObjectReturned(@object);
		}

		/// <summary>
		/// Disposes the object pool and based on the <see cref="config"/> also the pooled and/or in use objects.
		/// </summary>
		public void Dispose()
		{
			if (config.shouldDestroyPooledObjects)
			{
				foreach (T @object in pooled)
				{
					poolManagementService.ObjectDestroyed(@object);
					destructionService.Destroy(@object);
				}
				pooled.Clear();
			}

			if (config.shouldDestroyInUseObjects)
			{
				foreach (T @object in inUse)
				{
					poolManagementService.ObjectDestroyed(@object);
					destructionService.Destroy(@object);
				}
				inUse.Clear();
			}
		}

		/// <summary>
		/// Whether the given <paramref name="object"/> is being pooled inside here.
		/// </summary>
		/// <remarks>If this method returns <c>true</c>, think about releasing control of that objects since it shouldn't be used when it's being pooled.</remarks>
		/// <param name="object">The object to check whether it's inside the pool.</param>
		/// <returns>Whether the given <paramref name="object"/> is being pooled.</returns>
		public bool IsObjectInPool(T @object)
		{
			_ = @object ?? throw new ArgumentNullException(nameof(@object));

			return pooled.Contains(@object);
		}

		/// <summary>
		/// Whether the given object originally belonged to this pool.
		/// </summary>
		/// <param name="object">The object check whether it originally belonged in this pool</param>
		/// <returns>Whether this object originally belonged in this pool.</returns>
		public bool IsObjectInUse(T @object)
		{
			_ = @object ?? throw new ArgumentNullException(nameof(@object));

			return inUse.Contains(@object);
		}
		#endregion
		#endregion
	}
}