/******************************************************************************
 * Copyright (C) Ultraleap, Inc. 2011-2022.                                   *
 *                                                                            *
 * Use subject to the terms of the Apache License 2.0 available at            *
 * http://www.apache.org/licenses/LICENSE-2.0, or another agreement           *
 * between Ultraleap and you, your company or other organization.             *
 ******************************************************************************/

using System.Collections;
using Leap.Unity.Interaction;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
    /// <summary>
    /// Questo codice allinea due dispositivi di Leap Motion, rappresentati dai provider sourceDevice e targetDevice, 
    /// in modo che le ossa delle mani dei due dispositivi siano allineate entro un certo margine di varianza definito da alignmentVariance. 
    /// La classe utilizza l'algoritmo di Kabsch per calcolare la matrice di trasformazione che allinea le ossa delle mani dei due dispositivi 
    /// e poi applica questa trasformazione al targetDevice. La funzione ReAlignProvider() reimposta la posizione del targetDevice a zero per 
    /// consentire una nuova allineamento. 
    /// Il metodo Update() viene chiamato ogni frame e allinea i dispositivi se la posizione non è ancora stata completata. 
    /// Per ogni mano nel sourceDevice, viene cercata una corrispondente mano nel targetDevice e se trovata, i punti delle ossa delle mani vengono raccolti 
    /// e utilizzati per calcolare la trasformazione di allineamento. Una volta che l'allineamento è stato completato, la posizione del targetDevice viene 
    /// impostata sulla posizione corretta e i punti delle ossa delle mani vengono cancellati per la prossima iterazione.
    /// </summary>
    
    public class MultideviceAlignment : MonoBehaviour
    {
        public LeapProvider sourceDevice;
        public LeapProvider targetDevice;

        [Tooltip("The maximum variance in bone positions allowed to consider the provider aligned. (in metres)")]
        public float alignmentVariance = 0.02f;

        List<Vector3> sourceHandPoints = new List<Vector3>();
        List<Vector3> targetHandPoints = new List<Vector3>();

        bool positioningComplete = false;

        KabschSolver solver = new KabschSolver();

        public void ReAlignProvider()
        {
            targetDevice.transform.position = Vector3.zero;
            positioningComplete = false;
        }

        void Update()
        {
            if (!positioningComplete)
            {
                foreach (var sourceHand in sourceDevice.CurrentFrame.Hands)
                {
                    var targetHand = targetDevice.CurrentFrame.GetHand(sourceHand.IsLeft ? Chirality.Left : Chirality.Right);

                    if (targetHand != null)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                sourceHandPoints.Add(sourceHand.Fingers[j].bones[k].Center);
                                targetHandPoints.Add(targetHand.Fingers[j].bones[k].Center);
                            }
                        }

                        // This is temporary while we check if any of the hands points are not close enough to eachother
                        positioningComplete = true;

                        for (int i = 0; i < sourceHandPoints.Count; i++)
                        {
                            if (Vector3.Distance(sourceHandPoints[i], targetHandPoints[i]) > alignmentVariance)
                            {
                                // we are already as aligned as we need to be, we can exit the alignment stage
                                positioningComplete = false;
                                break;
                            }
                        }

                        if (positioningComplete)
                        {
                            return;
                        }

                        Matrix4x4 deviceToOriginDeviceMatrix =
                          solver.SolveKabsch(targetHandPoints, sourceHandPoints, 200);

                        // transform the targetDevice.transform by the deviceToOriginDeviceMatrix
                        Matrix4x4 newTransform = deviceToOriginDeviceMatrix * targetDevice.transform.localToWorldMatrix;
                        targetDevice.transform.position = newTransform.GetVector3();
                        targetDevice.transform.rotation = newTransform.GetQuaternion();
                        targetDevice.transform.localScale = Vector3.Scale(targetDevice.transform.localScale, deviceToOriginDeviceMatrix.lossyScale);


                        targetHandPoints.Clear();
                        sourceHandPoints.Clear();
                        return;
                    }
                }
            }
        }
    }
}