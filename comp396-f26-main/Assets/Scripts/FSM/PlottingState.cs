using UnityEngine;
using UnityEngine.AI;

namespace Core.FSM
{
    /* This state is meant to work in conjunction with the Harvesting State
     * I included the same navmesh setup from Harvesting State, since the same
     * plot would be used for both states.
     * From this state, you would be able to transition to a resting state
     * or to the harvesting state.
     * You can only enter it from the Harvesting State, as you would plant new
     * seeds after harvesting the previous ones
     */
    public class PlottingState : BaseState
    {
        private GameObject harvestingPlot;
        public PlottingState(MeshRenderer renderer, NavMeshAgent agent) : base(renderer, agent)
        {

        }

        public override void Enter()
        {
            meshRenderer.material.color = Color.lightGreen; // I felt a different shade of green would show its connection with the harvesting state
            harvestingPlot = GameObject.FindWithTag("HarvestingPlot");
            agent.SetDestination(harvestingPlot.transform.position);
            agent.isStopped = false;
        }
        public override void Exit()
        {
            base.Exit();
            harvestingPlot = null;
        }
    }
}