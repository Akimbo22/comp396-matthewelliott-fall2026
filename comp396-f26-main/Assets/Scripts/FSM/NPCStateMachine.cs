using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Core.FSM
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCStateMachine : MonoBehaviour
    {
        StateMachine stateMachine;

        private void Awake()
        {
            // Creation of the StateMachine
            stateMachine = new StateMachine();
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            NavMeshAgent agent = GetComponent<NavMeshAgent>();

            // Create instances for concrete stateNodes
            PatrolState patrol = new PatrolState(renderer, agent, new GameObject[2]);
            HarvestState harvest = new HarvestState(renderer, agent);
            RestState rest = new RestState(renderer, agent);
            PlottingState plotting = new PlottingState(renderer, agent);
            MiningState mining = new MiningState(renderer, agent);
            //FishingState fishing = new FishingState(renderer);

            stateMachine.AddTransition(rest, patrol, new FuncPredicate(() => Keyboard.current.pKey.wasPressedThisFrame));
            stateMachine.AddTransition(rest, harvest, new FuncPredicate(() => Keyboard.current.hKey.wasPressedThisFrame));

            stateMachine.AddTransition(harvest, rest, new FuncPredicate(() => Keyboard.current.rKey.wasPressedThisFrame));
            stateMachine.AddTransition(patrol, rest, new FuncPredicate(() => Keyboard.current.rKey.wasPressedThisFrame));

            // I went with F key for plotting transitions, since it can stand for "Farming"
            // M for Mining, which is... self-explanatory

            // once you finish harvesting crops, you plant new ones
            stateMachine.AddTransition(harvest, plotting, new FuncPredicate(() => Keyboard.current.fKey.wasPressedThisFrame));
            // Harvest any crops that may be done after plotting new seeds
            stateMachine.AddTransition(plotting, harvest, new FuncPredicate(() => Keyboard.current.hKey.wasPressedThisFrame));
            // Wait for the newly planted seeds to bloom
            stateMachine.AddTransition(plotting, rest, new FuncPredicate(() => Keyboard.current.rKey.wasPressedThisFrame));
            // Head to the mines or end your break in the mines
            stateMachine.AddTransition(rest, mining, new FuncPredicate(() => Keyboard.current.mKey.wasPressedThisFrame));
            // Take a break from mining to converse or recover stamina
            stateMachine.AddTransition(mining, rest, new FuncPredicate(() => Keyboard.current.rKey.wasPressedThisFrame));
            // Look for enemies in the mines to fight
            stateMachine.AddTransition(mining, patrol, new FuncPredicate(() => Keyboard.current.pKey.wasPressedThisFrame));

            stateMachine.SetState(rest);
        }

        private void Update()
        {
            stateMachine.Update();
        }
    }
}
