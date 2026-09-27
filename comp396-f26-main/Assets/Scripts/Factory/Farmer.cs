using Core.FSM;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Farmer : MonoBehaviour, IEntity, IStateMachine, ITask
{
    StateMachine stateMachine;
    [SerializeField] private VoidEventChannel harvestEvent;
    [SerializeField] private bool isHarvestReady;

    private void Awake()
    {
        // Creation of the StateMachine
        stateMachine = new StateMachine();
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        // Create instances for concrete stateNodes
        HarvestState harvest = new HarvestState(renderer, agent);
        RestState rest = new RestState(renderer, agent);
        PlottingState plotting = new PlottingState(renderer, agent);

        stateMachine.AddTransition(rest, harvest, new FuncPredicate(() => isHarvestReady));

        stateMachine.AddTransition(harvest, rest, new FuncPredicate(() => !isHarvestReady));

        // I went with F key for plotting transitions, since it can stand for "Farming"
        // M for Mining, which is... self-explanatory

        // once you finish harvesting crops, you plant new ones
        stateMachine.AddTransition(harvest, plotting, new FuncPredicate(() => !isHarvestReady));
        // Harvest any crops that may be done after plotting new seeds
        stateMachine.AddTransition(plotting, harvest, new FuncPredicate(() => isHarvestReady));
        // Wait for the newly planted seeds to bloom
        stateMachine.AddTransition(plotting, rest, new FuncPredicate(() => Keyboard.current.rKey.wasPressedThisFrame));

        stateMachine.SetState(rest);

        harvestEvent.OnEventRaised += TransitionToHarvest;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void OnDisable()
    {
        harvestEvent.OnEventRaised -= TransitionToHarvest;
    }

    //private bool CheckHarvestPoint(Vector3 harvestLocation)
    //{

    //    return isHarvestReady;
    //}

    private void TransitionToHarvest()
    {
        isHarvestReady = !isHarvestReady;
    }
}
