import clr
clr.AddReference("dlls/MidiUtils")
import time
from MidiUtils import MidiClass

Midi = MidiClass()
outdevice = Midi.FindMidi()
for i in range (0,10):

    note_sent=Midi.SendEvent(i,1,outdevice)
    print(note_sent)

    time.sleep(1)

    note = Midi.SendMidiOff(i,1,outdevice)
    print(note)
