using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSystem : MonoBehaviour
{
    public Transform[] points;
    public Transform obj;
    public float speed;
    public bool cycle;

    Transform targetPoint;
    int currentPoint;
    bool isMoveForward;
    void Start()
    {
        currentPoint = 0;
        targetPoint = points[currentPoint];
        isMoveForward = true;
    }

    void Update()
    {
        if (obj.position == targetPoint.position)
        {
            if (!isMoveForward && cycle) isMoveForward = true;
            if (currentPoint == points.Length - 1 && !cycle) isMoveForward = false;
            else if (currentPoint == 0 && !cycle) isMoveForward = true;

            if (isMoveForward) currentPoint++;
            else currentPoint--;

            if (currentPoint >= points.Length && cycle)
            {
                currentPoint = 0;
            }

            targetPoint = points[currentPoint];
        }
        obj.position = Vector3.MoveTowards(obj.position, targetPoint.position, speed * Time.deltaTime);
    }
}
