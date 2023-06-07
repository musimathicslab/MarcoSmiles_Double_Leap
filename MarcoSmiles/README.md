# Installation on Windows

*N.B. This section is for developers only!*

## Prerequisites

- Python 3.9.13;
  - numpy 1.24.3;
  - pandas 2.0.2;
  - scikit-learn 1.2.2;
  - matplotlib 3.7.1;
- [LeapMotion SDK v5.12](https://developer.leapmotion.com/tracking-software-download);
- [LoopMidi](https://www.tobias-erichsen.de/software/loopmidi.html). Create a virtual MIDI port with the name `MarcoSmiles` (it's essential to give this name to the port in order to give the possibility to the MIDI management module to find this virtual MIDI port);
- [Unity Engine 2021.3.15f1](https://unity.com/download);
- Visual Studio with the "Game development with Unity" package (during UnityEngine installation).

## Installation

1. Download the project placed into the `MarcoSmiles` folder.
2. Open Unity Hub and create an empty project called `MarcoSmiles5.0`.
3. Download the unitypackage from this link and import it in the new project. You can do that in `Assets -> Import Package -> Custom Package`.
4. In `Window -> Package Manager` select the `Packages: Unity Registry` and install `Magic Leap XR Plugin`.
5. In `Edit -> Project Settings` go to `Project -> Package Manager` insert the following information in the `Scoped Registries` section:
   1. Name: "Ultraleap";
   2. URL: "https://package.openupm.com";
   3. Scope(s): "com.ultraleap".
6. Click `Save`. In `Window -> Package Manager` select the `Packages: My Registries` and install `Ultraleap Tracking`.
7. Go into the `Project` section, and find the folder `Own/Scenes` folder. Select the `MainPage` scene, go into `File -> Build settings` and click `Add Open Scenes`. Repeat this process for each scene in this order:
   1. PlayScene;
   2. TrainingScene;
   3. TestingScene. <br>
   After this step you will have the following result.<br><br><p align="center"><img src="../readme_images/build_settings.png"/></p>
8. Change the resolution to `1280x960`.


    