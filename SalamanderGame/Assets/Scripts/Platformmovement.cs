using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformmovement : MonoBehaviour {
// platform initial position
    private Vector3 posA;
    //platform final position
    private Vector3 posB;
    private Vector3 nextPos;
    //platform movement speed
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform childTrans;
    [SerializeField]
    private Transform transformB;


    // Use this for initialization
    void Start ()
    {
        //assign position A and B to the transforms
        posA = childTrans.localPosition;
        posB = transformB.localPosition;
        nextPos = posB;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();

	}

    private void Move()
        //makes the child move to the next position
    {
        childTrans.localPosition = Vector3.MoveTowards(childTrans.localPosition, nextPos, speed * Time.deltaTime);
        // once the platform reachs a destination call function ChangeDestination()
        if (Vector3.Distance(childTrans.localPosition, nextPos) <= 0.1)
        {
            ChangeDestination();
        }
    }

    private void ChangeDestination()

        // next position is equal A or B depending on the current position
    {
        nextPos = nextPos != posA ? posA : posB;
    }
}


