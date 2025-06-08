using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowTarget : MonoBehaviour
{

    private Transform[] targets;

    void Update()
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("ArrowTarget");
        targets = new Transform[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            targets[i] = targetObjects[i].transform;
        }
        
        Transform closestTarget = GetClosestTarget();
        if (closestTarget != null)
        {
            transform.LookAt(closestTarget);
            transform.Rotate(90f, -90f, 0);
        }
    }

    Transform GetClosestTarget()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        if (targets.Length == 0)
        {
            gameObject.SetActive(false);
            return null;
        }

        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(currentPosition, target.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target;
            }
        }

        return closest;
    }
}
