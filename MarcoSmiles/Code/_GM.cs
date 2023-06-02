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
/// Game Master Contiene lo stato dell'applicazione.
/// </summary>
public class _GM : MonoBehaviour
{
    TrainingScript ThatTrainer;



    public int previousclickedKey = 0;
    public int k = 1;
    public int counter = 0;
    public int[] Mylabel = new int[25];

    /// <summary>
    /// Flag necessario per sapere se il leap motion è connesso
    /// </summary>
    public static bool IsLeapConnected = false;

    /// <summary>
    /// Bottone preseente nel main menu per passare a scena "PlayScene"
    /// </summary>
    private Button playButton;

    /// <summary>
    /// Istanza della classe trainer, utilizzata esclusvamente nella scena di training
    /// </summary>
    public TrainingScript trainer;
    /// <summary>
    /// Dati statici della mano destra
    /// </summary>
    public static Hand hand_R;
    /// <summary>
    /// Dati statici della mano sinistra
    /// </summary>
    public static Hand hand_L;

    /// <summary>
    /// Dati statici della mano destra del secondo device
    /// </summary>
    public static Hand secondDeviceHand_R;
    /// <summary>
    /// Dati statici della mano sinistra del secondo device
    /// </summary>
    public static Hand secondDeviceHand_L;

    /// <summary>
    /// Istanza della classe di testing per le matrici, utilizzata esclusivamente nella scena di testing
    /// </summary>
    public ConfusionTestingScript tester;

    /// <summary>
    /// Viene usata solo nella scena di training per salvare nel dataset
    /// </summary>
    public static List<Position> list_posizioni;
    /// <summary>
    /// Contiene una lista di indici, che rappresentano le note allenate (0-23)
    /// </summary>
    public static List<int> trainedNotes;

    /// <summary>
    /// true se ci sono le mani, altrimenti false. lo script ProceduralAudioOscillator.cs osserva questa variabile per decidere se deve suonare 
    /// </summary>
    public static bool isActive = false;
    /// <summary>
    /// Attualmente le features sono floats, risolviamo sto problemo
    /// </summary>
    public static float[] current_Features;
    /// <summary>
    /// Indice della nota corrente da suonare, � letta da PCMOscillator
    /// </summary>
    public static int indexPlayingNote;
    /// <summary>
    /// Indice della nota precedentemente suonata nel fixed update precedente
    /// </summary>
    public static int indexPreviousNote;


    /// <summary>
    /// Variabile contenente riferimento al pannello di popup
    /// </summary>
    public GameObject PopupPanel;
    /// <summary>
    /// Variabile contenente riferimento al pannello di popup di connessione leap
    /// </summary>
    public static GameObject ConnectLeapPanel;

    //  Interfaccia. Per sapere se � stato fatto Learn su configurazione selezionata
    /// <summary>
    /// Gameobject contenente immagine che indica che � stato fatto Learn per la conf. selezionata. (TRAINING SCENE)                           
    /// </summary>
    public GameObject ConfLearn;
    /// <summary>
    /// Gameobject contenente immagine che indica che NON � stato fatto Learn per la conf. selezionata. (TRAINING SCENE)
    /// </summary>
    public GameObject ConfNotLearn;
    /// <summary>
    /// Gameobject contenente la data dell'ultimo Learn per la conf. selezionata. (TRAINING SCENE)
    /// </summary>
    public GameObject DateLatestLearning;

    //  Variabili animazione loading circle
    /// <summary>
    /// Gameobject contenente l'animazione
    /// </summary>
    public GameObject LoadingCircle;
    /// <summary>
    /// Icona per l'animazione
    /// </summary>
    public RectTransform mainIcon;
    /// <summary>
    /// Intervallo di tempo per l'animazione di caricamento
    /// </summary>
    public static float timeStep = 0.1f;
    /// <summary>
    /// Angolo di rotazione per l'animazione di caricamento
    /// </summary>
    public static float oneStepAngle = -36;
    float startTime;

    /// <summary>
    /// Testo contenente il nome della Configurazione (cartella) selezionata
    /// </summary>    
    public Text SelectedDatasetText;
    /// <summary>
    /// Enum per le scene unity esistenti nella build
    /// </summary>
    private enum SceneEnum
    {
        MainPage,
        PlayScene,
        TrainingScene,
        TestingScene
    }
    /// <summary>
    /// Variabile per tenere traccia della scena corrente
    /// </summary>
    private SceneEnum currSceneEnum;
    /// <summary>
    /// Oggetto scena, utilizzato per modificare la variabile currSceneEnum
    /// </summary>
    private Scene currentScene;
    private GameObject pauseButton;
    private AsyncOperation loadingOperation0;
    


    #region UNITY METH

    /// <summary>
    /// Chiamato quando viene inizializzato un oggetto contenente lo script _GM.cs
    /// </summary>
    private void Awake()
    {
        pauseButton = GameObject.Find("PauseButton");
        trainer = new TrainingScript();

        /*
         * In unity, possono essere caricati nella build solo determinati tipi di file. File .txt vengono copiati all'interno della cartella 
         * della build.
         * In questo modo riusciamo ad avere lo script .py (che in questo momento � un file .txt) all'interno della build.
         * Dunque, leggiamo il file .txt dalla cartella Resources, e usaando il metodo SavePy, salviamo lo script letto dal file .txt
         * in un file ad estensione .py. Questo file potr� poi essere lanciato su linea di comando.    
         */

        string nameFile = "ML";                                         //  Nome dello script python. 
        var MLFile = Resources.Load<TextAsset>("Text/" + nameFile);     //  Carica lo script dalla cartella Resources di Unity(file .txt)
        FileUtils.SavePy(MLFile.bytes, MLFile.name);                    //  Converte il file .txt in script .py

        currentScene = SceneManager.GetActiveScene();                   //  Prende la scena correntemente attiva
        Debug.Log("********CurrentScene" + SceneManager.GetActiveScene().name);

        switch (currentScene.buildIndex)
        {
            case (0):
                currSceneEnum = SceneEnum.MainPage;
                Debug.Log("Current scene enumerator" + currSceneEnum);
                break;
            case (1):
                currSceneEnum = SceneEnum.PlayScene;
                Debug.Log("Current scene enumerator" + currSceneEnum);
                break;
            case (2):
                currSceneEnum = SceneEnum.TrainingScene;
                Debug.Log("Current scene enumerator" + currSceneEnum);
                break;
            case (3):
                currSceneEnum = SceneEnum.TestingScene;
                Debug.Log("Current scene enumerator" + currSceneEnum);
                break;
        }

        //  Caso in cui la scena corrente � la scena mainpage(Mainpage)
        if (currSceneEnum == SceneEnum.MainPage)
        {
        
        }

        //  Caso in cui la scena corrente � la scena per suonare (PlayScene)
        if (currSceneEnum == SceneEnum.PlayScene)
        {
            TestML.Populate();                                              //  Effettua il caricamento dei file necessari per la scena PlayScene

        }

        // Se la scena corrente � la scena di training (TrainingScene)
        if (currSceneEnum == SceneEnum.TrainingScene)
        {
            if (TestML.Populate())
            {                                        //  Restituisce true se trova i file weights.txt e bias.txt
                SetLearnStatus(true);                                       //  Segnala che � stato effettuato il Learning sul dataset selezionato 
                UpdateLatestLearningDate();                                 //  Aggiorna il testo contente la data dell'ultimo training effettuato sul dataset selezionato
            }
            else
                SetLearnStatus(false);                                      //  Segnala che non � stato effettuato il trianing sul dataset selezionato 
        }

        // Se la scena corrente � la scena di testing (TestingScene)
        if (currSceneEnum == SceneEnum.TestingScene)
        {
            TestML.Populate();
        }
    }

    void Start()
    {
        trainer = new TrainingScript();

        currentScene = SceneManager.GetActiveScene();                   //  Prende la scena correntemente attiva
        Debug.Log("AGGIORNATO____CurrentScene in start:" + currentScene.buildIndex);

        switch (currentScene.buildIndex)
        {
            case (0):
                currSceneEnum = SceneEnum.MainPage;
                Debug.Log("Current scene enumerator" + currSceneEnum);
                break;
            case (1):
                currSceneEnum = SceneEnum.PlayScene;
                Debug.Log("Current scene enumerator" + currSceneEnum);
                break;
            case (2):
                currSceneEnum = SceneEnum.TrainingScene;
                Debug.Log("Current scene enumerator" + currSceneEnum);
                break;
            case (3):
                currSceneEnum = SceneEnum.TestingScene;
                Debug.Log("Current scene enumerator" + currSceneEnum);
                break;
        }


        counter = 0;

        if (File.Exists(FileUtils.GeneratePath("lbl_notes.txt")) != false)
        {
            foreach (string line in System.IO.File.ReadLines(FileUtils.GeneratePath("lbl_notes.txt")))
            {

                Mylabel[counter] = Int32.Parse(line);

                counter++;
            }
        }
        else
        {
            Debug.Log("lbl_notes � assente");
        }


        list_posizioni = new List<Position>();

        if (currSceneEnum == SceneEnum.MainPage)
        {
            
            playButton = GameObject.Find("PlayButton").GetComponent<Button>();          //  Istanzia il pulsante PlayButton

            //  Controlla se ci sono i file necessari per passare alla scena "Play"
            try
            {
                playButton.interactable = FileUtils.CheckForDefaultFiles();

            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }


            UpdateSelectedDatasetText();            //  Aggiorna il testo del dataset selezionato

        }

        if (currSceneEnum == SceneEnum.PlayScene)
        {
            ConnectLeapPanel = GameObject.Find("ConnectLeapPanel");         //  Istanzia il popup ConnectLeapPanel
        }

        if (currSceneEnum == SceneEnum.TrainingScene)
        {



            ConnectLeapPanel = GameObject.Find("ConnectLeapPanel");         //  Istanzia il popup ConnectLeapPanel
            ClosePopUp();                                                   //  Chiude il popup






            FileUtils.UpdateTrainedNotesList(FileUtils.filename);           //  Aggiorna la lista delle note gi� registrate
            UpdateButtonsKeyboard();                                        //  Aggiorna la tastiera

            startTime = Time.time;
            LoadingCircle.SetActive(false);
        }

        if (currSceneEnum == SceneEnum.TestingScene)
        {
            ConnectLeapPanel = GameObject.Find("ConnectLeapPanel");         //  Istanzia il popup ConnectLeapPanel
            ClosePopUp();                                                   //  Chiude il popup
        }

    }
    
    void FixedUpdate()
    {
        //Debug.Log("UPDATE curr scene " + currSceneEnum);
        if (currSceneEnum == SceneEnum.PlayScene)
        {
            if (isActive)
            {

                // Aggiorna array delle features currentFeatures in modo tale che venga calcolata la nota giusta ad ogni update 
                current_Features = TestingScript.GetCurrentFeatures();

                //salva la nota che si stava suonando nell'update precedefnte prima di calcolare la nuova nota
                //  Salva in memoria l'indice dell'ultima nota suonata
                indexPreviousNote = indexPlayingNote;

                //  Rappresenta la nota che deve essere suonata
                Debug.Log("return valiue " + TestML.ReteNeurale(current_Features));
                indexPlayingNote = Mylabel[TestML.ReteNeurale(current_Features)];

                Debug.Log("Actual Note " + indexPlayingNote);

                //  Cambia il colore del tasto sulla tastiera corrispondente alla nota che si sta suonando  
                ChangeColor(indexPreviousNote, indexPlayingNote);
            }


            if (!isActive)
            {
                // Ripristina i colori delle note al default
                ResetColorNotes();
            }
        }

        if (currSceneEnum == SceneEnum.TrainingScene)
        {
            //  Avvia l'animazione di caricamento se necessario
            if (LoadingCircle.activeSelf)
                StartCircleAnimation();
        }
    }

    /// <summary>
    /// Aggiorna il nome della configurazione selezionata all'interno del Menu Principale
    /// </summary>
    public void UpdateSelectedDatasetText()
    {
        SelectedDatasetText.text = "Selected Configuration: " + FileUtils.selectedDataset;
    }

    /// <summary>
    /// Aggiorna la data dell'ultimo addestramento effettuato all'interno della scena Training
    /// </summary>
    public void UpdateLatestLearningDate()
    {
        GameObject.Find("LearnButtonPrinc").GetComponent<MeshRenderer>().material = Resources.Load("TransparentBlue", typeof(Material)) as Material;
        GameObject.Find("LearnButtonPrinc").GetComponent<Light>().range = 0;
        DateLatestLearning.GetComponent<Text>().text = "Latest Learning: \n " + TestML.DateLatestLearning.ToString();
    }

    #endregion


    #region Configurations Management

    /// <summary>
    /// Lanciato quando viene premuto il pulsante di training per la nota selezionata
    /// </summary>
    public void TrainButtonClick(TrainingScript trainerMy)
    {

        Material LightGreen = Resources.Load("LightenedGreen", typeof(Material)) as Material;

        GameObject Button = GameObject.Find("SaveButton");
        Button.GetComponent<MeshRenderer>().material = LightGreen;
        Button.GetComponent<Light>().range = 10;

        trainerMy.Trainer();

        //else if (currSceneEnum == SceneEnum.TestingScene)
        //   tester.Tester();
    }

    /// <summary>
    /// Lanciato quando viene premuto il pulsante di rimozione nota da dataset per la nota selezionata
    /// </summary>
    public async void RemoveButtonClick(GameObject Circle)
    {
        GameObject Button = GameObject.Find("RemoveButton");
        Material LRed = Resources.Load("LightenedRed", typeof(Material)) as Material;


        Button.GetComponent<MeshRenderer>().material = LRed;
        Button.GetComponent<Light>().range = 10;

        Circle.SetActive(true);

        //L'operatore await sospende la valutazione del metodo asincrono racchiuso fino al completamento
        //dell'operazione asincrona rappresentata dal suo operando. Quando l'operazione asincrona � completata,
        //l'operatore await restituisce il risultato dell'operazione, se presente.

        if (await ThatTrainer.RemoveNote())
        {
            UpdateButtonsKeyboard();
            Circle.SetActive(false);
            Material TRed = Resources.Load("TransparentRed", typeof(Material)) as Material;


            Button.GetComponent<MeshRenderer>().material = TRed;
            Button.GetComponent<Light>().range = 0;
        }
    }

    /// <summary>
    /// Elimina tutti i file nel Dataset selezionato
    /// </summary>
    public void ClearDefaultDatasetDirectory()
    {
        FileUtils.ClearDefaultDatasetDirectory();
        ResetColorNotes();
    }

    #endregion

    #region Keyboard Buttons
    public void GetTrainer(TrainingScript trainerMy)
    {
        ThatTrainer = trainerMy;
    }



    /// <summary>
    /// Cambia il colore della nota da suonare, ripristinando al colore di default la nota precedentemente suonata (se necessario)
    /// Se la nota da suonare � la stessa della precedente non cambia nulla
    /// </summary>
    /// <param name="id_prev">id nota precedenteme</param>
    /// <param name="id_curr">id nota da suonare</param>
    private void ChangeColor(int id_prev, int id_curr)
    {
        GameObject[] z;
        GameObject[] y = new GameObject[24];
        GameObject[] x = new GameObject[24];


        for (int i = 0; i < 24; i++)
        {
            z = GameObject.FindGameObjectsWithTag("" + i);
            y[i] = z[0];
            z = GameObject.FindGameObjectsWithTag("A" + i);
            x[i] = z[0];


        }



        List<int> SharpNotes = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22 };
        Color old;
        if (id_prev == id_curr)
        {
            // return;
        }
        else
        {

            if (id_curr != 24)
            {
                //coloro la corrente nota
                x[id_curr].GetComponent<Renderer>().material.color = Color.blue;
                y[id_curr].transform.rotation = Quaternion.Euler(-3.4f, 0.0f, 0.0f);
            }



            //decoloro la precedente

            if (SharpNotes.Contains(id_prev))
            {
                old = Color.black;
            }
            else
            {
                old = Color.white;
            }
            if (id_prev != 24)
            {
                x[id_prev].GetComponent<Renderer>().material.color = old;
                y[id_prev].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }


        }


    }





    /// <summary>
    /// Viene chiamato ogni volta che un tasto del pianoforte viene premuto, in modo che venga cambiato l'id
    /// della nota selezionata. Serve alla classe Trainer.
    /// </summary>
    /// <param name="key"></param>
    public void KeyNotesWantToSave(int key)
    {

        GameObject[] z;

        GameObject[] x = new GameObject[24];

        Material GreenT = Resources.Load("TransparentGreen", typeof(Material)) as Material;
        Material BlueT = Resources.Load("TransparentBlue", typeof(Material)) as Material;
        Material material = BlueT;
        for (int i = 0; i < 24; i++)
        {

            z = GameObject.FindGameObjectsWithTag("A" + i);
            x[i] = z[0];


        }
        Color old;


        List<int> SharpNotes = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22 };

        if (SharpNotes.Contains(previousclickedKey))
        {
            old = Color.black;
        }
        else
        {
            old = Color.white;
        }

        Debug.Log("PREVIOSU CLICKED " + previousclickedKey);
        if (trainedNotes.Contains(previousclickedKey))
        {
            Debug.Log("YES CONTAIN " + previousclickedKey);
            if (previousclickedKey != 24)
            {
                old = Color.green;
            }
            else
            {
                Debug.Log("IMHERE");
                material = GreenT;
                GameObject.Find("PauseButton").GetComponent<Light>().range = 0;

            }
        }
        if (previousclickedKey != 24)
        {
            x[previousclickedKey].GetComponent<Renderer>().material.color = old;
        }
        else
        {
            //Pause Old click
            GameObject.Find("PauseButton").GetComponent<MeshRenderer>().material = material;
            GameObject.Find("PauseButton").GetComponent<Light>().range = 0;
        }

        previousclickedKey = key;
        if (key != 24)
        {
            x[key].GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            //if is the pause button a pause
            GameObject.Find("PauseButton").GetComponent<MeshRenderer>().material = Resources.Load("LightenedBlue", typeof(Material)) as Material;
            GameObject.Find("PauseButton").GetComponent<Light>().range = 20;
        }

        currentScene = SceneManager.GetActiveScene();
        Debug.Log("CurrentScene ==============" + currentScene.buildIndex);
        currSceneEnum = SceneEnum.TrainingScene;
        Debug.Log("cURRENT SCENE ENUMERATOR" + currSceneEnum);





        if (currSceneEnum == SceneEnum.TrainingScene)
        {

            Debug.Log("sono here");
            ThatTrainer.ChangeNoteId(key);
        }


        //  Serve per testing... Usa un altro script trainer (ConfusionTestingScript.cs)
        // if (currSceneEnum == SceneEnum.TestingScene)
        //   tester.ChangeNoteId(key);


    }


    /// <summary>
    /// Evidenzia tutte le note della tastiera che sono state gi� allenate (le note che sono presenti nel dataset selezionato e dunque nella lista trainedNotes)
    /// </summary>
    public void UpdateButtonsKeyboard()
    {

        ResetColorNotes();
        GameObject[] z;
        GameObject[] y = new GameObject[24];

        for (int i = 0; i < 24; i++)
        {
            z = GameObject.FindGameObjectsWithTag("A" + i);
            y[i] = z[0];

        }

        foreach (var item in trainedNotes)
        {
            if (item != 24)
            {
                y[item].GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                pauseButton.GetComponent<MeshRenderer>().material = Resources.Load("TransparentGreen", typeof(Material)) as Material;
            }
        }

    }


    /// <summary>
    /// Resetta il colore dei tasti
    /// </summary>
    public void ResetColorNotes()
    {


        //Pause Old click
        if (pauseButton != null)
        {
            pauseButton.GetComponent<MeshRenderer>().material = Resources.Load("TransparentBlue", typeof(Material)) as Material;
            pauseButton.GetComponent<Light>().range = 0;
        }
        
        GameObject[] z = new GameObject[48];
        GameObject[] y = new GameObject[24];
        GameObject[] x = new GameObject[24];

        for (int i = 0; i < 24; i++)
        {

            z = GameObject.FindGameObjectsWithTag("A" + i);
            y[i] = z[0];
            z = GameObject.FindGameObjectsWithTag("" + i);
            x[i] = z[0];
            /************************
            //AGGIUNTA IF
            if (z[0]!=null)
            {
                y[i] = z[0];
            }
            z = GameObject.FindGameObjectsWithTag("" + i);
            //AGGIUNTA IF
            if (z[0] != null)
            {
                x[i] = z[0];
            }
            */
        }
        

        List<int> SharpNotes = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22 };
        Color old;

        for (int i = 0; i < 24; i++)
        {
            if (SharpNotes.Contains(i))
            {
                old = Color.black;
            }
            else
            {
                old = Color.white;
            }
            y[i].GetComponent<Renderer>().material.color = old;
            x[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            /*
            //aggiunta IF
            if (x.Length>0 && y.Length>0)
            {
                y[i].GetComponent<Renderer>().material.color = old;
                x[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            */
        }

    }
    #endregion


    #region NAVIGATION

    /// <summary>
    /// Chiude l'applicazione
    /// </summary>
    public void QuitGame() => Application.Quit();

    /// <summary>
    /// Effettua la navigazione alla scena principale
    /// </summary>
    public void NavigateToMainScene()
    {
        Debug.Log("I PRESSED THIS BUTTON 0");
        SceneManager.LoadScene(0);
        currSceneEnum = SceneEnum.MainPage;

    }

    /// <summary>
    /// Effettua la navigazione alla scena di test
    /// </summary>
    public void NavigateToTestScene()
    {
        TestML.Populate();
        SceneManager.LoadScene(1);
        currSceneEnum = SceneEnum.PlayScene;
        Debug.Log("I PRESSED THIS BUTTON 1");
    }


    /// <summary>
    /// Effettua la navigazione alla sena di testing della matrice
    /// </summary>
    public void NavigateToTestingMatrixScene()
    {
        SceneManager.LoadScene(3);
        currSceneEnum = SceneEnum.TestingScene;
        Debug.Log("I PRESSED THIS BUTTON 3");
    }

    /// <summary>
    /// Effettua la navigazione alla scena di training
    /// </summary>
    public void NavigateToTrainingtScene()
    {
        //loadingOperation0 = SceneManager.LoadSceneAsync("TrainingScene", LoadSceneMode.Additive);
        SceneManager.LoadScene(2);
        currSceneEnum = SceneEnum.TrainingScene;
        Debug.Log("I PRESSED THIS BUTTON 2");

    }

    #endregion

    #region Panels (Managing Configurations)



    /// <summary>
    /// Apre il pannello per selezionare una configurazione.
    /// </summary>
    public void OpenPanel()
    {
        PanelUtils.OpenPanel();

        Debug.Log(FileUtils.selectedDataset);
        if (currSceneEnum == SceneEnum.MainPage)
        {
            UpdateSelectedDatasetText();
            playButton.interactable = FileUtils.CheckForDefaultFiles();
        }
    }

    /// <summary>
    /// Apre un pannello per selezionare un dataset da importare nella cartella MyDataset
    /// </summary>
    public void OpenImportPanel()
    {
        PanelUtils.OpenImportPanel();

        if (currSceneEnum == SceneEnum.MainPage)
            UpdateSelectedDatasetText();
    }

    /// <summary>
    /// Apre un pannello per selezionare un dataset da esportare in una qualsiasi directory sul pc
    /// </summary>
    public void OpenExportPanel()
    {
        PanelUtils.OpenExportPanel();
    }

    #endregion


    #region Popups
    /// <summary>
    /// Apre il PopUp dopo aver cliccato il bottone info
    /// </summary>
    public void OpenPopUp(GameObject popup)
    {

        Debug.Log("OPEN");

        popup.SetActive(true);
    }

    /// <summary>
    /// Chiude il PopUp dopo aver cliccato il bottone info
    /// </summary>
    public void ClosePopUp()
    {
        Debug.Log("Closepopup");
        PopupPanel.SetActive(false);

    }

    public void ButtonCloseP(GameObject popup)
    {
        popup.SetActive(false);
    }

    /// <summary>
    /// Mostra il popup che richiede di connettere il LeapMotion
    /// </summary>
    public static void ShowConnectLeapPopup()
    {
        ConnectLeapPanel.SetActive(true);
    }

    /// <summary>
    /// Nasconde il popup che richiede di connettere il LeapMotion
    /// </summary>
    public static void HideConnectLeapPopup()
    {
        ConnectLeapPanel.SetActive(false);
    }

    /// <summary>
    /// Mostra il popup per la conferma della cancellazione dell' intero dataset
    /// </summary>
    public void OpenDeletePanel(GameObject panel)
    {
        GameObject.Find("RemoveButtonBody").GetComponent<MeshRenderer>().material = Resources.Load("LightenedRed", typeof(Material)) as Material; //cambio il materiale 
        GameObject.Find("RemoveButtonBody").GetComponent<Light>().range = 10;                                                                   // Attivo la luce
        panel.SetActive(true);
    }

    public void CloseDeletePanel(GameObject panel)
    {
        panel.SetActive(false);
        GameObject.Find("RemoveButtonBody").GetComponent<MeshRenderer>().material = Resources.Load("RedDark", typeof(Material)) as Material;  //ritorno al materiale iniziale
        GameObject.Find("RemoveButtonBody").GetComponent<Light>().range = 0;                                                                   //disattivo la luce

    }

    #endregion


    ///<summary>
    ///  Avvia l'animazione del caricamento
    ///</summary>
    public void StartCircleAnimation()
    {
        if (Time.time - startTime >= timeStep)
        {
            Vector3 iconAngle = mainIcon.localEulerAngles;
            iconAngle.z += oneStepAngle;

            mainIcon.localEulerAngles = iconAngle;

            startTime = Time.time;
        }
    }

    /// <summary>
    /// Visualizza la spunta vicino tasto "learn" per comunicare che per la configurazione selezionata � stato fatto precedentemente il learning
    /// </summary>
    /// <param name="state">true se � stato effettuato il learning, false altrimenti</param>
    public void SetLearnStatus(bool state)
    {
        ConfLearn.SetActive(state);
        DateLatestLearning.SetActive(state);
        ConfNotLearn.SetActive(!state);
    }
}
