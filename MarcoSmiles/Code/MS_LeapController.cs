using UnityEngine;
using Leap;
using Leap.Unity;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class handle the connection between LeapMotion and Unity3d.
/// </summary>
public class MS_LeapController : MonoBehaviour{

    /// <summary>
    /// LeapMotion controller.
    /// </summary>
    Controller controller;

    /// <summary>
    /// Listener.
    /// </summary>
    MS_LeapListener listener;

    /// <summary>
    /// Flag to keep track of the LeapMotion connection.
    /// </summary>
    bool connected = false;

    /// <summary>
    /// Flag to keep track of the notification (not on screen) of the leapMotion connection.
    /// </summary>
    bool notified = false;

    /// <summary>
    /// Flag to keep track of the pop-up notification that the LeapMotion is not connected.
    /// </summary>
    bool notConnectedShowed = true;

    public LeapProvider leapProvider1;
    public LeapProvider leapProvider2;


    // Start is called before the first frame update
    void Start()
    {
        //  Initialize new Leap controller
        controller = new Controller();

        // Add a device when it connects
        controller.Device += MS_LeapListener.OnLeapConnect;
        _GM.ShowConnectLeapPopup();
    }

    // Update is called once per frame
    void Update(){
        if(controller.Devices.Count <=1  && !notConnectedShowed){
            notified = false;
            notConnectedShowed = true;
            MS_LeapListener.Connected = false;
            _GM.ShowConnectLeapPopup();
        }

        // Check if there are two LeapMotion devices connected
        if (controller.Devices.Count >= 2 && leapProvider1 != null && leapProvider2 != null ){
            Frame frame1 = leapProvider1.CurrentFrame;
            Frame frame2 = leapProvider2.CurrentFrame;

            // Get hands objects
            MS_LeapListener.onFrameDisp1(frame1);
            MS_LeapListener.onFrameDisp2(frame2);
        }

        if (MS_LeapListener.Connected && !notified)
        {
            notified = true;
            notConnectedShowed = false;
            _GM.HideConnectLeapPopup();
        }
    }
}
