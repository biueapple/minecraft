using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;
    public float m_Speed = 5f;

    public void Init(Rigidbody rigidbody)
    {
        rb = rigidbody; 
    }

    public void Repetition()        //update
    {
        MovePosi();
    }

    public void MovePosi()
    {
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        m_Input = rb.transform.TransformDirection(m_Input);

        rb.MovePosition(rb.transform.position + m_Input * Time.deltaTime * m_Speed);
    }

}
