using UnityEngine;

public static class TutorialManager
{
    public static bool HasSeen(string id)
    {
        return PlayerPrefs.GetInt(id, 0) == 1;
    }

    public static void MarkSeen(string id)
    {
        PlayerPrefs.SetInt(id, 1);
        PlayerPrefs.Save();
    }

    public static void ResetTutorials()
    {
        PlayerPrefs.DeleteAll();
    }
}