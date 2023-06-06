import clr
clr.AddReference('dlls/MidiLib')
import time
from MidiLib import MidiClass

Midi = MidiClass()
out_device = Midi.FindMidi()
for i in range (0,10):

    note_sent=Midi.SendEvent(i,1,out_device,"on")
    print(note_sent)

    time.sleep(1)

    note = Midi.SendEvent(i,1,out_device,"off")
    print(note)
