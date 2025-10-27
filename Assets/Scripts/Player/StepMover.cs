using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepMover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    private Transform[] steps;
    private int currentStep = 0;
    private bool moving = false;

    public void SetStepsFromPath(GameObject path)
    {
        int count = path.transform.childCount;
        steps = new Transform[count];
        for (int i = 0; i < count; i++)
            steps[i] = path.transform.GetChild(i);
    }

    public void MoveUpOneStep()
    {
        if (moving || steps == null || currentStep >= steps.Length - 1) return;
        currentStep++;
        StartCoroutine(MoveToStep(steps[currentStep]));
    }

    IEnumerator MoveToStep(Transform target)
    {
        moving = true;
        Vector3 start = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(start, target.position, t);
            yield return null;
        }

        moving = false;
    }
}
