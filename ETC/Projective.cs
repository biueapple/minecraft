using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projective : MonoBehaviour
{
    private Unit user;
    private float range;
    private Vector3 startpoint;
    private Action<Unit, Transform> action;
    private Coroutine coroutine;
    private float speed;

    public void MoveToUnit(Unit user, float range, Vector3 angle, Action<Unit, Transform> action, float speed)
    {
        this.user = user;
        this.range = range;
        this.action = action;
        this.speed = speed;
        startpoint = transform.position = user.transform.position + user.transform.forward;
        transform.localEulerAngles = angle;
        if(coroutine != null )
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(MoveTo());
    }

    private IEnumerator MoveTo()
    {
        while(Vector3.Distance(startpoint, transform.position) <= range)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);

            yield return null;
        }
        coroutine = null;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Unit>() != null &&  other.transform.GetComponent<Unit>() != user)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                action(other.transform.GetComponent<Unit>(), transform);
                Destroy(gameObject);
            }
        }
    }
}
