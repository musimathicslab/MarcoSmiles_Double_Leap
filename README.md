# MarcoSmiles v5.0 :musical_keyboard: :saxophone:
MarcoSmiles is a project born in 2016 within the Musimathics Laboratory of the University of Salerno that aims to realise new Human-Computer Interaction techniques for musical performances.
It offers the ability to fully customize the way a virtual musical instrument is played with only basic hand movements. It uses a Leap Motion Controller to track the user's hand position and a suitably trained artificial neural network (ANN) to link that position to a note in real time.
It also features a MIDI management module that ensures MarcoSmiles can be used with any device with a midi interface.

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



### Export dataset

To export a dataset u need to navigate to the main menu page and press "_export dataset_". At this point select the desired dataset and save it. Then you can use it into MarcoSmiles5.0 or into MarcoSmiles Portable.

#### Import in MarcoSmiles5.0
Navigate to the main scene and select import dataset. Choose the dataset you wish to import and confirm. (_You can check the dataset you are using on the main page_)




#### Import in MarcoSmiles Portable 
Access the utils folder and copy and paste the dataset you want to use. This must be done before using MarcoSmiles Portable so that the dataset can be processed by the system.


## Developed by
[Salerno Daniele](https://github.com/DanieleSalerno)<br>
[Simone Benedetto](https://github.com/BenedettoSimone)
