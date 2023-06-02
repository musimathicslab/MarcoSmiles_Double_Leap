using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Sanford.Multimedia.Midi;
using System;


public class tesi
{


    
    private OutputDevice outD;
    private ChannelMessageBuilder builder;

    public tesi()
    {

    }

    /// <summary>
    /// Permette di inviare un segnale Midi di Note On al dispositivo di output
    /// </summary>
    /// <param name="note">Rappresenta l' indice della  nota da inviare</param>
    /// <param name="octave">Rappresenta il numero dell'ottava </param>
    public void SendEvent(int note, int octave)
    {
        //-----Creazione del Messaggio MIDI da inviare ---
      
        //Data1 rappresenta il primo Data byte
        //in questo caso la nota da "suonare"

        builder.Data1 = note + (octave * 12);

        //Data2 rappresenta il secondo Data byte
        //in questo caso la velocity
        builder.Data2 = 105;

        //Assegno il System Byte
        //Most Significant Bit
        //in questo caso il segnale di NoteOn (1001)
        builder.Command = ChannelCommand.NoteOn;

        //Least Significant Bit
        //MidiChannel rappresente il canale Midi (0-15) a cui inviare il messaggio 
        builder.MidiChannel = 0;

        //Quindi il System Byte inviato sarà il seguente 
        // MSB | LSB
        //1001 | 0000

      
        //assemblo il messaggio
        builder.Build();

        //invio il messaggio al dispositivo di output
        outD.Send(builder.Result);

    }

    /// <summary>
    /// Ricerca il dispositivo di output associato a MarcoSmiles
    /// </summary>
    public void FindMidi()
    {
        int DevId = 0;

        //Ricavo il numero totale di Output Device
        int numDevice = OutputDevice.DeviceCount;
       

        for (int i = 0; i < numDevice; i++)
        {
            //Ricavo il nome del della porta MIDI associata al dispositivo
            MidiOutCaps dev = OutputDevice.GetDeviceCapabilities(i);
           
            //trovo l'ID della porta
            if (dev.name == "MarcoSmiles")
            {
                DevId = i;
               

            }
        }


        //seleziono L'output Device e inizializzo Il MessageBuilder
        outD = new OutputDevice(DevId);
        builder = new ChannelMessageBuilder();

    }


    /// <summary>
    /// Permette di inviare un segnale Midi di Note Off al dispositivo di output
    /// </summary>
    /// <param name="note">Rappresenta l' indice della  nota da "rilasciare"</param>
    /// <param name="octave">Rappresenta il numero dell'ottava </param>

    public void SendMidiOff(int note, int octave)
    {

        //-----Creazione del Messaggio MIDI da inviare---

        //Data1 rappresenta il primo Data byte
        //in questo caso la nota da "rilasciare"
        builder.Data1 = (note + (octave * 12));

        //Data2 rappresenta il secondo Data byte
        //in questo caso la velocity di rilascio
        builder.Data2 = 0;


        //Assegno il System Byte
        //Most Significant Bit
        //in questo caso il segnale di NoteOff (1000)
        builder.Command = ChannelCommand.NoteOff;

        //Least Significant Bit
        //MidiChannel rappresente il canale Midi (0-15) a cui inviare il messaggio 
        builder.MidiChannel = 0;

        //Quindi il System Byte inviato sarà il seguente 
        // MSB | LSB
        //1000 | 0000

        //assemblo il messaggio
        builder.Build();

        //invio il messaggio al dispositivo di output
        outD.Send(builder.Result);

    }

}