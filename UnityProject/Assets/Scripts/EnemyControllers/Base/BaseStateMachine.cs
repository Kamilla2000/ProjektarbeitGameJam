using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{
    public BaseState CurrentState { get; protected set; }


    void Start()
    {
        Initialize();    
    }

    void Update()
    {        
        CurrentState?.OnUpdateState(this);
        Tick();
    }

    public void SwitchToState(BaseState nextState) 
    {
        CurrentState?.OnExitState(this);
        CurrentState = nextState;
        CurrentState.OnEnterState(this);
    }

    /// <summary>
    /// Called when statemachine started
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Called every frame 
    /// </summary>
    public abstract void Tick(); 
}
