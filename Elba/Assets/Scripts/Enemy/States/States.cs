using System.Collections;
using UnityEngine;

public abstract class States<T> : MonoBehaviour where T : FSMController<T>
{
    protected T _controller;

    public virtual void InitController(T controller)
    {
        _controller = controller;
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}