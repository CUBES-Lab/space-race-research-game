using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace KartGame.KartSystems
{
    
    public class MobileInput : MonoBehaviour, IInput
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

        float m_Acceleration = 1f;
        float m_Steering;
        bool m_HopPressed;
        bool m_HopHeld;
        bool m_BoostPressed;
        bool m_FirePressed;
        public float rotationMod = 2.3f;

        bool m_FixedUpdateHappened;

        void Update()
        {

            m_Steering = Input.acceleration.x * rotationMod;

            m_HopHeld = Input.GetButton("Hop");

            if (m_FixedUpdateHappened)
            {
                m_FixedUpdateHappened = false;

                m_HopPressed = false;
                m_BoostPressed = false;
                m_FirePressed = false;
            }

            m_HopPressed |= Input.GetButtonDown("Hop");
            m_BoostPressed |= Input.GetButtonDown("Boost");
            m_FirePressed |= Input.GetButtonDown("Fire");
        }

        void FixedUpdate()
        {
            m_FixedUpdateHappened = true;
        }
    }
}