using UnityEngine;

public class PlayerDeathAnimation : MonoBehaviour
{
    public void OnDeathAnimationFinished()
    {
        GameOverController.Instance?.Show();
    }
}