using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OscillateObject : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    Vector3[] waypointsDefinition = new Vector3[]
    {
        new Vector3(-8, 0, 0),
        new Vector3(8, 0, 0)
    };
    [SerializeField]
    float waypointPauseTime = 0.75f;

    [SerializeField]
    bool areWaypointsRelative = true;
    [SerializeField]
    bool recomputeWaypointsOnResume = false;

    private Vector3[] waypoints;
    private bool allowMovement = true;
    private float waypointCountdown = 0f;
    private int curWaypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        ComputeWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        if (allowMovement == true)
        {
            if (waypointCountdown > 0f)
            {
                waypointCountdown -= Time.deltaTime;
                return;
            }

            var newPosition = Vector3.MoveTowards(transform.position, waypoints[curWaypointIndex], Time.deltaTime * speed);

            transform.position = newPosition;
            if (transform.position == waypoints[curWaypointIndex])
            {
                curWaypointIndex = (curWaypointIndex + 1) % waypoints.Length;
                waypointCountdown = waypointPauseTime;
            }
        }
    }

    void StopMotion()
    {
        allowMovement = false;
        waypointCountdown = 0f;
    }

    void ResumeMotion()
    {
        if (recomputeWaypointsOnResume)
        {
            ComputeWaypoints();
        }
        allowMovement = true;
    }

    void ComputeWaypoints()
    {
        if (areWaypointsRelative)
        {
            waypoints = waypointsDefinition.Select(wd => wd + transform.position).ToArray();
        }
        else
        {
            waypoints = waypointsDefinition;
        }
    }
}
