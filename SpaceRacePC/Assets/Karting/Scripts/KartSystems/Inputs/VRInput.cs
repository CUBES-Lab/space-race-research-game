using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// A basic keyboard implementation of the IInput interface for all the input information a kart needs.
    /// </summary>
    public class VRInput : MonoBehaviour, IInput
    {
        public float Acceleration
        {
            get { return m_Acceleration; }
        }
        public float Steering
        {
            get { return m_Steering; }
        }
        public bool BoostPressed
        {
            get { return m_BoostPressed; }
        }
        public bool FirePressed
        {
            get { return m_FirePressed; }
        }
        public bool HopPressed
        {
            get { return m_HopPressed; }
        }
        public bool HopHeld
        {
            get { return m_HopHeld; }
        }

        float m_Acceleration;
        float m_Steering;
        bool m_HopPressed;
        bool m_HopHeld;
        bool m_BoostPressed;
        bool m_FirePressed;

        Camera m_Camera;
        bool m_FixedUpdateHappened;

        public float minTurnThreshold = 0.05f;
        public float steeringFactor = 1;
        public float steerAdditive = 0.3f;
        //Vector3 camLeft;
        Vector3 camRight;
        void Start()
        {
            m_Acceleration = 1f;
            m_Camera = Camera.main;
            camRight = m_Camera.transform.right;
        }

        void Update()
        {
            float rot_angle = Vector3.Dot(m_Camera.transform.right, -transform.up);
            float rot_z = Mathf.Abs(rot_angle) > minTurnThreshold ? rot_angle : 0f;
            
            
            if (rot_z > 0)
            {   
                m_Steering = Mathf.Min(1f, rot_z + steerAdditive);
            }
            else if (rot_z < 0)
            {
                m_Steering = Mathf.Max(-1f, rot_z - steerAdditive);
            }
            else
            {
                m_Steering = 0f;
            }
            m_Steering *= steeringFactor;
        }
    }
}