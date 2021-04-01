using UnityEngine;
using System.Collections;

namespace Pathfinding {
	[UniqueComponent(tag = "ai.destination")]
	public class Destination : VersionedMonoBehaviour {
		IAstarAI ai;
        private Transform[] targets = new Transform[10];
        private int currentPoint = 0;
		public Transform destination;
		GameObject checkpointHolder;

        void Start() {
			if(TogglePath.shortestPath == false) 
				checkpointHolder = GameObject.Find("SafestPath");
			else if(TogglePath.shortestPath == true) 
				checkpointHolder = GameObject.Find("ShortestPath");
			
			for(int i = 0; i < 10; i++)
				targets[i] = checkpointHolder.transform.Find(i.ToString());
        }

		void OnEnable () {
			ai = GetComponent<IAstarAI>();
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		void Update () {
			if(currentPoint < 10) {
				ai.destination = targets[currentPoint].position; // Sets the current checkpoint
				Vector3 targetPos = new Vector3(targets[currentPoint].position.x, transform.position.y, targets[currentPoint].position.z); // Stores the checkpoints position
				//Changes the current checkpoint
				if((transform.position).Round(1) == targetPos.Round(1)) currentPoint++;
			} else {
				ai.destination = destination.position;
			}
		}
	}
}
