using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;
    public float m_Speed = 5f;
    public float jumpPower;
    public LayerMask mask;

    public void Init(Rigidbody rigidbody)
    {
        rb = rigidbody; 
    }

    public void Repetition()        //update
    {
        MovePosi();
        Jump();
    }

    public void MovePosi()
    {
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        m_Input = rb.transform.TransformDirection(m_Input);

        rb.MovePosition(rb.transform.position + m_Input * Time.deltaTime * m_Speed);
    }

    public void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + transform.up * -1, new Vector3(0.1f, 0.025f, 0.1f), Quaternion.identity, mask);
            if (colliders.Length > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(transform.up * jumpPower);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + transform.up * -1, new Vector3(0.2f, 0.05f, 0.2f));
    }
}
