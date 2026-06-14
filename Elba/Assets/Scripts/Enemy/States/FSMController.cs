using UnityEngine;

public class FSMController<T> : MonoBehaviour where T : FSMController<T>
{
    private States<T> currentState;

    protected virtual void Update()
    {
        currentState?.OnUpdate();
    }

    public void SetState(States<T> state)
    {
        if (currentState == state)
            return;

        currentState?.OnExit();

        currentState = state;

        currentState.OnEnter();
    }
}