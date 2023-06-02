using UnityEngine;

using Leap;
using Leap.Unity;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Gestisce interfacciamento tra Unity3D e LeapMotion
/// </summary>
public class MS_LeapController : MonoBehaviour
{
    
    /// <summary>
    /// Controller leap motion
    /// </summary>
    Controller controller;
    /// <summary>
    /// Listener
    /// </summary>
    MS_LeapListener listener;

    /// <summary>
    /// Flag per tenere traccia della connessione del leap
    /// </summary>
    bool connected = false;
    /// <summary>
    /// Flag per tenere traccia della notifica (non a video) della connesione del leap
    /// </summary>
    bool notified = false;
    /// <summary>
    /// Flag per tenere traccia della notifica tramite popup che il leap non è connesso
    /// </summary>
    bool notConnectedShowed = true;

    public LeapProvider leapProvider1;
    public LeapProvider leapProvider2;
    ScriptDiAppoggio scriptDiAppoggio;

    // Start is called before the first frame update
    void Start()
    {
        //  inizializza un novo controller per il leap
        controller = new Controller();

        //  assegna il device quando un sensore leap motion è connesso
        controller.Device += MS_LeapListener.OnLeapConnect;

        _GM.ShowConnectLeapPopup();

    }

    // Update is called once per frame
    void Update()
    {
        if(controller.Devices.Count <=1  && !notConnectedShowed)
        {
            //Debug.Log("Sono in if controller in Update MS_LeapController");
            notified = false;
            notConnectedShowed = true;
            MS_LeapListener.Connected = false;
            _GM.ShowConnectLeapPopup();
        }

        /*
         * //SINGOLO DEVICE
         //  ascolta ogni frame del leap motion
         if (controller.Devices.Count > 0 && !connected)
         {
            // Debug.Log("Sono in Update MS_LeapController");
             leapProvider.OnUpdateFrame += MS_LeapListener.chiamaFunzione;
             connected = true;
         }
        */

        //AGGIUNTA PER DUE DEVICE
        if (controller.Devices.Count >= 2 && leapProvider1 != null && leapProvider2 != null )
        {
            
            Frame frame1 = leapProvider1.CurrentFrame;
            Frame frame2 = leapProvider2.CurrentFrame;
            MS_LeapListener.onFrameDisp1(frame1);
            MS_LeapListener.onFrameDisp2(frame2);
            

            //aggiornaFrame();
            //connected = true;
        }

        if (MS_LeapListener.Connected && !notified)
        {
            notified = true;
            notConnectedShowed = false;
            _GM.HideConnectLeapPopup();
        }
    }

    //Funzione di prova per stampa dati 
    public void aggiornaFrame()
    {
        Frame frame1 = leapProvider1.CurrentFrame;
        Frame frame2 = leapProvider2.CurrentFrame;
        List<Hand> handsFrame1 = frame1.Hands;
        List<Hand> handsFrame2 = frame2.Hands;

        /*
        MS_LeapListener.chiamaFunzione(frame1, 0);
        MS_LeapListener.chiamaFunzione(frame2, 1);
        */

        
        foreach (Hand h in handsFrame1)
        {
            if (h.IsLeft)
            {
                DataSelectorSinistra(h,1);
            }
            else if (h.IsRight)
            {
                DataSelectorDestra(h,1);
            }

        }
        
        foreach (Hand h in handsFrame2)
        {
            if (h.IsLeft)
            {
                DataSelectorSinistra(h,2);
            }
            else if (h.IsRight)
            {
                DataSelectorDestra(h,2);
            }

        }
       

        
    }

    public void DataSelectorSinistra(Hand l, int numDevice)
    {
        //Debug.Log("SONO IN DATASELECTOR SCRIPT DI APPOGGIO");
        //  mano sinistra
        var left_hand = new DataToStore(
            l,
            DatasetHandler.getFF(l.Fingers[0], true),
            DatasetHandler.getFF(l.Fingers[1]),
            DatasetHandler.getFF(l.Fingers[2]),
            DatasetHandler.getFF(l.Fingers[3]),
            DatasetHandler.getFF(l.Fingers[4]),

            DatasetHandler.getNFA(l.Fingers[0], l.Fingers[1]),
            DatasetHandler.getNFA(l.Fingers[1], l.Fingers[2]),
            DatasetHandler.getNFA(l.Fingers[2], l.Fingers[3]),
            DatasetHandler.getNFA(l.Fingers[3], l.Fingers[4]));
        Debug.Log("left_hand :" + numDevice + " in script appoggio: " + left_hand);


    }

    public void DataSelectorDestra(Hand r, int numDevice)
    {
        //Debug.Log("SONO IN DATASELECTOR SCRIPT DI APPOGGIO");
        //  mano destra
        var right_hand = new DataToStore(
            r,
            DatasetHandler.getFF(r.Fingers[0], true),
            DatasetHandler.getFF(r.Fingers[1]),
            DatasetHandler.getFF(r.Fingers[2]),
            DatasetHandler.getFF(r.Fingers[3]),
            DatasetHandler.getFF(r.Fingers[4]),

            DatasetHandler.getNFA(r.Fingers[0], r.Fingers[1]),
            DatasetHandler.getNFA(r.Fingers[1], r.Fingers[2]),
            DatasetHandler.getNFA(r.Fingers[2], r.Fingers[3]),
            DatasetHandler.getNFA(r.Fingers[3], r.Fingers[4]));
        Debug.Log("right_hand dev: " + numDevice + " in script appoggio: " + right_hand);
    }

}
