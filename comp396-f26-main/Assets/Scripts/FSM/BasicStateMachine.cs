using UnityEngine;
using UnityEngine.InputSystem;

public enum BasicStates
{
    Patrolling, Harvesting, Resting
}

[RequireComponent(typeof(MeshRenderer))]
public class BasicStateMachine : MonoBehaviour
{
    private BasicStates currentState;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        currentState = BasicStates.Resting;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void ChangeState(BasicStates newState)
    {
        if (currentState == newState) { return; }

        currentState = newState;
    }

    private void Update()
    {
        switch (currentState) {
            case BasicStates.Patrolling:
                meshRenderer.material.color = Color.yellow;
                break;
            case BasicStates.Harvesting:
                meshRenderer.material.color = Color.green;
                break;
            case BasicStates.Resting:
                meshRenderer.material.color = Color.cyan;
                // Apply the Behaviour for this State here.
                break;
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            ChangeState(BasicStates.Patrolling);
        }
        if(Keyboard.current.hKey.wasReleasedThisFrame)
        {  ChangeState(BasicStates.Harvesting);}
        if (Keyboard.current.rKey.wasPressedThisFrame) { 
            ChangeState(BasicStates.Resting);
        }
    }
}
