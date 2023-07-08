using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projective : MonoBehaviour
{
    private Unit unit;
    private Action<Unit> action;
    private Coroutine coroutine;
    private float speed;

    public void MoveToUnit(Unit target, Action<Unit> action, float speed)
    {
        unit = target;
        this.action = action;
        this.speed = speed;
        if(coroutine != null )
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(MoveTo());
    }

    private IEnumerator MoveTo()
    {
        while(unit != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, unit.transform.position, speed * Time.deltaTime);

            if(!unit.gameObject.activeSelf)
            {
                break;
            }

            yield return null;
        }
        coroutine = null;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Unit>() != null &&  other.transform.GetComponent<Unit>() == unit)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                action(unit);
                Destroy(gameObject);
            }
        }
    }
}
