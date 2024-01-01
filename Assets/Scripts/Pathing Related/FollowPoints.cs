using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowPoints : MonoBehaviour
{
    private Vector2[] pointsToFollow;
    private Vector2 currentTargetPos;
    private int currentTargetIndex;

    public void SetPath(Vector2[] path)
    {
        pointsToFollow = path;
    }

    public void PlaceObjectOnPoint(Transform movingTransform, int pointIndex, Action OnChangeDir)
    {
        movingTransform.position = pointsToFollow[pointIndex];
        SetLocalUpDirection(movingTransform, pointsToFollow[pointIndex + 1]);
        currentTargetIndex = pointIndex + 1;
        OnChangeDir();
    }

    //sets the local up of the given Transform
    public void SetLocalUpDirection(Transform movingTransform, Vector2 targetPos)
    {
        Vector2 targetDir = targetPos - (Vector2)movingTransform.position;
        movingTransform.up = targetDir;
        currentTargetPos = targetPos;
    }

    //moves the given Transform upwards
    public void MoveLocalUp(Transform movingTransform, float moveSpeed, Action OnChangeDir, Action OnReachEnd)
    {
        movingTransform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        //if reached target position
        if (movingTransform.InverseTransformPoint(movingTransform.position).y > movingTransform.InverseTransformPoint(currentTargetPos).y)
        {
            if (currentTargetIndex + 1 < pointsToFollow.Length)
            {
                PlaceObjectOnPoint(movingTransform, currentTargetIndex, OnChangeDir);
            }
            else
            {
                OnReachEnd();
            }
        }
    }
}
