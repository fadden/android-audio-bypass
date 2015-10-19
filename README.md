Android Audio Bypass
====================

This is a quick demo project that illustrates the use of an Android plugin
to avoid the hideous latency added by the Unity 3D game engine on some
devices.  The difference is easily noticeable in
[this demo video](https://www.youtube.com/watch?v=E_VwFBxf4ew), recorded
on a Moto X running Android 4.4.4.

On a Nexus 10, and reportedly on old and new Nexus 7s, there's no observable
difference between the Unity sound player and the plugin-based player.  So
while Unity is the proximate cause of latency, it varies between devices, and
the blame might be shared by more than one party.

There are two parts, the plugin and a Unity 5 test project.  (The Plugin
should work fine with Unity 4.6, but I haven't tried it.)

**NOTE:** this is not a finished product.  It's a bare-bones demo intended to
highlight the latency discrepancy between Unity's AudioSource and Android's
SoundPool.  I'm reluctant to put too much effort into this because I'm
hoping that Unity will figure out how to make sound work better across
all Android devices.

#### About the Plugin ####

The plugin is a standard Android library project.  The easiest way to build
it is by running `ant jar` from the command line, then copying the plugin
jar (`AndroidBypassPlugin.jar`) into the Unity project.  See the `mk.bat`
file for a sample build script.

It's not strictly necessary to have a plugin.  You can do everything
necessary using the
[AndroidJNI](http://docs.unity3d.com/ScriptReference/AndroidJNI.html)
methods in Unity.  Writing the SoundPool interface code in the Java programming
language makes the code easier to understand and maintain however.

#### Test Project ####

The Unity project includes a pair of generated WAV files in the StreamingAssets
directory, which makes them easily accessible to both Unity and the Android
AssetManager.  UI buttons allow you to play either sound with either method
and observe the difference.

**NOTE:** a behavior change in Unity 5, specifically noted in 5.1.3, breaks
the demo.  The Unity editor no longer recognizes WAV files in the
StreamingAssets folder as import sources for AudioClip.  As a result, when
you check out the project the Unity AudioSource objects will have broken
references to the sound files.  You can work around the issue by using
"import asset" to import the WAV files to a different folder, and then drag
them into StreamingAssets.

(It's not clear if this change was unintentional, intentional but poorly
considered, or intentional with good reason.  I filed bug 737168 on
2015/10/18 to try to get this sorted.)

### Use of this code ###

This code is distributed under the Apache 2 license (the same one used by
the Android Open Source Project).  That means you can use the code in a
commercial product without any viral-licensing concerns.
