# MarcoSmiles v5.0 :musical_keyboard: :saxophone:
MarcoSmiles is a project born in 2016 within the Laboratory of Musimatics at the University of Salerno that aims to realise new Human-Computer Interaction techniques for musical performances.
It offers the possibility to completely customise the way a virtual musical instrument is played with only basic hand movements. It uses two Leap Motion Controller to track the position of the user's hands and a suitably trained artificial neural network (ANN) to link that position to a note in real time.
It also features a MIDI management module that ensures MarcoSmiles can be used with any device with a midi interface.
<br><br>
*New features compared to version 4.0:*
- *Possibility to use the system (Play phase only) with a Mini PC Windows using MarcoSmiles Portable. This function allows you to use it without the need for a computer, thus guaranteeing portability. At the moment it's possible only to use a single Leap Motion configuration;*
- *Possibility to train the system (on MarcoSmiles5.0) with two Leap Motion Controller and simultaneously train the system for one Leap Motion Controller to use it with MarcoSmiles Portable*;
- *Fix bugs*.


## How it works
MarcoSmiles 5.0 is a desktop application with a graphical user interface. Make sure you have connected Leap Motions before starting the system.

### Main Scene
<p align="center"><img src="readme_images/main_scene.png"/></p>
The Main Scene is the home page of the system. At the top is the logo of MarcoSmiles and below it the various buttons that allow you to navigate the different screens of the system. It will not be possible to access the Play Scene (Play Scene) without having previously trained the system.

at the top right there is a button that allows access to the dataset management scene.

### Dataset scene
<p align="center"><img src="readme_images/dataset_scene.png"/></p>

The Dataset scene deals with the management of datasets and is presented through a scrollmenu that contains clickable cards. Each card is associated with a dataset present in the application and contains all the information relating to the dataset and its configurations. Furthermore, there are two buttons on each card that allow you to modify and delete its contents. By clicking on the card, you can select the dataset.

The scene also includes three other buttons: one at the top right for adding a new dataset, and two buttons at the bottom that deal with importing and exporting datasets.

- To export a dataset you need to press "_export dataset_" button. At this point select the desired dataset and save it. Then you can use it into MarcoSmiles5.0 or into MarcoSmiles Portable;
  - To import the configuration in MarcoSmiles portable you need to copy and paste the exported dataset you want to use. This must be done before using MarcoSmiles Portable so that the dataset can be processed by the system;
- To import the configuration in MarcoSmiles5.0 select "_import dataset_" button. Choose the dataset you wish to import and confirm. (_You can check the dataset you are using on the main page_).

### Training Scene
<p align="center"><img src="readme_images/training_scene.png"/></p>

The Training scene is dedicated to training the system in which the user can associate each note with the hand configuration they most desire.

The middle section is dedicated to the projection of the hands in real
time detected by the Leap Motion Controllers. 

On the right side of the screen there is a button with a semiquaver pause icon whose function
is to train the system to "play" a pause. 

On the left instead is
present the info button, which, when clicked, shows instructions
for training the system. 

At the bottom of the GUI there is
a virtual piano, consisting of two octaves, which allows you to select the note to be trained. The user, through a click on the key of the
piano, decides to train the system on that note which will assume a blue color, so as to visually communicate the choice made. To the right
of the piano there are two different colored buttons:
- the green one starts recording the hand configuration;
- the red one deletes the last configuration acquired;

To the left of the piano there is a button that allows you to return to the Main Scene.

Centrally in the top of the interface is a blue button that has the function of starting the training.


The training process consists of several steps:
1. Select, on the virtual piano, the note that you wish to train (or pause with the corresponding button);

2. Press the green button, which starts a 3 second countdown in which you must assume the configuration of the hands you want to associate with the selected note;
3. At the end of the countdown, the system will start the actual recording of the position of the hands; in this phase 500 measurements will be taken, for a total of about 3 minutes of training (you will be able to see the progress of this operation in the upper right corner, where the relevant counter is);
4. When the process is finished, the note will turn green to indicate that training has been carried out for the latter.
   
It is possible to perform multiple trainings for the same note.
In case you are not satisfied with the newly registered configuration you can delete it using the red button. 
When you have registered the various desired positions, press the training button, thus proceeding with the creation of a configuration of the newly trained system. 
This operation will execute the Machine Learning model, which will take time proportional to the computational resources of the machine on which it is executed and the size of the dataset. At the end of the training, the system will allow access to the execution scene (Play Scene) and thus use the configuration just created. Finally, at the top, the outcome of the learning process will be indicated with its completion time.

### Play Scene
<p align="center"><img src="readme_images/play_scene.png"/></p>
You can watch a system execution here: https://drive.google.com/file/d/16SmEtO0M_kDU3Nr-sK38A6QsHcirsray/view?usp=sharing

The Play Scene enables musical performances using the selected system configuration. 
As in the Training Scene, it is possible to view the user's hands in real-time.

At the bottom of the scene there is a virtual piano, the keys of which are highlighted in blue and their pressure is simulated when the user assumes a hand configuration that the system can recognize. There are two buttons that allow the user to vary the range of octaves. The default option uses octaves C3-C4, and the buttons allow increasing or decreasing that range, respectively.

There is also a button to enable/disable the MIDI feature, which, when activated, will mute the synthesizer and the button will emit a blue light. Next to the piano there is an additional button that allows you to activate/deactivate a "hologram" which allows you to visualize the notes that are being played.

Next to the window in which the hands are displayed is the section dedicated to controlling the parameters of the Virtual Synth, which has the characteristic of being totally customizable, and thus allows the timbre of the sound to be changed and modulations, filters and effects to be added. Thanks to a graphical interface based on Knobs, which attempt to simulate the potentiometers of a real synthesizer, the you will be able to vary the parameters of the Synth.

<p align="center"><img src="readme_images/synth.png"/></p>

