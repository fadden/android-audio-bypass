Android Audio Bypass
====================

This is a quick demo project that illustrates the use of an Android plugin
to avoid the hideous latency added by the Unity 3D game engine on some
devices.

There are two parts, the plugin and a Unity 5 test project.  (The Plugin
should work fine with Unity 4.6, but I haven't tried it.)

The plugin is a standard Android library project.  The easiest way to build
it is by running `ant jar` from the command line, then copying the plugin
jar (`AndroidBypassPlugin.jar`) into the Unity project.  See the `mk.bat`
file for a sample build script.

The Unity project includes a pair of generated WAV files in the StreamingAssets
directory, which makes them easily accessible to both Unity and the Android
AssetManager.  UI buttons allow you to play either sound with either method
and observe the difference.

On a Moto X running 4.4.4, there's a significant difference in latency, as
demonstrated in this video [need youtube link].  On a Nexus 10, there's
no observable difference.  So while Unity is the proximate cause of latency,
it varies between devices, and the blame may be shared.


### Use of this code ###

This code is distributed under the Apache 2 license.

This is not a finished product.  It's a bare-bones demo intended to
highlight the latency discrepancy between Unity's AudioSource and Android's
SoundPool.  I'm reluctant to put too much effort into this because I'm
hoping that Unity will figure out how to make sound work better across
all Android devices.

