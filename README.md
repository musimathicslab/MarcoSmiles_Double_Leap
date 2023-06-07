# MarcoSmiles v5.0 :musical_keyboard: :saxophone:
MarcoSmiles is a project born in 2016 within the Musimathics Laboratory of the University of Salerno that aims to realise new Human-Computer Interaction techniques for musical performances. 

In particular, by using a gesture recognition module to associate hand positions with musical notes, MarcoSmiles actually realises a virtual musical instrument.

<br><br>
*New features compared to version 4.0:*
- *Possibility to use the system (Play phase only) with a Mini PC Windows. This function allows you to use it without the need for a computer, thus guaranteeing portability. At the moment it's possible only to use a single Leap Motion configuration.*
- *Fix bugs*.


## Prerequisites
On the Windows computer:
- Python 3.9.13;
  - numpy 1.24.3;
  - pandas 2.0.2;
  - scikit-learn 1.2.2;
  - matplotlib 3.7.1;
- [LeapMotion SDK v5.12](https://developer.leapmotion.com/tracking-software-download);
- [LoopMidi](https://www.tobias-erichsen.de/software/loopmidi.html). Create a virtual MIDI port with the name `MarcoSmiles` (it's essential to give this name to the port in order to give the possibility to the MIDI management module to find this virtual MIDI port).

You also need to use a Windows Mini PC. If you do not have one already configured, follow the guide in the `MarcoSmilesPortable` folder.
## Use it




## Developed by
[Salerno Daniele](https://github.com/DanieleSalerno)<br>
[Simone Benedetto](https://github.com/BenedettoSimone)
