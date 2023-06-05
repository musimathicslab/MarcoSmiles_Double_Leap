import mido




#msg=mido.Message('note_on', note=60)
#print(msg.type)
#print(msg.note)
#print(msg.bytes())
#print(mido.get_output_names())
#print(msg.copy(channel=0))



def Send_message_midi(command, note, octave,velocity,midi_channel):
    
        #to print the note on display 
        note_print = ""
        oct_print = ""
        note_names =["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"]



        # C C# D D# E F F# G G# A A# B
        # 0 1  2 3  4 5 6  7 8  9 10 11 ... 23
        # add 12 for each octave
        # MarcoSmiles uses a 2-octave keyboard by numbering the selected notes from 0 to 23
        # also offers the ability to select octaves starting from Range C0-C1
        #C0-C1  C1-C2  C2-C3 ....
        #  0      1      2



        #building midi event
        #calculating midi index note
        msg_note = note + (octave * 12)


        
        msg=mido.Message(command, note=msg_note, velocity=velocity)
        print("MESSAGE SENT")
        print(msg.copy(channel=midi_channel))

        
        note_print = note_names[note % 12]
        if (note > 11):
        
            oct_print = "" + str((octave + 1))
        
        else:
            oct_print = "" + str(octave)
        
        print(note_print + oct_print)

    
if __name__=="__main__":
	mido.set_backend('mido.backends.portmidi')
	print(mido.backend)
	Send_message_midi('note_on',60,0,105,0)



