using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Training process.
/// </summary>
public class TrainingScript : MonoBehaviour{
    /// <summary>
    /// Number of recording for each position.
    /// </summary>
    const int RECORD_COUNT_DEF = 500;

    /// <summary>
    /// Start recording countdown.
    /// </summary>
    const int COUNT_DEF = 3;

    /// <summary>
    /// TextBox countdown.
    /// </summary>
    public Text countDown_Text;

    /// <summary>
    /// TextBox recording.
    /// </summary>
    public Text recording_Text;

    /// <summary>
    /// TextBox show text.
    /// </summary>
    public Text position_Text;

    /// <summary>
    /// Current note ID [0, 23].
    /// </summary>
    [Range(0,23)]
    [SerializeField]
    public int currentNoteId;

    private void Start(){
        recording_Text.text = RECORD_COUNT_DEF.ToString();
    }

    /// <summary>
    ///Event on each frame.
    /// </summary>
    private void FixedUpdate(){}

    /// <summary>
    /// Start recording countdown.
    /// </summary>
    int count = COUNT_DEF;

    /// <summary>
    /// Number of recording for each position.
    /// </summary>
    int record_count = RECORD_COUNT_DEF;

    /// <summary>
    /// Flag set to true if countdown is taking place, false otherwise
    /// </summary>
    bool counting_flag = false;

    /// <summary>
    /// Flag set to true if position recording is taking place, false otherwise.
    /// </summary>
    bool recording_flag = false;

    /// <summary>
    /// Text shown at 'Selection' stage
    /// </summary>
    string text1 = "Choose a position.";

    /// <summary>
    /// Text shown at 'Registration' stage
    /// </summary>
    string text2 = "Hold the position.";


    /// <summary>
    /// Change current note id
    /// </summary>
    /// <param name="note_id"></param>
    public void ChangeNoteId(int note_id){
        currentNoteId = note_id;
    }


    public async Task<bool> RemoveNote(){
        await FileUtils.DeleteRowsNote(currentNoteId);
        return true;
    }



    /// <summary>
    /// Countdown to start position registration.
    /// </summary>
    /// <returns>yield</returns>
    IEnumerator Waiter(){
        if (count > 0){
            count--;
            countDown_Text.text = count.ToString();
            position_Text.text = text1;

            //  sleep (1 second)
            yield return new WaitForSeconds(1);

            //  Repeat the coroutine
            StartCoroutine(Waiter());
        }else{
            //  Countdown ended
            counting_flag = false;

            // Start Coroutine to save positions
            StartCoroutine(WaiterRecording());
        }
    }

   

    /// <summary>
    /// Manages the Coroutine that records the current position.
    /// </summary>
    /// <returns>yield</returns>
    IEnumerator WaiterRecording(){
        if (record_count > 0){
            record_count--;
            recording_Text.text = record_count.ToString();
            position_Text.text = text2;

            //  Sleep (125 ms)
            yield return new WaitForSeconds(0.125f);

            //  Add current position in the list
            DataSelector();

            //  Restart coroutine so save positions
            StartCoroutine(WaiterRecording());
        }else{
            //  Registration ended
            recording_flag = false;

            //  Create the dataset
            FileUtils.Save(_GM.list_posizioni);

            Material TGreen = Resources.Load("TransparentGreen", typeof(Material)) as Material;
            GameObject Button = GameObject.Find("SaveButton");
            Button.GetComponent<MeshRenderer>().material = TGreen;
            Button.GetComponent<Light>().range = 0;
        }
    }


    /// <summary>
    /// Start coroutine for position registration.
    /// </summary>
    public void Trainer(){

        if(!counting_flag){
            count = COUNT_DEF + 1;
            record_count = RECORD_COUNT_DEF;

            StartCoroutine(Waiter());
            counting_flag = true;
        }
    }

    /// <summary>
    /// Adds the current position to the list of positions
    /// </summary>
    private void DataSelector(){
        // Left Hand Device 1
        var left_hand1 = new DataToStore(
            _GM.hand_L,
            DatasetHandler.getFF(_GM.hand_L.Fingers[0], true),
            DatasetHandler.getFF(_GM.hand_L.Fingers[1]),
            DatasetHandler.getFF(_GM.hand_L.Fingers[2]),
            DatasetHandler.getFF(_GM.hand_L.Fingers[3]),
            DatasetHandler.getFF(_GM.hand_L.Fingers[4]),

            DatasetHandler.getNFA(_GM.hand_L.Fingers[0], _GM.hand_L.Fingers[1]),
            DatasetHandler.getNFA(_GM.hand_L.Fingers[1], _GM.hand_L.Fingers[2]),
            DatasetHandler.getNFA(_GM.hand_L.Fingers[2], _GM.hand_L.Fingers[3]),
            DatasetHandler.getNFA(_GM.hand_L.Fingers[3], _GM.hand_L.Fingers[4]));

        // Right Hand Device 1
        var right_hand1 = new DataToStore(
            _GM.hand_R,
            DatasetHandler.getFF(_GM.hand_R.Fingers[0], true),
            DatasetHandler.getFF(_GM.hand_R.Fingers[1]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[2]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[3]),
            DatasetHandler.getFF(_GM.hand_R.Fingers[4]),

            DatasetHandler.getNFA(_GM.hand_R.Fingers[0], _GM.hand_R.Fingers[1]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[1], _GM.hand_R.Fingers[2]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[2], _GM.hand_R.Fingers[3]),
            DatasetHandler.getNFA(_GM.hand_R.Fingers[3], _GM.hand_R.Fingers[4]));

        // Left Hand Device 2
        var left_hand2 = new DataToStore(
            _GM.secondDeviceHand_L,
            DatasetHandler.getFF(_GM.secondDeviceHand_L.Fingers[0], true),
            DatasetHandler.getFF(_GM.secondDeviceHand_L.Fingers[1]),
            DatasetHandler.getFF(_GM.secondDeviceHand_L.Fingers[2]),
            DatasetHandler.getFF(_GM.secondDeviceHand_L.Fingers[3]),
            DatasetHandler.getFF(_GM.secondDeviceHand_L.Fingers[4]),

            DatasetHandler.getNFA(_GM.secondDeviceHand_L.Fingers[0], _GM.secondDeviceHand_L.Fingers[1]),
            DatasetHandler.getNFA(_GM.secondDeviceHand_L.Fingers[1], _GM.secondDeviceHand_L.Fingers[2]),
            DatasetHandler.getNFA(_GM.secondDeviceHand_L.Fingers[2], _GM.secondDeviceHand_L.Fingers[3]),
            DatasetHandler.getNFA(_GM.secondDeviceHand_L.Fingers[3], _GM.secondDeviceHand_L.Fingers[4]));

        // Right Hand Device 2
        var right_hand2 = new DataToStore(
            _GM.secondDeviceHand_R,
            DatasetHandler.getFF(_GM.secondDeviceHand_R.Fingers[0], true),
            DatasetHandler.getFF(_GM.secondDeviceHand_R.Fingers[1]),
            DatasetHandler.getFF(_GM.secondDeviceHand_R.Fingers[2]),
            DatasetHandler.getFF(_GM.secondDeviceHand_R.Fingers[3]),
            DatasetHandler.getFF(_GM.secondDeviceHand_R.Fingers[4]),

            DatasetHandler.getNFA(_GM.secondDeviceHand_R.Fingers[0], _GM.secondDeviceHand_R.Fingers[1]),
            DatasetHandler.getNFA(_GM.secondDeviceHand_R.Fingers[1], _GM.secondDeviceHand_R.Fingers[2]),
            DatasetHandler.getNFA(_GM.secondDeviceHand_R.Fingers[2], _GM.secondDeviceHand_R.Fingers[3]),
            DatasetHandler.getNFA(_GM.secondDeviceHand_R.Fingers[3], _GM.secondDeviceHand_R.Fingers[4]));

        _GM.list_posizioni.Add(new Position(left_hand: left_hand1, right_hand: right_hand1, left_hand2: left_hand2, right_hand2: right_hand2, id: currentNoteId));
    }
}
