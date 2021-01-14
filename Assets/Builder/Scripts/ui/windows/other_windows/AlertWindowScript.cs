using UnityEngine.UI;

public class AlertWindowScript : WindowScript
{
    public static AlertWindowScript instance;

    void Awake()
    {
        instance = this;
    }

    public Text alertTitle;
    public Text alertContent;
}