// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
// Dependencies: SingletonBehaviours
//----------------------------------

using BWolf.Behaviours.SingletonBehaviours;
using BWolf.Utilities.AgentCommands;
using UnityEngine;

namespace BWolf.Examples.AgentCommands
{
    public class CommandCenter : SingletonBehaviour<CommandCenter>
    {
        [SerializeField]
        private LayerMask mask;

        [SerializeField]
        private Agent agent = null;

        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, mask.value))
                {
                    switch (LayerMask.LayerToName(hit.transform.gameObject.layer))
                    {
                        case "Terrain":
                            CreateMoveCommand(hit.point);
                            break;

                        case "Obstacle":
                            CreatePickupCommand(hit);
                            break;

                        default:
                            break;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                agent.Undo();
            }
        }

        private void CreateMoveCommand(Vector3 movePosition)
        {
            agent.Command(new MoveCommand(agent, movePosition), true);
        }

        private void CreatePickupCommand(RaycastHit hit)
        {
            PickupableObstacle obstacle = hit.transform.GetComponent<PickupableObstacle>();
            if (obstacle != null)
            {
                agent.Command(new PickupCommand(agent, obstacle), true);
            }
        }
    }
}