# Installation on Mini PC Windows

*N.B. This section is for developers only!*

- Python 3.7.9;
  - numpy 1.18.0;
  - pandas 1.1.4;
  - [pythonnet](https://github.com/pythonnet/pythonnet).
- [LoopMidi](https://www.tobias-erichsen.de/software/loopmidi.html). Create a virtual MIDI port with the name `MarcoSmiles` (it's essential to give this name to the port in order to give the possibility to the MIDI management module to find this virtual MIDI port).

## How to use Leap Developer Kit 2.3.1+31549 Windows

_References: https://support.leapmotion.com/hc/en-us/articles/360004362237-Generating-a-Python-3-3-0-Wrapper-with-SWIG-2-0-9_

1. Download the kit from this [link](https://api.leapmotion.com/v2?id=skeletal-beta&platform=windows&version=2.3.1.31549).
2. Install the `.exe` file in the folder.
3. Since the LeapSDK does not currently support Python3, we have to compile the library with our version of Python. _N.B It only works with the version of Python used during compilation._
To do that execute the following steps, otherwise if you want to use python 3.7 go directly to step 10 and download those files from the `MarcoSmilesPortable` folder.
   1. Install Visual Studio 2019 (with Desktop development with C++ workload) and [Swig 3.0.3](https://www.swig.org/download.html). To work with swig create a Path system variable. 
   2. Create an empty C++ project. Copy `Leap.h`, `LeapMath.h`, `Leap.i`, and `Leap.lib` (x64) into this project folder.
   3. Run SWIG from that folder to generate `LeapPython.cpp`.
   ```
   swig -c++ -python -o LeapPython.cpp -interface LeapPython Leap.i
   ```
   4. Open project properties, select Release configuration and set `x64` as target. Go to the `Configuration Properties -> General page`. From there, set the `Target Name` to "LeapPython" and set the `Configuration Type` to "Dynamic Library (.dll)".<br><br><p align="center"><img src="../readme_images/sdk_python3_1.png"/></p>
   5. Go to the `C/C++ -> General` property page. Add the path containing `Python.h`, typically C:\path\Python37\include.<br><br><p align="center"><img src="../readme_images/sdk_python3_2.png"/></p>
   6. Go to the `Linker -> Input` property page. Add `Leap.lib` path and the full path to `python37.lib`, typically C:\path\Python37\libs\python37.lib.<br><br><p align="center"><img src="../readme_images/sdk_python3_3.png"/></p>
   7. Go to `C/C++ -> Preprocessor` property page and add `_CRT_SECURE_NO_WARNINGS` to preprocessor definitions.<br><br><p align="center"><img src="../readme_images/sdk_python3_4.png"/></p>
   8. Build the project with a `x64` configuration.<br><br><p align="center"><img src="../readme_images/sdk_python3_5.png"/></p>
   9. Rename the output `LeapPython.dll` to `LeapPython.pyd`.
   10. Finally, you can copy the files `LeapPython.pyd`, `Leap.py` and `Leap.dll` in a new folder to use them in any project.

   
## Create/Modify DLL for MidiLib
This section is useful for creating a customised Midi library. If you do not want to modify the version we created, you can find the result of the next steps in `MarcoSmilesPortable/ddls`.
   
1. Install Visual Studio.
2. Create a new project of type `Class Library(.NET Standard)`.
3. Copy and paste the content of the file `Midi_Library_File.cs` placed in `MarcoSmilesPortable/dlls`.
4. Build the project and locate the dll created (the path can be found in the output console).
5. Copy the `MidiLib.dll` in the `MarcoSmilesPortable/dlls` folder. 
6. You can use it in a Python script in this way.
     ```
   import clr
   clr.AddReference('dlls/MidiLib')
   from MidiLib import MidiClass
   ```  
   Where MidiLib is the name of the dll file (_and of the namespace_) and MidiClass is the name of the class that you want to import.
7. At this point you can instantiate an object of that class in this way and access to all the public methods.
   ```    
   Midi = MidiClass()
   ```
   MidiClass offer two methods:
   1. `FindMidi()` that allow to retrieve the MIDI port of MarcoSmiles and return an object of the type `OutputDevice`.
   2. `SendEvent(int note,int octave,OutputDevice outDev, string command)` that allow to send note_on/note_off message on a MIDI output port:
      1. note --> 0 to 24 (_represent the key pressed on a piano keyboard of 24 keys_) 
      2. octave --> 0 to 9 (_represent the starting octave_)
      3. outDev --> (_represent the midi output device to which you want to send the message_)
      4. command --> on/off (_represent the command of note_on/note_off_)

   In order to use this class you need to create a virtual port named `MarcoSmiles` on LoopMidi software.
   You can check if it is working correctly using a DAW (for example [FLStudio](https://www.image-line.com/)), selecting the MarcoSmiles MIDI port and running the script Dlltest.py.


## Use MarcoSmiles Portable
1. Download the portable version of MarcoSmiles placed in `MarcoSmilesPortable`.
2. Go into your terminal and execute the following instructions:
   ```
   cd MarcoSmilesPortable
   ```
   ```
   python LeapEventListener.py
   ```