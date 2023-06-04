using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Testing class, to get features.
/// </summary>
public class TestingScript{

    /// <summary>
    /// Select current features.
    /// </summary>
    /// <returns>Current features</returns>
    public static float[] GetCurrentFeatures(){

        float[] current_features = new float[36];

        // First device
        var left_hand = new DataToStore(
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

        var right_hand = new DataToStore(
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

        // Second device
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

        current_features[0] = left_hand.FF1;
        current_features[1] = left_hand.FF2;
        current_features[2] = left_hand.FF3;
        current_features[3] = left_hand.FF4;
        current_features[4] = left_hand.FF5;
        current_features[5] = left_hand.NFA1;
        current_features[6] = left_hand.NFA2;
        current_features[7] = left_hand.NFA3;
        current_features[8] = left_hand.NFA4;

        current_features[9] = right_hand.FF1;
        current_features[10] = right_hand.FF2;
        current_features[11] = right_hand.FF3;
        current_features[12] = right_hand.FF4;
        current_features[13] = right_hand.FF5;
        current_features[14] = right_hand.NFA1;
        current_features[15] = right_hand.NFA2;
        current_features[16] = right_hand.NFA3;
        current_features[17] = right_hand.NFA4;

        current_features[18] = left_hand2.FF1;
        current_features[19] = left_hand2.FF2;
        current_features[20] = left_hand2.FF3;
        current_features[21] = left_hand2.FF4;
        current_features[22] = left_hand2.FF5;
        current_features[23] = left_hand2.NFA1;
        current_features[24] = left_hand2.NFA2;
        current_features[25] = left_hand2.NFA3;
        current_features[26] = left_hand2.NFA4;

        current_features[27] = right_hand2.FF1;
        current_features[28] = right_hand2.FF2;
        current_features[29] = right_hand2.FF3;
        current_features[30] = right_hand2.FF4;
        current_features[31] = right_hand2.FF5;
        current_features[32] = right_hand2.NFA1;
        current_features[33] = right_hand2.NFA2;
        current_features[34] = right_hand2.NFA3;
        current_features[35] = right_hand2.NFA4;

        return current_features;
    }
}
