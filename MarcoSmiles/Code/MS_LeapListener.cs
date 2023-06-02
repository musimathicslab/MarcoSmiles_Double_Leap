using UnityEngine;

using Leap;
using Leap.Unity;
using System.Collections.Generic;
/// <summary>
///  Gestisce la connessione del LeapMotion a Unity3d.
/// </summary>
public class MS_LeapListener : MonoBehaviour
{
    public static bool Connected = false;
    

    /// <summary>
    /// Evento attivato all'esecuzione, se il leap rileva le mani.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void OnLeapConnect(object sender, DeviceEventArgs args)
    {
        //  stampa un messaggio di log
        Debug.Log("Connesso");
        Connected = true;
    }
    /// <summary>
    /// Evento attivato se il Leap Moton viene disconnesso
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void OnLeapDisconnect(object sender, DeviceEventArgs args)
    {
        //  stampa un messaggio di log
        Debug.Log("DISCONNESSO");
        Connected = false;
    }
    /*
    /// <summary>
    /// Rileva frame
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void OnFrame(object sender, FrameEventArgs args)
    {
        Debug.Log("Sono nell'onFrame del MS_LeapListener");
        Frame frame = args.frame;

        //  Se ci sono delle mani rilevate
        if(frame.Hands.Count > 1)
        {
            Debug.Log("Sono nell'if del MS_LeapListener");
            _GM.isActive = true;            //  deve suonare
            //  qui attivi

            //  per ogni mano, sceglie se è dx o sx, e stampa i seguenti dati
            //  DATI GENERICI:          dx o sx, tupla di coords (x,y,z) dal sensore leap, numero dita,
            //  ROTAZIONI relative:     rotazione su asse-x (hand pitch), rotazione su asse-z (hand roll), rotazione su asse-y (hand yaw)
            foreach (var hand in frame.Hands)
            {
                //  seleziona se è la mano destra

                if (hand.IsRight)
                    _GM.hand_R = hand;
                else if (hand.IsLeft)
                    _GM.hand_L = hand;
            }
            Debug.Log("hand_l: "+_GM.hand_L);
            Debug.Log("hand_l: " + _GM.hand_L);

        }
        else
        {
            Debug.Log("Sono nell'else del MS_LeapListener");
            //  non ci sono delle mani rilevate
            //  qui disattivi
            _GM.isActive = false;           //  non deve suonare
        }

    }

    */

    public static void onFrameDisp1(Frame f)
    {
       // Debug.Log("OnFrameDisp1");
        Frame frame = f;
        //  Se ci sono delle mani rilevate
        //per assicurarsi che ci siano entrambe le mani 
        if (frame.Hands.Count > 1) 
        {
            _GM.isActive = true;            //  deve suonare
                                            //  qui attivi

            //  per ogni mano, sceglie se è dx o sx, e stampa i seguenti dati
            //  DATI GENERICI:          dx o sx, tupla di coords (x,y,z) dal sensore leap, numero dita,
            //  ROTAZIONI relative:     rotazione su asse-x (hand pitch), rotazione su asse-z (hand roll), rotazione su asse-y (hand yaw)

            foreach (var hand in frame.Hands)
            {
                //  seleziona se è la mano destra
                if (hand.IsRight)
                    _GM.hand_R = hand;
                else if (hand.IsLeft)
                    _GM.hand_L = hand;
            }
          //  Debug.Log("_GM.hand_L: " + _GM.hand_L);
          //  Debug.Log("_GM.hand_R: " + _GM.hand_R);

        }
        else
        {
            //Debug.Log("Sono nell'else del MS_LeapController");
            //  non ci sono delle mani rilevate
            //  qui disattivi
            _GM.isActive = false;           //  non deve suonare
        }

    }

    public static void onFrameDisp2(Frame f)
    {
     //   Debug.Log("OnFrameDisp2");
        Frame frame = f;
        //  Se ci sono delle mani rilevate

        //per assicurarsi che ci siano entrambe le mani 
        //if (frame.Hands.Count > 1)
        if (frame.Hands.Count > 1)
        {
            _GM.isActive = true;            //  deve suonare
            //  qui attivi

            //  per ogni mano, sceglie se è dx o sx, e stampa i seguenti dati
            //  DATI GENERICI:          dx o sx, tupla di coords (x,y,z) dal sensore leap, numero dita,
            //  ROTAZIONI relative:     rotazione su asse-x (hand pitch), rotazione su asse-z (hand roll), rotazione su asse-y (hand yaw)

                foreach (var hand in frame.Hands)
                {
                    //  seleziona se è la mano destra
                    if (hand.IsRight)
                        _GM.secondDeviceHand_R = hand;
                    else if (hand.IsLeft)
                        _GM.secondDeviceHand_L = hand;
                }
              //  Debug.Log("secondDeviceHand_R: " + _GM.secondDeviceHand_R);
              //  Debug.Log("secondDeviceHand_L: " + _GM.secondDeviceHand_L);
            
        }
        else
        {
            //Debug.Log("Sono nell'else del MS_LeapController");
            //  non ci sono delle mani rilevate
            //  qui disattivi
            _GM.isActive = false;           //  non deve suonare
        }

    }





}
