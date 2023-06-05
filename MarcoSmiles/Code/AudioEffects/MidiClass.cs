using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Sanford.Multimedia.Midi;
using System;


public class MidiClass{
    /*
     * Install LoopMidi
     * --------------------------------------------------------------------
     * https://www.tobias-erichsen.de/software/loopmidi.html
     * --------------------------------------------------------------------
     * Create in LoopMidi a new "MarcoSmiles" port
     */
    private OutputDevice outD;
    private ChannelMessageBuilder builder;

    public MidiClass(){}

    public void SendEvent(int note, int octave){
        String Nota = "";
        String Ottava = "";
        String[] NomeNote = new String[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        // C C# D D# E F F# G G# A A# B
        // 0 1  2 3  4 5 6  7 8  9 10 11 ... 23
        // add 12 for each octave
        // MarcoSmiles uses a 2-octave keyboard by numbering the selected notes from 0 to 23
        // also offers the ability to select octaves starting from Range C0-C1
        // C0-C1  C1-C2  C2-C3 ....
        //  0      1      2

        Debug.Log("Note ON" + (note + (octave * 12)));

        // Note
        builder.Data1 = note + (octave * 12);

        // Velocity
        builder.Data2 = 105;

        builder.MidiChannel = 0;

        // Send note
        builder.Command = ChannelCommand.NoteOn;
        builder.Build();
        outD.Send(builder.Result);

        Nota = NomeNote[note % 12];
        if (note > 11){
            Ottava = "" + (octave + 1);
        }else{
            Ottava = "" + octave;
        }
        Debug.Log("\n" + Nota + Ottava);
    }

    public void FindMidi(){
        int DevId = 0;

        int numDevice = OutputDevice.DeviceCount;
        Debug.Log("\n Output Devices Available: " + numDevice);

        for (int i = 0; i < numDevice; i++){
            MidiOutCaps dev = OutputDevice.GetDeviceCapabilities(i);
            Debug.Log("\n" + dev.name);
            if (dev.name == "MarcoSmiles"){
                DevId = i;
                Debug.Log("\nPort MarcoSmiles " + DevId);
            }
        }
        outD = new OutputDevice(DevId);
        builder = new ChannelMessageBuilder();
    }

    public void SendMidiOff(int note, int octave){

        Debug.Log("Note OFF" + (note + (octave * 12)));

        // Note
        builder.Data1 = (note + (octave * 12));

        // Velocity
        builder.Data2 = 105;
        builder.MidiChannel = 0;
        // Send NoteOff
        builder.Command = ChannelCommand.NoteOff;
        builder.Build();

        outD.Send(builder.Result);
    }
}