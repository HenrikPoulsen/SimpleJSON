# SimpleJSON
A C# json reader/writer which is Unity3D compatible

It is based on the version at http://wiki.unity3d.com/index.php/SimpleJSON with some additional improvements.
Sadly there is no official Github repo for this script so I ended up creating my own, while waiting for an official one to pop up.

The improvements made are mostly a bunch of unit tests (compatible with Unity test runner) which verify that things work like it should, and some performance improvements. The unit tests helped discover some stuff where this implemention did not follow the json standard and they were fixed.
