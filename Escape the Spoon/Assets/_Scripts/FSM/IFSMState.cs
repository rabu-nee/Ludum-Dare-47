
public interface IFSMState<T> {
    /// <summary>
    /// Called when a new state is assigned - e.g. use to initialize variables
    /// </summary>
    /// <param name="entity"></param>
    void Enter(T entity);

    /// <summary>
    /// search for reason to change states
    /// </summary>
    /// <param name="entity"></param>
    void Reason(T entity);

    /// <summary>
    /// Do what should be done within the state
    /// </summary>
    /// <param name="entity"></param>
    void Update(T entity);

    /// <summary>
    /// Called when a new state is assigned - e.g. use to wrap up/finish things
    /// </summary>
    /// <param name="entity"></param>
    void Exit(T entity);
}

// Another possibility would be to use an abstract class. Like so:
//public abstract class FSMState<T>
//{
//  abstract public void Enter(T entity);
//  abstract public void Reason(T entity);
//  abstract public void Update(T entity);
//  abstract public void Exit(T entity);
//}
//
// This would be suitable if there was any internal state in the form of variables, which all FSMStates 
// would share. We could, e.g., store an owner instead of passing an entity into every method. 
