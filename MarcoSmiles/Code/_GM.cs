using Leap;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;

/// <summary>
/// Game Master that contains the state of the application.
/// </summary>
public class _GM : MonoBehaviour{

    TrainingScript ThatTrainer;
    public int previousclickedKey = 0;
    public int k = 1;
    public int counter = 0;
    public int[] Mylabel = new int[25];

    /// <summary>
    /// Flag LeapMotion device connected.
    /// </summary>
    public static bool IsLeapConnected = false;

    /// <summary>
    /// Button "PlayScene".
    /// </summary>
    private Button playButton;

    /// <summary>
    /// Class instance used during training scene.
    /// </summary>
    public TrainingScript trainer;

    /// <summary>
    /// Right hand first device.
    /// </summary>
    public static Hand hand_R;

    /// <summary>
    /// Left hand first device.
    /// </summary>
    public static Hand hand_L;

    /// <summary>
    /// Right hand second device.
    /// </summary>
    public static Hand secondDeviceHand_R;

    /// <summary>
    /// Left hand second device.
    /// </summary>
    public static Hand secondDeviceHand_L;

    /// <summary>
    /// Class instance used during ttesting scene.
    /// </summary>
    public ConfusionTestingScript tester;

    /// <summary>
    /// Used in training scene to save the dataset.
    /// </summary>
    public static List<Position> list_posizioni;

    /// <summary>
    /// Used to contain the trained notes (0-23)
    /// </summary>
    public static List<int> trainedNotes;

    /// <summary>
    /// true if there are hands, otherwise false. the script ProceduralAudioOscillator.cs looks at this variable to decide whether it should play
    /// </summary>
    public static bool isActive = false;

    /// <summary>
    ///
    /// </summary>
    public static float[] current_Features;

    /// <summary>
    /// Index current note, read by PCMOscillator.
    /// </summary>
    public static int indexPlayingNote;

    /// <summary>
    /// Index previous note.
    /// </summary>
    public static int indexPreviousNote;

    /// <summary>
    /// Popup panel.
    /// </summary>
    public GameObject PopupPanel;

    /// <summary>
    /// Leap connection popup.
    /// </summary>
    public static GameObject ConnectLeapPanel;

    /// Interface. Learn on selected configuration
    /// <summary>
    /// GameObject containing image indicating that Learn was made for the selected conf. (TRAINING SCENE)
    /// </summary>
    public GameObject ConfLearn;

    /// <summary>
    /// GameObject containing image indicating that Learn was not made for the selected conf. (TRAINING SCENE)
    /// </summary>
    public GameObject ConfNotLearn;

    /// <summary>
    /// GameObject containing the date of the last Learn for the selected conf. (TRAINING SCENE)
    /// </summary>
    public GameObject DateLatestLearning;

    //  Loading circle animation.
    /// <summary>
    /// GameObject loading animation.
    /// </summary>
    public GameObject LoadingCircle;

    /// <summary>
    /// Animation icon.
    /// </summary>
    public RectTransform mainIcon;

    /// <summary>
    /// Time slice loading animation.
    /// </summary>
    public static float timeStep = 0.1f;

    /// <summary>
    /// Rotation angle for loading animation.
    /// </summary>
    public static float oneStepAngle = -36;
    float startTime;

    /// <summary>
    /// Configuration selected.
    /// </summary>    
    public Text SelectedDatasetText;

    /// <summary>
    /// Unity scenes.
    /// </summary>
    private enum SceneEnum{
        MainPage,
        PlayScene,
        TrainingScene,
        TestingScene
    }

    /// <summary>
    /// Current scene.
    /// </summary>
    private SceneEnum currSceneEnum;

    /// <summary>
    /// Scene object to modify current scene.
    /// </summary>
    private Scene currentScene;
    private GameObject pauseButton;
    private AsyncOperation loadingOperation0;
    


    #region UNITY METH

    /// <summary>
    /// Called when the GM object is initialised.
    /// </summary>
    private void Awake(){
        pauseButton = GameObject.Find("PauseButton");
        trainer = new TrainingScript();

        /*
         * In Unity, only certain file types can be loaded into the build. .txt files are copied into the folder of the build.
         * This way we can have the .py script (which is currently a .txt file) inside the build.
         * So, we read the .txt file from the Resources folder, and using the SavePy method, we save the script read from the .txt file
         * into a file with the extension .py. This file can then be launched on the command line.
         */

        string nameFile = "ML";                                         //  Python script file name.
        var MLFile = Resources.Load<TextAsset>("Text/" + nameFile);     //  Get script from resource folder (file .txt)
        FileUtils.SavePy(MLFile.bytes, MLFile.name);                    //  Convert .txt in script .py

        currentScene = SceneManager.GetActiveScene();                   // Get active scene.
        Debug.Log("********CurrentScene" + SceneManager.GetActiveScene().name);

        switch (currentScene.buildIndex){
            case (0):
                currSceneEnum = SceneEnum.MainPage;
                Debug.Log("Current scene: " + currSceneEnum);
                break;
            case (1):
                currSceneEnum = SceneEnum.PlayScene;
                Debug.Log("Current scene: " + currSceneEnum);
                break;
            case (2):
                currSceneEnum = SceneEnum.TrainingScene;
                Debug.Log("Current scene: " + currSceneEnum);
                break;
            case (3):
                currSceneEnum = SceneEnum.TestingScene;
                Debug.Log("Current scene: " + currSceneEnum);
                break;
        }

        //  Current scene Main Page
        if (currSceneEnum == SceneEnum.MainPage){}

        //  Current scene Play
        if (currSceneEnum == SceneEnum.PlayScene){
            TestML.Populate();
        }

        // Current scene Training
        if (currSceneEnum == SceneEnum.TrainingScene){
            if (TestML.Populate()){                // True if weights.txt e bias.txt exist
                SetLearnStatus(true);              // Learning done on the selected dataset
                UpdateLatestLearningDate();        // Update last training date
            }else{
                SetLearnStatus(false);
            }                               // No training on the selected dataset
        }

        //  Current scene Testing
        if (currSceneEnum == SceneEnum.TestingScene){
            TestML.Populate();
        }
    }

    void Start(){
        trainer = new TrainingScript();

        currentScene = SceneManager.GetActiveScene();                   // Get active scene
        Debug.Log("Current scene in start:" + currentScene.buildIndex);

        switch (currentScene.buildIndex){
            case (0):
                currSceneEnum = SceneEnum.MainPage;
                Debug.Log("Current scene: " + currSceneEnum);
                break;
            case (1):
                currSceneEnum = SceneEnum.PlayScene;
                Debug.Log("Current scene: " + currSceneEnum);
                break;
            case (2):
                currSceneEnum = SceneEnum.TrainingScene;
                Debug.Log("Current scene: " + currSceneEnum);
                break;
            case (3):
                currSceneEnum = SceneEnum.TestingScene;
                Debug.Log("Current scene: " + currSceneEnum);
                break;
        }

        counter = 0;

        if (File.Exists(FileUtils.GeneratePath("lbl_notes.txt")) != false){
            foreach (string line in System.IO.File.ReadLines(FileUtils.GeneratePath("lbl_notes.txt"))){
                Mylabel[counter] = Int32.Parse(line);
                counter++;
            }
        }else{ Debug.Log("lbl_notes missing"); }

        list_posizioni = new List<Position>();

        if (currSceneEnum == SceneEnum.MainPage){
            playButton = GameObject.Find("PlayButton").GetComponent<Button>();    // Play Button

            //  Check if there are all necessary files to Play
            try{
                playButton.interactable = FileUtils.CheckForDefaultFiles();
            }catch (Exception ex){
                Debug.Log(ex.Message);
            }
            UpdateSelectedDatasetText();            // Update text selected dataset
        }

        if (currSceneEnum == SceneEnum.PlayScene){
            ConnectLeapPanel = GameObject.Find("ConnectLeapPanel");         //  Popup ConnectLeapPanel
        }
        if (currSceneEnum == SceneEnum.TrainingScene){

            ConnectLeapPanel = GameObject.Find("ConnectLeapPanel");         //  Popup ConnectLeapPanel
            ClosePopUp();                                                   //  Close popup

            FileUtils.UpdateTrainedNotesList(FileUtils.filename);           //  Update recorded notes list
            UpdateButtonsKeyboard();                                        //  Update the keyboard piano

            startTime = Time.time;
            LoadingCircle.SetActive(false);
        }

        if (currSceneEnum == SceneEnum.TestingScene){
            ConnectLeapPanel = GameObject.Find("ConnectLeapPanel");         //  Popup ConnectLeapPanel
            ClosePopUp();                                                   //  Close popup
        }
    }
    
    void FixedUpdate(){
        if (currSceneEnum == SceneEnum.PlayScene){
            if (isActive){

                // Get current features
                current_Features = TestingScript.GetCurrentFeatures();

                //  Save previous note
                indexPreviousNote = indexPlayingNote;

                // Perform prediction
                indexPlayingNote = Mylabel[TestML.ReteNeurale(current_Features)];

                Debug.Log("Actual Note " + indexPlayingNote);

                // Update keyboard piano with the current note
                ChangeColor(indexPreviousNote, indexPlayingNote);
            }

            if (!isActive){
                // Default color notes
                ResetColorNotes();
            }
        }

        if (currSceneEnum == SceneEnum.TrainingScene){
            // Loading animation
            if (LoadingCircle.activeSelf)
                StartCircleAnimation();
        }
    }

    /// <summary>
    /// Update selected configuration name in the main page.
    /// </summary>
    public void UpdateSelectedDatasetText(){
        SelectedDatasetText.text = "Selected Configuration: " + FileUtils.selectedDataset;
    }

    /// <summary>
    /// Update last training date.
    /// </summary>
    public void UpdateLatestLearningDate(){
        GameObject.Find("LearnButtonPrinc").GetComponent<MeshRenderer>().material = Resources.Load("TransparentBlue", typeof(Material)) as Material;
        GameObject.Find("LearnButtonPrinc").GetComponent<Light>().range = 0;
        DateLatestLearning.GetComponent<Text>().text = "Latest Learning: \n " + TestML.DateLatestLearning.ToString();
    }

    #endregion

    ///
    /// ----------------------------------- CONFIGURATION MANAGEMENT -----------------------------------
    ///
    #region Configurations Management

    /// <summary>
    /// Button event on training note.
    /// </summary>
    public void TrainButtonClick(TrainingScript trainerMy) {

        Material LightGreen = Resources.Load("LightenedGreen", typeof(Material)) as Material;

        GameObject Button = GameObject.Find("SaveButton");
        Button.GetComponent<MeshRenderer>().material = LightGreen;
        Button.GetComponent<Light>().range = 10;

        trainerMy.Trainer();
    }

    /// <summary>
    /// Button event to remove note from dataset.
    /// </summary>
    public async void RemoveButtonClick(GameObject Circle){
        GameObject Button = GameObject.Find("RemoveButton");
        Material LRed = Resources.Load("LightenedRed", typeof(Material)) as Material;

        Button.GetComponent<MeshRenderer>().material = LRed;
        Button.GetComponent<Light>().range = 10;

        Circle.SetActive(true);

        if (await ThatTrainer.RemoveNote()){
            UpdateButtonsKeyboard();
            Circle.SetActive(false);
            Material TRed = Resources.Load("TransparentRed", typeof(Material)) as Material;

            Button.GetComponent<MeshRenderer>().material = TRed;
            Button.GetComponent<Light>().range = 0;
        }
    }

    /// <summary>
    /// Delete all files from dataset
    /// </summary>
    public void ClearDefaultDatasetDirectory(){
        FileUtils.ClearDefaultDatasetDirectory();
        ResetColorNotes();
    }

    #endregion


    ///
    /// ----------------------------------- KEYBOARD BUTTONS -----------------------------------
    ///
    #region Keyboard Buttons

    public void GetTrainer(TrainingScript trainerMy){
        ThatTrainer = trainerMy;
    }

    /// <summary>
    /// Change color of the note on the keyboard piano.
    /// </summary>
    /// <param name="id_prev">Previous Note Id </param>
    /// <param name="id_curr">Next Note Id</param>
    private void ChangeColor(int id_prev, int id_curr){
        GameObject[] z;
        GameObject[] y = new GameObject[24];
        GameObject[] x = new GameObject[24];

        for (int i = 0; i < 24; i++){
            z = GameObject.FindGameObjectsWithTag("" + i);
            y[i] = z[0];
            z = GameObject.FindGameObjectsWithTag("A" + i);
            x[i] = z[0];
        }

        List<int> SharpNotes = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22 };
        Color old;
        if (id_prev == id_curr){
            // return;
        }else{
            if (id_curr != 24){
                // set color of the note
                x[id_curr].GetComponent<Renderer>().material.color = Color.blue;
                y[id_curr].transform.rotation = Quaternion.Euler(-3.4f, 0.0f, 0.0f);
            }

            //remove color of the previous note
            if (SharpNotes.Contains(id_prev)){
                old = Color.black;
            }else{
                old = Color.white;
            }

            if (id_prev != 24){
                x[id_prev].GetComponent<Renderer>().material.color = old;
                y[id_prev].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
        }
    }

    /// <summary>
    /// Event on key of keyboard. Training.
    /// </summary>
    /// <param name="key"></param>
    public void KeyNotesWantToSave(int key){

        GameObject[] z;

        GameObject[] x = new GameObject[24];

        Material GreenT = Resources.Load("TransparentGreen", typeof(Material)) as Material;
        Material BlueT = Resources.Load("TransparentBlue", typeof(Material)) as Material;
        Material material = BlueT;
        for (int i = 0; i < 24; i++){
            z = GameObject.FindGameObjectsWithTag("A" + i);
            x[i] = z[0];
        }
        Color old;

        List<int> SharpNotes = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22 };

        if (SharpNotes.Contains(previousclickedKey)){
            old = Color.black;
        }else{
            old = Color.white;
        }

        Debug.Log("Previous clicked" + previousclickedKey);
        if (trainedNotes.Contains(previousclickedKey)){
            if (previousclickedKey != 24){
                old = Color.green;
            }else{
                material = GreenT;
                GameObject.Find("PauseButton").GetComponent<Light>().range = 0;
            }
        }
        if (previousclickedKey != 24){
            x[previousclickedKey].GetComponent<Renderer>().material.color = old;
        }else{
            // Pause Old click
            GameObject.Find("PauseButton").GetComponent<MeshRenderer>().material = material;
            GameObject.Find("PauseButton").GetComponent<Light>().range = 0;
        }

        previousclickedKey = key;
        if (key != 24){
            x[key].GetComponent<Renderer>().material.color = Color.blue;
        }else{
            // Pause button
            GameObject.Find("PauseButton").GetComponent<MeshRenderer>().material = Resources.Load("LightenedBlue", typeof(Material)) as Material;
            GameObject.Find("PauseButton").GetComponent<Light>().range = 20;
        }
        currentScene = SceneManager.GetActiveScene();
        Debug.Log("Current scene: " + currentScene.buildIndex);
        currSceneEnum = SceneEnum.TrainingScene;
        Debug.Log("Current scene enumerator: " + currSceneEnum);

        if (currSceneEnum == SceneEnum.TrainingScene){
            ThatTrainer.ChangeNoteId(key);
        }

        // Testing ... (ConfusionTestingScript.cs)
        // if (currSceneEnum == SceneEnum.TestingScene)
        //      tester.ChangeNoteId(key);
    }

    /// <summary>
    /// Highlight all trained notes.
    /// </summary>
    public void UpdateButtonsKeyboard(){
        ResetColorNotes();
        GameObject[] z;
        GameObject[] y = new GameObject[24];

        for (int i = 0; i < 24; i++){
            z = GameObject.FindGameObjectsWithTag("A" + i);
            y[i] = z[0];
        }

        foreach (var item in trainedNotes){
            if (item != 24){
                y[item].GetComponent<Renderer>().material.color = Color.green;
            }else{
                pauseButton.GetComponent<MeshRenderer>().material = Resources.Load("TransparentGreen", typeof(Material)) as Material;
            }
        }
    }

    /// <summary>
    /// Reset color on keyboard.
    /// </summary>
    public void ResetColorNotes(){
        //Pause Old click
        if (pauseButton != null){
            pauseButton.GetComponent<MeshRenderer>().material = Resources.Load("TransparentBlue", typeof(Material)) as Material;
            pauseButton.GetComponent<Light>().range = 0;
        }
        
        GameObject[] z = new GameObject[48];
        GameObject[] y = new GameObject[24];
        GameObject[] x = new GameObject[24];

        for (int i = 0; i < 24; i++){
            z = GameObject.FindGameObjectsWithTag("A" + i);
            y[i] = z[0];
            z = GameObject.FindGameObjectsWithTag("" + i);
            x[i] = z[0];
        }

        List<int> SharpNotes = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22 };
        Color old;

        for (int i = 0; i < 24; i++){
            if (SharpNotes.Contains(i)){
                old = Color.black;
            }else{
                old = Color.white;
            }
            y[i].GetComponent<Renderer>().material.color = old;
            x[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    #endregion


    ///
    /// ----------------------------------- NAVIGATION -----------------------------------
    ///
    #region NAVIGATION

    /// <summary>
    /// Close application.
    /// </summary>
    public void QuitGame() => Application.Quit();

    /// <summary>
    /// Navigate to Main scene.
    /// </summary>
    public void NavigateToMainScene(){
        SceneManager.LoadScene(0);
        currSceneEnum = SceneEnum.MainPage;
    }

    /// <summary>
    /// Navigate to Test scene.
    /// </summary>
    public void NavigateToTestScene(){
        TestML.Populate();
        SceneManager.LoadScene(1);
        currSceneEnum = SceneEnum.PlayScene;
    }

    /// <summary>
    /// Navigate to Testing with confusion matrix.
    /// </summary>
    public void NavigateToTestingMatrixScene(){
        SceneManager.LoadScene(3);
        currSceneEnum = SceneEnum.TestingScene;
    }

    /// <summary>
    /// Navigate to Training scene.
    /// </summary>
    public void NavigateToTrainingtScene(){
        SceneManager.LoadScene(2);
        currSceneEnum = SceneEnum.TrainingScene;
    }

    #endregion


    ///
    /// ----------------------------------- PANELS -----------------------------------
    ///
    #region Panels (Managing Configurations)

    /// <summary>
    /// Open configuration panel.
    /// </summary>
    public void OpenPanel(){
        PanelUtils.OpenPanel();

        Debug.Log(FileUtils.selectedDataset);
        if (currSceneEnum == SceneEnum.MainPage){
            UpdateSelectedDatasetText();
            playButton.interactable = FileUtils.CheckForDefaultFiles();
        }
    }

    /// <summary>
    /// Open panel to import the dataset.
    /// </summary>
    public void OpenImportPanel(){
        PanelUtils.OpenImportPanel();
        if (currSceneEnum == SceneEnum.MainPage)
            UpdateSelectedDatasetText();
    }

    /// <summary>
    /// Open panel to export the dataset.
    /// </summary>
    public void OpenExportPanel(){
        PanelUtils.OpenExportPanel();
    }

    #endregion

    ///
    /// ----------------------------------- POPUPS -----------------------------------
    ///

    #region Popups
    /// <summary>
    /// Open info popup.
    /// </summary>
    public void OpenPopUp(GameObject popup){
        popup.SetActive(true);
    }

    /// <summary>
    /// Close info popup.
    /// </summary>
    public void ClosePopUp(){
        PopupPanel.SetActive(false);
    }

    public void ButtonCloseP(GameObject popup){
        popup.SetActive(false);
    }

    /// <summary>
    /// Popup LeapMotion not connected.
    /// </summary>
    public static void ShowConnectLeapPopup(){
        ConnectLeapPanel.SetActive(true);
    }

    /// <summary>
    /// Popup LeapMotion connected.
    /// </summary>
    public static void HideConnectLeapPopup(){
        ConnectLeapPanel.SetActive(false);
    }

    /// <summary>
    /// Popup to delete the dataset.
    /// </summary>
    public void OpenDeletePanel(GameObject panel){
        GameObject.Find("RemoveButtonBody").GetComponent<MeshRenderer>().material = Resources.Load("LightenedRed", typeof(Material)) as Material;
        GameObject.Find("RemoveButtonBody").GetComponent<Light>().range = 10;
        panel.SetActive(true);
    }

    public void CloseDeletePanel(GameObject panel){
        panel.SetActive(false);
        GameObject.Find("RemoveButtonBody").GetComponent<MeshRenderer>().material = Resources.Load("RedDark", typeof(Material)) as Material;
        GameObject.Find("RemoveButtonBody").GetComponent<Light>().range = 0;
    }

    #endregion


    ///<summary>
    /// Start loading animation.
    ///</summary>
    public void StartCircleAnimation(){
        if (Time.time - startTime >= timeStep){
            Vector3 iconAngle = mainIcon.localEulerAngles;
            iconAngle.z += oneStepAngle;

            mainIcon.localEulerAngles = iconAngle;

            startTime = Time.time;
        }
    }

    /// <summary>
    /// Show check near Learn button to communicate that learning was previously done for the selected configuration.
    /// </summary>
    /// <param name="state">True learning done, false otherwise</param>
    public void SetLearnStatus(bool state){
        ConfLearn.SetActive(state);
        DateLatestLearning.SetActive(state);
        ConfNotLearn.SetActive(!state);
    }
}
