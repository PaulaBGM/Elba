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
        Debug.Log(
            $"STATE CHANGE: " + $"{currentState?.GetType().Name ?? "NULL"} -> {state.GetType().Name}");

        currentState?.OnExit();

        currentState = state;

        currentState.OnEnter();
    }
}