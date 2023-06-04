using UnityEngine;

using Leap;

/// <summary>
/// Class that compute FF and NFA features.
/// </summary>
public class DatasetHandler : MonoBehaviour
{
    #region Getters

    /// <summary>
    /// Analyses the flexion of a finger.
    /// </summary>
    /// <param name="f">Finger to be analysed</param>
    /// <param name="isThumb">True if is Thumb, False otherwise</param>
    /// <returns></returns>
    public static float getFF(Finger f, bool isThumb = false){
        return (isThumb) ? grads(Vector3.Angle(f.bones[1].Direction, f.bones[3].Direction)) :
            grads(Vector3.Angle(f.bones[0].Direction, f.bones[3].Direction));
    }

    /// <summary>
    /// Analyses the angle between pairs of fingers.
    /// </summary>
    /// <param name="f1">Finger 1</param>
    /// <param name="f2">Finger 2</param>
    /// <returns></returns>
    public static float getNFA(Finger f1, Finger f2){
        return grads(Vector3.Angle(f1.Direction, f2.Direction));
    }

    /// <summary>
    /// Float to grads conversion.
    /// </summary>
    /// <param name="num">Value to convert/param>
    /// <returns>Value in gradients</returns>
    private static float grads(float num)
    {
        return num * 180.0f / Mathf.PI;
    }

}
