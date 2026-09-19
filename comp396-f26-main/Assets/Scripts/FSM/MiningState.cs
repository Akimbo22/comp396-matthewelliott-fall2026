using UnityEngine;
using UnityEngine.AI;

namespace Core.FSM
{
    /*
     * This state is based on another activity in Stardew Valley, where you can
     * mine for minerals and fight enemies. From this state, you can rest and patrol.
     * In my mind, I was thinking the patrol could be used to check for enemies, while
     * resting would be restoring or saving stamina. You can transistion to this state from the
     * resting state.
     * 
     * I used navmesh for this as well, however, the NPC heads towards a different tag, which is
     * the caves.
     */
    public class MiningState : BaseState
    {
        private GameObject caves;

        public MiningState(MeshRenderer renderer, NavMeshAgent agent) : base(renderer, agent)
        {

        }

        public override void Enter()
        {
            meshRenderer.material.color = Color.red; // I went with red, to match gems like rubies
            caves = GameObject.FindWithTag("Caves");
            agent.SetDestination(caves.transform.position);
            agent.isStopped = false;
        }
        public override void Exit()
        {
            base.Exit();
            caves = null;
        }
    }
}