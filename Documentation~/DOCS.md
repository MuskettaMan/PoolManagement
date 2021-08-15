# Documentation

## Table of Contents

- [Documentation](#documentation)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Package Contents](#package-contents)
  - [Installation Instructions](#installation-instructions)
  - [Limitations](#limitations)
  - [Workflows](#workflows)
    - [Setup the GameObjectPoolBehaviour](#setup-the-gameobjectpoolbehaviour)
    - [Setup the ComponentObjectPoolBehaviour](#setup-the-componentobjectpoolbehaviour)
    - [Setup the PoolableComponentObjectPoolBehaviour](#setup-the-poolablecomponentobjectpoolbehaviour)
    - [Setup the ObjectPool](#setup-the-objectpool)
    - [Create Editor](#create-editor)
      - [Automated](#automated)
      - [Manual](#manual)
  - [Advanced Topics](#advanced-topics)
    - [Object Pool Services](#object-pool-services)
  - [Reference](#reference)
    - [ObjectPool API](#objectpool-api)
      - [ObjectPool Fields](#objectpool-fields)
      - [ObjectPool Methods](#objectpool-methods)
    - [ObjectPoolBehaviour API](#objectpoolbehaviour-api)
      - [ObjectPoolBehaviour Fields](#objectpoolbehaviour-fields)
      - [ObjectPoolBehaviour Methods](#objectpoolbehaviour-methods)
  - [Samples](#samples)

## Overview

This package is an implementation of the [Object Pool](https://en.wikipedia.org/wiki/Object_pool_pattern) design pattern made for Unity. It contains the traditional implementation of the design pattern, but also a more specialized implementation dealing with how GameObjects are instantiated, maintained and destroyed.

## Package Contents

- The `GameObjectPoolBehaviour` is an out-of-the-box component that you put on any GameObject. You can pass this a prefab as the object it should pool and it'll work.
- The `ComponentObjectPoolBehaviour` is the class you want to inherit from when creating your own more specialized Object Pools. You want to derive from this for when you want to pool a specific type of component.
- The `PoolableComponentObjectPoolBehaviour` shares the same functionality as the `ComponentObjectPoolBehaviour`, but this requires the type to also implement the `IPoolable` interface so it can send out callbacks for whenever an object has been returned or requested to/from it's pool.
- This package also contains a feature to deal with the issue that you can't write custom editors for generic classes. By right-clicking the script file that contains a class that derives from `ObjectPoolBehaviour` it will show a context menu item; *Pool Management -> Create Editor Script*. This can be used to create an editor implementation of your Object Pool.

## Installation Instructions

[*This is based on the Unity documentation.*](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

To load a package from a Git URL:

1. Open the Package Manager window by navigating from your menu bar to; *Window -> Package Manager*.
![Menu item screenshot](https://i.imgur.com/OWQSdJc.png)
2. Click the **add** ![Add button](https://i.imgur.com/jaTAW3U.png) button in the status bar.
3. The options for adding packages appear.
![Add package screenshot](https://i.imgur.com/IWPlIsp.png)
4. Select **Add package from git URL** from the add menu. A text box and an **Add** button appear.
5. Enter the Git URL; `https://github.com/MuskettaMan/PoolManagement.git` in the text box and click **Add**.
![Enter Git URL screenshot](https://i.imgur.com/YDyd546.png)

If Unity was able to install the package successfully, the package now appears in the package list with the ![Git tag screenshot](https://i.imgur.com/bI4Uvm4.png) tag.

## Limitations

1. To support the custom editor for classes derived from `ObjectPoolBehaviour`, you need to define this in an explicit editor file. You can either do this manually by creating your own editor file and inheriting from `ObjectPoolBehaviourEditor`, or by making use of the 'Create Editor Script' menu item.
:memo: *Note: it's not required to make use of the custom editor, but it does offer a better experience when using the component.*

## Workflows

### Setup the GameObjectPoolBehaviour

1. Add the `GameObjectPoolBehaviour` to your desired GameObject in the hierarchy.
![Add component screenshot](https://i.imgur.com/Y2XYTA6.png)
2. Add the prefab you want to pool inside the `Prefab` field in the inspector.
![Select prefab screenshot](https://i.imgur.com/QhP3cJ1.png)
3. (Optional) Change the `Pooled Objects Parent` ![Pooled Objects Parent field screenshot](https://i.imgur.com/hIIOjkf.png), more information about this field can be found by hovering over it.
4. (Optional) Tweak the configuration settings. The setting you most likely want to change is the `Initial Capacity`, this is the amount of objects that will be created whenever the ObjectPool becomes active.
![Configuration section screenshot](https://i.imgur.com/XAdJ1hW.png)
5. (Optional) Enable the field `Use Send Messages` ![Use Send Messages screenshot](https://i.imgur.com/41gzYra.png), this determines whether to make use of Unity's message system. With this enabled Components on the GameObjects that are being pooled will get callbacks for when they are requested and returned. To make use of this you add the methods `void Requested() {...}` and `void Returned() {...}`. (This works in a similar manner as the `Awake`, `Start` and `Update` methods.))]
:memo: *Note: it's suggested to not make use of this as this can decrease performance. For a more elegant solution look into the PoolableComponentObjectPoolBehaviour.*

### Setup the ComponentObjectPoolBehaviour

1. Create a new script file inside your project.
![Create c# script screenshot](https://i.imgur.com/XBUJ35W.png)
2. Make your class inherit from the `ComponentObjectPoolBehaviour`;
📝*Don't forget to add the using statement `using Musketta.PoolManagement;`*

```cs
using Musketta.PoolManagement;

public class MyCustomPool : ComponentObjectPoolBehaviour<...>
{

}
```

3. This will also require you to add the generic type of what object will be pooled. The type you define here has to derive from the `Component` class. For this example I will make use of the `Transform` component.

```cs
using Musketta.PoolManagement;
using UnityEngine;

public class MyCustomPool : ComponentObjectPoolBehaviour<Transform>
{

}
```

4. After doing so you have successfully created an Object Pool that will work for the type you've defined in your class. You can now add this script to any GameObject and give a reference to a prefab that contains a component of the type you defined previously.
![Reference prefab screenshot](https://i.imgur.com/0qEwEdf.png)
5. (Optional) I suggest reading the [Setup the GameObjectPoolBehaviour](#setup-the-gameobjectpoolbehaviour) section, for more information about the settings you can tweak in the inspector.
6. (Optional) This package also contains a feature to deal with the issue that you can't write custom editors for generic classes. By right-clicking the script file that contains a class that derives from `ObjectPoolBehaviour` it will show a context menu item; *Pool Management -> Create Editor Script*. This can be used to create an editor implementation of your Object Pool.
![Create editor script screenshot](https://i.imgur.com/T7Fg4kD.png)

### Setup the PoolableComponentObjectPoolBehaviour

1. Create a new script file inside your project.
![Create c# script screenshot](https://i.imgur.com/XBUJ35W.png)
2. Make your class inherit from the `PoolableComponentObjectPoolBehaviour`.

```cs
using Musketta.PoolManagement;

public class MyCustomPool : PoolableComponentObjectPoolBehaviour<...>
{

}
```

3. This will also require you to add the generic type of what object will be pooled. The type you define here has to derive from the `Component` class and will have to implement the `IPoolable` interface.

```cs
public class MyPoolableComponent : MonoBehaviour, IPoolable 
{
    public void Requested() 
    {
        Debug.Log("I've been requested :)!");
    }

    public void Returned() 
    {
        Debug.Log("I've been returned :(!");
    }
}
```

```cs
using Musketta.PoolManagement;

public class MyCustomPool : PoolableComponentObjectPoolBehaviour<MyPoolableComponent>
{

}
```

4. After doing so you have successfully created an Object Pool that will work for the type you've defined in your class. You can now add this script to any GameObject and give a reference to a prefab that contains a component of the type you defined previously.
![Poolable component prefab inspection screenshot](https://i.imgur.com/f7OdKUx.png)
![Reference prefab in object pool screenshot](https://i.imgur.com/82OKNpQ.png)
5. (Optional) I suggest reading the [Setup the GameObjectPoolBehaviour](#setup-the-gameobjectpoolbehaviour) section, for more information about the settings you can tweak in the inspector.
6. (Optional) This package also contains a feature to deal with the issue that you can't write custom editors for generic classes. By right-clicking the script file that contains a class that derives from `ObjectPoolBehaviour` it will show a context menu item; *Pool Management -> Create Editor Script*. This can be used to create an editor implementation of your Object Pool.
![Create editor script screenshot](https://i.imgur.com/T7Fg4kD.png)

### Setup the ObjectPool

The base implementation of the `ObjectPool` works with any type. The main target of this package was to also be able to use this with Unity's components and editors, but it's still possible to use this with normal C# types.
Let's look at an example where we want to make a pool of `StringBuilder` objects.

1. Create an instance of the `ObjectPool` by using such a statement;

```cs
ObjectPool<StringBuilder> stringPool = new ObjectPool<StringBuilder>();
```

2. This won't compile due to the lack of arguments, but it will setup for later. The 3 parameters this constructor expects are a creation service, pool management service and destruction service. More about the specifics of these can be read in [Object Pool Services](#object-pool-services).
3. The most challenging parameter here is the creation service, so let's tackle that right away. The only thing the creation service expects is a class that implements `ICreationService<T>` which has a method `T Create();`. So what we'll do is create a class that implements `ICreationService` with as generic type `StringBuilder`. Let's look at the result;

```cs
public class StringBuilderCreationService : ICreationService<StringBuilder>
{
    public StringBuilder Create()
    {
        return new StringBuilder();
    }
}
```

4. Now we can use that class for our first parameter;

```cs
ObjectPool<StringBuilder> stringPool = new ObjectPool<StringBuilder>(new StringBuilderCreationService());
```

or

```cs
StringBuilderCreationService creationService = new StringBuilderCreationService();
ObjectPool<string> stringPool = new ObjectPool<string>(creationService);
```

5. And now we only have to add the remaining pool management service and destruction service. Luckily these services can be defaulted by doing nothing. The `EmptyPoolManagementService` and `EmptyDestructionService` classes implement the interfaces, but don't do anything. **So if you want some type of clean up to happen to your objects you should create custom implementations of these.** But for this example we can just use the [null objects](https://en.wikipedia.org/wiki/Null_object_pattern);

```cs
StringBuilderCreationService creationService = new StringBuilderCreationService();
EmptyPoolManagementService poolManagementService = new EmptyPoolManagementService();
EmptyDestructionService destructionService = new EmptyDestructionService();
ObjectPool<string> stringPool = new ObjectPool<string>(creationService, poolManagementService, destructionService);
```

6. The final non-required parameter is the config. The config is a class that defines some settings that can change the way the pool behaves, these are the same settings that appear in the Unity inspector for the `ObjectPoolBehaviour`. This is the place to also define with how many objects your Object Pool will initialize. Let's take a look at a final example where we pass a config that will tell the pool to initialize with 500 objects.

```cs
StringBuilderCreationService creationService = new StringBuilderCreationService();
EmptyPoolManagementService poolManagementService = new EmptyPoolManagementService();
EmptyDestructionService destructionService = new EmptyDestructionService();
ObjectPool<StringBuilder>.ObjectPoolConfig config = new ObjectPool<StringBuilder>.ObjectPoolConfig(500);
ObjectPool<string> stringPool = new ObjectPool<string>(creationService, poolManagementService, destructionService, config);
```

### Create Editor

#### Automated

1. Find your Object Pool script inside your project view.
2. Right click on the script file and open the menu item; _Pool Management -> Create Editor Script_.
📓*If this option is greyed out/disabled, make sure the script your inspecting derives from `ObjectPoolBehaviour`.*
3. After pressing that option a new file will be added in the same directory as your Object Pool script. You can now move/edit/change/delete this script.

#### Manual

If the automated feature didn't pan out for you, you can still manually define an editor script.

1. Create a new script in your desired directory.
2. Import the `UnityEditor` and Object Pool editor namespace and the namespace of the object that will be pooled;

```cs
using UnityEditor;
using Musketta.PoolManagement.Editor;
using My.Custom.Namespace;
```

3. Inherit from the `ObjectPoolBehaviourEditor` with as generic the type of object that you will pool;

```cs
public class GameObjectPoolBehaviourEditor : ObjectPoolBehaviourEditor<GameObject> { }
```

4. And finally add the `CustomEditor` attribute above your class with the Object Pool type;

```cs
[CustomEditor(typeof(GameObjectPoolBehaviour))]
public class GameObjectPoolBehaviourEditor : ObjectPoolBehaviourEditor<GameObject> { }
```

## Advanced Topics

### Object Pool Services

The `ObjectPool` makes use of different services, three in total; creation, management and destruction. The purpose of these are as follows;

1. Creation of the object, for most class this can be achieved by making use of its constructor, but in Unity for example you have to make use of the `Initialize()` method. This is also a good place to make use of the [Factory](https://en.wikipedia.org/wiki/Factory_method_pattern) design pattern.
2. Management of the objects, this class will receive callbacks for when objects are created, requested, returned and destroyed, along with the object. This can be used for updating the state of the object from an outside perspective. For example in the `GameObjectPoolManagamentService` whenever a GameObject is returned to the pool it will be set to in active, `SetActive(false)`. But when it's requested it will be set active again.
3. Destruction of the objects, for most standard classes you might not need this, and you can just let `GarbageCollection` do its job. But in cases for objects that implement `IDisposable` interface or require another type of clean up, like Unity GameObjects with the `Destroy()` method, this is the place to perform such destruction, clean up, disposing.

It's recommended to derive from the default service for the Object Pool type you are using; for example the `PoolableComponentObjectPoolBehaviour` has as default for the management service the `PoolableComponentPoolManagementService`. So to further specify your own management you want to derive from this class to still maintain the original behaviour of the management.

## Reference

### ObjectPool API

#### ObjectPool Fields

Name   | Type                   | Description                                                                    | Access
-------|------------------------|--------------------------------------------------------------------------------|-------
Pooled | `ReadOnlyCollection<T>` | All the objects that currently are being pooled inside this class.             | Public
InUse  | `ReadOnlyCollection<T>` | All the objects that currently are in use, but originally belong to this pool. | Public

#### ObjectPool Methods

Name           | Description                                                                             | Parameters  | Return Type | Access
---------------|-----------------------------------------------------------------------------------------|-------------|-------------|-------
RequestObject  | Returns an object from the pool that the user can use.                                  |             | `T`         | Public
ReturnObject   | Returns the given `@object` so it can be used later.                                    | `T` @object | `void`      | Public
Dispose        | Disposes the object pool and based on the config also the pooled and/or in use objects. |             | `void`      | Public
IsObjectInPool | Whether the given `@object` is being pooled inside here.                                | `T` @object | `bool`      | Public
IsObjectInUse  | Whether the given object originally belonged to this pool.                              | `T` @object | `bool`      | Public

### ObjectPoolBehaviour API

#### ObjectPoolBehaviour Fields

Name                  | Type                        | Description                                                                                     | Access
----------------------|-----------------------------|-------------------------------------------------------------------------------------------------|---------------------------
prefab                | `T`                         | The prefab to pool, this will be the original for all the objects that will be inside the pool. | Protected
CreationService       | `ICreationService<T>`       | The service used for creating the `Object`s.                                                    | Protected get, Private set
PoolManagementService | `IPoolManagementService<T>` | The service used for managing the `Object`s.                                                    | Protected get, Private set
DestructionService    | `IDestructionService<T>`    | The service used for destroying the `Object`s.                                                  | Protected get, Private set
ObjectPool            | `ObjectPool<T>`             | The inner `ObjectPool<T>` that's used for managing the object pooling.                          | Public get, Private set

#### ObjectPoolBehaviour Methods

Name                            | Description                                                                                                      | Parameters  | Return Type                 | Access
--------------------------------|------------------------------------------------------------------------------------------------------------------|-------------|-----------------------------|------------------
ReturnObject                    | Returns the given `@object` so it can be reused.                                                                 | `T` @object | `void`                      | Virtual Public
RequestObject                   | Returns an object from the pool so it can be used.                                                               |             | `T`                         | Virtual Public
Awake                           | Initializes the services and creates an instance of the `ObjectPool<T>`.                                         |             | `void`                      | Virtual Protected
OnDestroy                       | Disposes the `ObjectPool` to make sure all the objects that were pooled and in use also get properly clean up.   |             | `void`                      | Virtual Protected
Reset                           | Resets the component to make sure it's in the right state.                                                       |             | `void`                      | Virtual Protected
InitializeCreationService       | Initializes the creation service. If not overridden this will default to `UnityObjectCreationService<T>`.        |             | `ICreationService<T>`       | Virtual Protected
InitializePoolManagementService | Initializes the pool management service. If not overridden this will default to `EmptyPoolManagementService<T>`. |             | `IPoolManagementService<T>` | Virtual Protected
InitializeDestructionService    | Initializes the destruction service. If not overridden this will default to `UnityObjectDestructionService<T>`.  |             | `IDestructionService<T>`    | Virtual Protected

## Samples

The package contains two samples, one for the `ComponentObjectPoolBehaviour` and one for the `PoolableComponentObjectPoolBehaviour`. The process for how these were created can be found in [Workflow - Setup the ComponentObjectPoolBehaviour](#setup-the-componentobjectpoolbehaviour) and [Workflow - Setup the PoolableComponentObjectPoolBehaviour](#setup-the-poolablecomponentobjectpoolbehaviour).
