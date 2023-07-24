using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouse : MonoBehaviour
{
    private Transform tf;
    private Camera cam;
    private float m_Vertical;
    private float m_Horizontal;
    public bool isLimit;
    public float verticalMaxLimit;
    public float verticalMinLimit;
    public float mouse_Horizontal_Sensitive;
    public float mouse_Vertical_Sensitive;
    public bool isMove;

    public Transform handTf;

    public void Init(Transform _transform, Camera camera, Transform head)
    {
        tf = _transform;
        cam = camera;
        cam.transform.SetParent(head, false);
        cam.transform.localPosition = Vector3.zero;
        isMove = true;
        
        handTf.transform.SetParent(cam.transform, false);
        handTf.transform.localEulerAngles = Vector3.zero;
        handTf.transform.localPosition = new Vector3(0.397f, -0.206f, 0.377f);
    }

    public void Repetition()
    {
        if(isMove)
        {
            m_Horizontal += Input.GetAxisRaw("Mouse X") * mouse_Horizontal_Sensitive;


            m_Vertical += Input.GetAxisRaw("Mouse Y") * mouse_Vertical_Sensitive;


            tf.eulerAngles = new Vector3(0, m_Horizontal, 0);


            if (m_Vertical > verticalMaxLimit)
            {
                m_Vertical = verticalMaxLimit;
            }
            else if (m_Vertical < verticalMinLimit)
            {
                m_Vertical = verticalMinLimit;
            }

            cam.transform.localEulerAngles = new Vector3(m_Vertical, 0, 0) * -1;
        }
    }


    public float GetHorizontal()
    {
        return m_Horizontal;
    }
    public float GetVertical()
    {
        return -m_Vertical;
    }
}
