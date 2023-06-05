import clr
clr.AddReference('dlls/MidiLib')
import time
from MidiLib import MidiClass

Midi = MidiClass()
outdevice = Midi.FindMidi()
for i in range (0,10):

    note_sent=Midi.SendEvent(i,1,outdevice,"on")
    print(note_sent)

    time.sleep(1)

    note = Midi.SendEvent(i,1,outdevice,"off")
    print(note)
