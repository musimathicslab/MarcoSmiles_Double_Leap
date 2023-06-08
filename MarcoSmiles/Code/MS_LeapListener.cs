using UnityEngine;
using Leap;
using Leap.Unity;
using System.Collections.Generic;

/// <summary>
/// This class handle the connection between LeapMotion and Unity3d.
/// Useful to get hand objects from the device(s)
/// </summary>
public class MS_LeapListener : MonoBehaviour{
    public static bool Connected = false;

    /// <summary>
    /// Connection event of LeapMotion.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void OnLeapConnect(object sender, DeviceEventArgs args){
        Debug.Log("Connected");
        Connected = true;
    }

    /// <summary>
    /// Disconnection event of LeapMotion.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void OnLeapDisconnect(object sender, DeviceEventArgs args){
        Debug.Log("Disconnected");
        Connected = false;
    }

    /// <summary>
    /// Function to handle the frame of the first LeapMotion device.
    /// </summary>
    /// <param name="f">Frame received by the device</param>
    public static void onFrameDisp1(Frame f){
        Frame frame = f;

        // Check if both hands are detected by the LeapMotion device.
        Debug.Log(frame.Hands.Count);
        if (frame.Hands.Count > 1){
            _GM.isActive = true;  //Must play

            foreach (var hand in frame.Hands){
                // Check what kind of hand it is
                if (hand.IsRight)
                    _GM.hand_R = hand;
                else if (hand.IsLeft)
                    _GM.hand_L = hand;
            }
        }else{
            _GM.isActive = false;
        }
    }


    /// <summary>
    /// Function to handle the frame of the second LeapMotion device.
    /// </summary>
    /// <param name="f">Frame received by the device</param>
    public static void onFrameDisp2(Frame f){

        Frame frame = f;
        // Check if both hands are detected by the LeapMotion device.
        Debug.Log(frame.Hands.Count);
        if (frame.Hands.Count > 1){
            _GM.isActive = true; //Must play

                foreach (var hand in frame.Hands){
                    // Check what kind of hand it is
                    if (hand.IsRight)
                        _GM.secondDeviceHand_R = hand;
                    else if (hand.IsLeft)
                        _GM.secondDeviceHand_L = hand;
                }
        }else{
            _GM.isActive = false;
        }
    }
}
