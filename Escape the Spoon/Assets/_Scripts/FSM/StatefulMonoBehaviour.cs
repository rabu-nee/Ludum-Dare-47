using UnityEngine;

public class StatefulMonoBehaviour<T> : MonoBehaviour {
    protected FSM<T> fsm;

	public void ChangeState(IFSMState<T> e) {
		fsm.ChangeState(e);
	}

    protected virtual void Update() {
        // do nothing but update the internal state machine 
        // (delegates to the current state)
		fsm.Update();
	}
}
