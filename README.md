# Pool Management

###### Author: Ferri de Lange

## Description

This package is an implementation of the [Object Pool](https://en.wikipedia.org/wiki/Object_pool_pattern) design pattern made for Unity. It contains the traditional implementation of the design pattern, but also a more specialized implementation dealing with how GameObjects are instantiated, maintained and destroyed.

## Getting Started

The Getting Started will be divided in 3 parts, each part contains a way to immediately start using the package, but going from less to more setup required.

*If this Getting Started section doesn't cover it, please look at the [documentation](Documentation~/DOCS.md). It's more extensive and gives more information.*

### GameObject Pool

The `GameObjectPoolBehaviour` is the quickest way to get started, it's the only concrete implementation of the pattern in here due to it having the most flexable usage. To start using it you can place the `GameObjectPoolBehaviour` component on a GameObject of your choosing.

### Component Object Pool Behaviour

The `ComponentObjectPoolBehaviour` is an abstract class that requires you to first inherit from it and define the type of object you want to pool. You also have the ability to override some methods to further customize the way your Object Pool behaves.
This is done mainly by overriding the methods; `InitializeCreationService()`, `InitializePoolManagementService()` and `InitializeDestructionService()`. These are the methods that initialize a certain 'service' that handles that way objects are created, destructed and managed for your Object Pool. More on this [here](https://github.com/MuskettaMan/PoolManagement#services).

### Poolable Component Object Pool Behaviour

This type of Object Pool gives you the added benefit that objects that are being pooled are getting callbacks on when they are being requested and returned from and to the pool. This same behaviour appears when the `Use Send Messages` is enabled, but this feature makes use of the Unity `SendMessage()` method which a performance heavy call. The `PoolableObjectPoolBehaviour` solves this problem by constraining its generic type to also have to implement the `IPoolable` interface. By doing so every type have to implement two methods; `Requested()` and `Returned()`. These methods will be called by the Object Pool whenever they are being requested or returned by the user. The downside of this pool type is that you can't use 'out-of-the-box' objects that don't implement this interface, e.g. `Transform`, `GameObject`, etc.

## Inspector

The base `ObjectPoolBehaviour` contains all the serialized settings necessary for setting up an Object Pool. The first setting that you'll notice on the component is the `Prefab` field. This field requires a GameObject reference, here you can place the prefab that you want to 'pool'. The second setting there is the `Pooled Objects Parent`, this requires a transform reference that will be used to parent under all the instantiated prefabs.
After this you have the `Objectpool Configuration` settings, these are some specialized settings that can tweak the way the Object Pool can behave. The first, and most important, is the `Initial Capacity` field. Here you can enter the amount of objects you want the Object Pool to 'prewarm' with. This means that these are the amount of objects that the Object Pool will create as soon as it starts existing. After this you have the `Should Destroy In Use` and `Should Destroy Pooled`, these are settings that determine what should happen to the objects managed by the Object Pool when it's destroyed. The former meaning whether it should destroy objects that are in use by 'clients' of the Object Pool and the latter meaning whether it should destroy objects that are loaded in by the Object Pool.

## Custom Editor

The custom editor for `ObjectPoolBehaviour` makes it possible to view the objects that are being pooled and are being used from the inspector. Here you can select the objects to find them in the hierarchy, and you can use another feature to force return pooled objects back to the pool by pressing the arrow button next to the pooled object that's in use.

The downside here is the face that this is a generic and abstract class and this editor doesn't automatically apply for any subclasses. But since creating an editor for a derived class is quite standard there is a feature that eases this repetitive task. By opening the context menu (pressing right mouse button) on the script file inside your project window, you'll find the option 'PoolManagement -> Create Editor Script'. This option will not be pressable if the class defined in here doesn't derive from `ObjectPoolBehaviour`. By pressing this option a new script file will be created in the same directory defining the editor code, after this is created you can move this script to any other directory for organizing or assembly reasons.

## Services

The `ObjectPool` makes use of different services, three in total; creation, management and destruction. The purpose of these are as follows;

1. Creation of the object, for most class this can be achieved by making use of its constructor, but in Unity for example you have to make use of the `Initialize()` method. This is also a good place to make use of the [Factory](https://en.wikipedia.org/wiki/Factory_method_pattern) design pattern.
2. Management of the objects, this class will receive callbacks for when objects are created, requested, returned and destroyed, along with the object. This can be used for updating the state of the object from an outside perspective. For example in the `GameObjectPoolManagamentService` whenever a GameObject is returned to the pool it will be set to in active, `SetActive(false)`. But when it's requested it will be set active again.
3. Destruction of the objects, for most standard classes you might not need this, and you can just let `GarbageCollection` do its job. But in cases for objects that implement `IDisposable` interface or require another type of clean up, like Unity GameObjects with the `Destroy()` method, this is the place to perform such destruction, clean up, disposing.

It's recommended to derive from the default service for the Object Pool type you are using; for example the `PoolableComponentObjectPoolBehaviour` has as default for the management service the `PoolableComponentPoolManagementService`. So to further specify your own management you want to derive from this class to still maintain the original behaviour of the management. Of course this isn't necessary, and you're free to use this package however you want!

Have fun Pooling! :D