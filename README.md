# Hopfion-VR

This will be made into a final build, but the source code is here.

___________________________________
Make local version in Unity
___________________________________

When making a local version of the program with Unity, make sure that you have an editor setup so that the scripts can be opened when working with the source code. I have been using Microsoft Visual Studio which connects with the Unity project if it is set as the Unity editor. (If no editor is set, this will cause problems). I think it is also necessary to have a personal Unity account so as to manage plug-ins (particularly SteamVR)

Begin by creating a new Unity project using the 3D template. Once the project has been created, go to the package manager under the Window tab at the top of the screen. If you have not already downloaded the plug-in, make sure to go to the asset store and download the SteamVR plug-in. At the top of the package manager, use the drop down and select my assets. Select SteamVR plug-in and click import at the bottom right counter of the window. Select import all when prompted.


___________________________________
Functionality
___________________________________

Currently the field being visualized by the arrows and particle trails must be hardcoded into the FieldScript and TestParticleScript. Right now, the hopfion field is given by the vector field given in the supplementary information of the Nature Materials SI for https://www.nature.com/articles/nmat4826

The default simulation scene have a cube with side length of 2 units and arrow resolution of 20. This gives a dense field of vector arrows, but its can be changed in the Unity inspector through the field object.
