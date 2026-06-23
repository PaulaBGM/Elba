using System;

[Serializable]
public class ActionData
{
    public string actionName;
    public string keyText;

    public ActionData(string actionName, string keyText)
    {
        this.actionName = actionName;
        this.keyText = keyText;
    }
}