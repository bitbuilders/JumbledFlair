using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public GameObject Target = null;
    [SerializeField, Range(0.0f, 20.0f)] float m_followSpeed = 4.0f;
    
    private void Update()
    {
        Vector3 target = Target.transform.position + Vector3.back * 10.0f;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * m_followSpeed);
    }
}
