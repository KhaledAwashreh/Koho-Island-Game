/* **************************************************************************
 * UTILITIES
 * **************************************************************************
 * Written by: Coppra Games
 * Created: June 2017
 * *************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Utilities : MonoBehaviour
{

    public static GameObject CreateInstance(GameObject original, GameObject parent, bool isActive)
    {
        GameObject instance = MonoBehaviour.Instantiate(original, parent.transform, false);
        instance.SetActive(isActive);
        return instance;
    }

    public static RenderQuadScript CreateRenderQuad()
    {
        return CreateInstance(SceneManager.instance.RenderQuad, SceneManager.instance.ItemsContainer, true).GetComponent<RenderQuadScript>();
    }

    public static float ClockwiseAngleOf3Points(Vector2 A, Vector2 B, Vector2 C)
    {

        Vector2 v1 = A - B;
        Vector2 v2 = C - B;

        var sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x) * -1;
        float angle = Vector2.Angle(v1, v2) * sign;

        if (angle < 0)
        {
            angle = 360 + angle;
        }

        return angle;
    }

    public static Vector2 GetScreenPosition(Vector3 position)
    {
        Vector3 screenPos = CameraManager.instance.MainCamera.WorldToScreenPoint(position);
        return screenPos;
    }

    public static string CleanStringForFloat(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";
        
        if (Regex.Match(input, @"^-?[0-9]*(?:\.[0-9]*)?$").Success)
            return input;
        else
        {
            //Debug.Log("Error, Bad Float: " + input);
            return "0";
        }
    }

    public static string CleanStringForInt(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";
        
        if (Regex.Match(input, "([-+]?[0-9]+)").Success)
            return input;
        else
        {
            //Debug.Log("Error, Bad Int: " + input);
            return "0";
        }
    }
}
