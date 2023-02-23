# Unity-Development-Toolkit
<img src="https://pbs.twimg.com/media/FpoRpnGX0AEcgS-?format=png&name=small"  style="width:100%; height:128px; object-fit:fill;">
A Toolkit made to simplify Unity Development, without limiting creative control or producing extra performance/development overhead. 

The Unity Development Toolkit (UDT) is a C# library that is designed with an "interface first" approach. This approach focuses on creating a flexible and efficient interface for developers to interact with their Unity projects. The UDT provides various tools to achieve this objective, including runtime singletons, system singletons, standard objects, standard components, and controllables.

## - Runtime Singletons

The UDT's runtime singletons are designed to provide quick control over your game's state. These singletons are built by extending the Runtime class, which uses state classes to determine the current state of the runtime. The first state is the default state, and to move to another state, you can call SetState(new state). Runtimes are singletons that are created when the Unity game starts.

## - System Singletons

The UDT's system singletons are built by extending the System<T> class, where T is the class name of the system. Systems act as a bridge for certain types of objects and components. You can start a system by calling System<T>.StartSystem(params), where "params" can include objects or components for the system to manage. Additionally, components can add themselves upon creation, as is the case for the default systems.

## - Standard Objects and Components

The UDT's standard objects and components provide a layer on top of Unity's default game object system. These objects and components are designed to pool objects by default and provide many features for picking, instantiating, communicating with, and managing the lifecycles of objects more easily.

Standard objects and components have many features that can be used in conjunction with systems, allowing gameplay systems to be built in the simplest way possible.

## - Controllables

The UDT's controllables add the functionality of controller/entity-style input management to the New Input System. This tool follows a similar approach to Unreal's player/AI controllers. It also contains a serializable input system that reads input action maps and tracks input status in order to serialize it in a form that can be used for netcode and machine learning agents.

Controllable components provide a layer on top of standard components that enable possession and control from controllers by default.

## Conclusion

The Unity Development Toolkit (UDT) is a powerful C# library that provides a range of tools for Unity developers. Its "interface first" approach focuses on creating flexible and efficient interfaces for developers to interact with their Unity projects. The UDT's tools, including runtime and system singletons, standard objects and components, and controllables, allow for simpler and more efficient development of gameplay systems. With the UDT, developers can build projects without the performance or development overhead that competing paid assets provide.
