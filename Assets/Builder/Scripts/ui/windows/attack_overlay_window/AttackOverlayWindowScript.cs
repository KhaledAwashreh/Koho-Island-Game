using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackOverlayWindowScript : WindowScript
{
    public static AttackOverlayWindowScript instance;

    void Awake()
    {
        instance = this;
    }

    public Text SwordManCounter;
    public Text ArcherCounter;


    public void OnClickHomeGoButton()
    {
        SceneManager.instance.EnterNormalMode();
    }

    public void OnClickSwordManButton()
    {
        SceneManager.instance.selectedUnit = 0;
    }

    public void OnClickArcherButton()
    {
        SceneManager.instance.selectedUnit = 1;
    }
}