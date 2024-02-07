# Hopfion-VR

This will be made into a final build, but the source code is here.

User view of Hopfion field with sample tracer particle trajectory:<br><br>
![alt text](https://raw.githubusercontent.com/bday336/Hopfion-VR/main/hopfion_image.png)

___________________________________
Make local version in Unity
___________________________________

When making a local version of the program with Unity, make sure that you have an editor setup so that the scripts can be opened when working with the source code. I have been using Microsoft Visual Studio which connects with the Unity project if it is set as the Unity editor. (If no editor is set, this will cause problems). I think it is also necessary to have a personal Unity account so as to manage plug-ins (particularly SteamVR)

Begin by creating a new Unity project using the 3D template. Download the github repository as a zip file to your local machine. Unzip the local repository files and copy then over to the new Unity project directory. This will install the SteamVR plug-in automatically and configure the controls. It might take some time to process after the files are copied, especially if the project is currently open. Once all the files have been configured the program should be complete functional. Have fun!


___________________________________
Functionality / Controls
___________________________________

Currently the field being visualized by the arrows and particle trails must be hardcoded into the FieldScript and TestParticleScript. Right now, the hopfion field is given by the vector field given in the supplementary information of the Nature Materials SI for https://www.nature.com/articles/nmat4826 (the m field)

The default simulation scene have the vector field represented by a cube with side length of 2 units and arrow resolution of 20 per dimension. This gives a dense field of vector arrows, but its can be changed in the Unity inspector through the field object by adjusting the grid resolution value (you may have to pause the scene, change the value, then restart the scene)

Controls:<br><br>
![alt text](https://raw.githubusercontent.com/bday336/Hopfion-VR/main/Controls%20Manual.png)

For Left Controller:
<br>Button 1 - Currently not assigned<br>
<br>Button 2 - Place object held in hand at current position in the scene<br>
<br>Button 3 - Change object held by controller (currently switches between test particle and nothing, i.e. empty hand)<br>
<br>Button 4 - Activate and deactivate the flow of test particles<br>

Right Controller:
<br>Button 1 - Teleport user to the position of the teleport pointer (if teleport function is active)<br>
<br>Button 2 - Grab object placed in scene<br>
<br>Button 3 - Reset the scene (delete all the test particles present)<br>
<br>Button 4 - Activate and deactivate the teleport function for the user<br>

