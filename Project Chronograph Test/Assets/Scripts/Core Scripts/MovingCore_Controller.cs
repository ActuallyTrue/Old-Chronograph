﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement2D))]
public class MovingCore_Controller : MonoBehaviour {
    //MAKE SURE TO SET THE PASSENGERMASK TO WHATEVER YOU WANNA MOVE 13:51
    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;
    public float defaultSpeed;
    public float fastSpeed;
    public float slowSpeed;
    public bool cyclic;
    public float waitTime;
    [Range(0, 2)]
    public float easeAmount; //above 3, it's basically just stopping and starting so we're clamping it from 0 to 2
    int fromWaypointIndex = 0;
    float percentBetweenWaypoints;
    float nextMoveTime;
    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    private Vector3 oldPoint;
    [HideInInspector]
    private Vector3 currentPoint;
    [HideInInspector]
    public float currentXVelocity;
    [HideInInspector]
    public float currentYVelocity;

    Movement2D Movement;

    public enum CoreStates
    {
        Default = 0,
        SpedUp = 1,
        SlowedDown = 2
    }

    public CoreStates currentState;

    // Use this for initialization
    public void Start()
    {
        currentState = CoreStates.Default;
        oldPoint = transform.position;
        Movement = GetComponent<Movement2D>();
        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CoreStates.Default:
                currentPoint = transform.position;
                currentXVelocity = (transform.position.x - oldPoint.x) / Time.deltaTime;
                currentYVelocity = (transform.position.y - oldPoint.y) / Time.deltaTime;
                Movement.UpdateRaycastOrigins();

                velocity = Movement.CalculatePlatformMovement(defaultSpeed, ref fromWaypointIndex, ref percentBetweenWaypoints, ref globalWaypoints, cyclic, ref nextMoveTime, waitTime, easeAmount);
                Movement.CalculatePassengerMovement(velocity);
                Movement.MovePassengers(true);
                Movement.MovePlatform(velocity);
                Movement.MovePassengers(false);
                oldPoint = currentPoint;
                break;
            case CoreStates.SpedUp:
                currentPoint = transform.position;
                currentXVelocity = (transform.position.x - oldPoint.x) / Time.deltaTime;
                currentYVelocity = (transform.position.y - oldPoint.y) / Time.deltaTime;
                Movement.UpdateRaycastOrigins();

                velocity = Movement.CalculatePlatformMovement(fastSpeed, ref fromWaypointIndex, ref percentBetweenWaypoints, ref globalWaypoints, cyclic, ref nextMoveTime, waitTime, easeAmount);
                Movement.CalculatePassengerMovement(velocity);
                Movement.MovePassengers(true);
                Movement.MovePlatform(velocity);
                Movement.MovePassengers(false);
                oldPoint = currentPoint;
                break;
            case CoreStates.SlowedDown:
                currentPoint = transform.position;
                currentXVelocity = (transform.position.x - oldPoint.x) / Time.deltaTime;
                currentYVelocity = (transform.position.y - oldPoint.y) / Time.deltaTime;
                Movement.UpdateRaycastOrigins();

                velocity = Movement.CalculatePlatformMovement(slowSpeed, ref fromWaypointIndex, ref percentBetweenWaypoints, ref globalWaypoints, cyclic, ref nextMoveTime, waitTime, easeAmount);
                Movement.CalculatePassengerMovement(velocity);
                Movement.MovePassengers(true);
                Movement.MovePlatform(velocity);
                Movement.MovePassengers(false);
                oldPoint = currentPoint;
                break;

        }
        currentPoint = transform.position;
        currentXVelocity = (transform.position.x - oldPoint.x) / Time.deltaTime;
        currentYVelocity = (transform.position.y - oldPoint.y) / Time.deltaTime;
        Movement.UpdateRaycastOrigins();

        velocity = Movement.CalculatePlatformMovement(defaultSpeed, ref fromWaypointIndex, ref percentBetweenWaypoints, ref globalWaypoints, cyclic, ref nextMoveTime, waitTime, easeAmount);
        Movement.CalculatePassengerMovement(velocity);
        Movement.MovePassengers(true);
        Movement.MovePlatform(velocity);
        Movement.MovePassengers(false);
        oldPoint = currentPoint;
    }

    Vector3 GiveOldPosition(Vector3 position)
    {

        return position;
    }

    public void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                //so that the waypoints are drawn in the correct position when playing and when not playing
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.right * size, globalWaypointPos + Vector3.right * size);
            }
        }
    }
}

