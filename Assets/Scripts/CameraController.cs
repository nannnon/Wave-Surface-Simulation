using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 m_lookAtPos;
    float m_theta = Mathf.PI / 4;
    float m_phi = Mathf.PI / 4;
    float m_radius = 50;

    // Start is called before the first frame update
    void Start()
    {
        m_lookAtPos = new Vector3(
            WaveSurfaceController.kWidth * WaveSurfaceController.kGap / 2f,
            0,
            WaveSurfaceController.kHeight * WaveSurfaceController.kGap / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        {
            float omega = Time.deltaTime;

            if (Input.GetKey(KeyCode.W))
            {
                if (m_phi + omega < Mathf.PI / 2)
                {
                    m_phi += omega;
                }
                else
                {
                    m_phi = Mathf.PI / 2 - 0.01f;
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (m_phi - omega > -Mathf.PI / 2)
                {
                    m_phi -= omega;
                }
                else
                {
                    m_phi = -Mathf.PI / 2 + 0.01f;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                m_theta -= omega;
            }
            if (Input.GetKey(KeyCode.D))
            {
                m_theta += omega;
            }
        }

        {
            float dr = 20 * Time.deltaTime;

            if (Input.GetKey(KeyCode.E))
            {
                m_radius += dr;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                if (m_radius - dr >= 0.01f)
                {
                    m_radius -= dr;
                }
                else
                {
                    m_radius = 0.01f;
                }
            }
        }


        Vector3 pos;
        pos.x = m_radius * Mathf.Cos(m_theta) * Mathf.Cos(m_phi);
        pos.y = m_radius * Mathf.Sin(m_phi);
        pos.z = m_radius * Mathf.Sin(m_theta) * Mathf.Cos(m_phi);

        transform.position = m_lookAtPos + pos;

        transform.LookAt(m_lookAtPos);
    }
}
