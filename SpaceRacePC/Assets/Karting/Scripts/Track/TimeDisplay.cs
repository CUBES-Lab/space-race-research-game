﻿using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KartGame.Track
{
    /// <summary>
    /// A class to display information about a particular racer's timings.  WARNING: This class uses strings and creates a small amount of garbage each frame.
    /// </summary>
    public class TimeDisplay : MonoBehaviour
    {
        /// <summary>
        /// The different information that can be displayed on screen.
        /// </summary>
        public enum DisplayOptions
        {
            /// <summary>
            /// Displays the total time of the current race.
            /// </summary>
            Race,
            /// <summary>
            /// Displays the time for all complete and non-complete laps.
            /// </summary>
            AllLaps,
            /// <summary>
            /// Displays the time for all the complete laps.
            /// </summary>
            FinishedLaps,
            /// <summary>
            /// Displays the time for all the complete laps and the current lap.
            /// </summary>
            LastAndCurrentLaps,
            /// <summary>
            /// Displays the time for the current lap.
            /// </summary>
            CurrentLap,
            /// <summary>
            /// Displays the time for the best lap since the session started.
            /// </summary>
            SessionBestLap,
            /// <summary>
            /// Displays the time for the best race since the session started.
            /// </summary>
            SessionBestRace,
            /// <summary>
            /// Displays the time for the best lap ever.
            /// </summary>
            HistoricalBestLap,
            /// <summary>
            /// Displays the time for the best race ever.
            /// </summary>
            HistoricalBestRace,
        }


        [Tooltip("The timings to be displayed and the order to display them.")]
        public List<DisplayOptions> initialDisplayOptions = new List<DisplayOptions>();
        [Tooltip("A reference to the track manager.")]
        public TrackManager trackManager;
        [Tooltip("A reference to the TextMeshProUGUI to display the information.")]
        public TextMeshProUGUI textComponent;
        [Tooltip("A reference to the racer to display the information for.")]
        [RequireInterface(typeof(IRacer))]
        public Object initialRacer;

        List<Action> m_DisplayCalls = new List<Action>();
        IRacer m_Racer;
        StringBuilder m_StringBuilder = new StringBuilder(0, 300);

        public void StartDisplay()
        {
            //m_DisplayCalls.Add(DisplayFakeRecordTime);
            for (int i = 0; i < initialDisplayOptions.Count; i++)
            {
                switch (initialDisplayOptions[i])
                {
                    case DisplayOptions.AllLaps:
                        m_DisplayCalls.Add(DisplayAllLapTimes);
                        break;
                    case DisplayOptions.FinishedLaps:
                        m_DisplayCalls.Add(DisplayFinishedLapTimes);
                        break;
                    case DisplayOptions.LastAndCurrentLaps:
                        m_DisplayCalls.Add(DisplayLastAndCurrentLapTimes);
                        break;
                    case DisplayOptions.CurrentLap:
                        m_DisplayCalls.Add(DisplayCurrentLapTime);
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            
        }

        public void SetRacer(IRacer racer)
        {
            m_Racer = racer;
        }

        void Update()
        {
            m_StringBuilder.Clear();

            for (int i = 0; i < m_DisplayCalls.Count; i++)
            {
                m_DisplayCalls[i].Invoke();
            }

            textComponent.text = m_StringBuilder.ToString();
        }

        void DisplayFakeRecordTime()
        {
            m_StringBuilder.AppendLine($"UUID: Put the generated");
        }

        //void DisplayRaceTime()
        //{
        //    m_StringBuilder.AppendLine($"Total: {m_Racer.GetRaceTime():0.00}");
        //}

        void DisplayAllLapTimes()
        {
            List<float> lapTimes = m_Racer.GetLapTimes();
            int lapTotal = trackManager.raceLapTotal;

            for (int i = 0; i < lapTotal; i++)
            {
                m_StringBuilder.Append("Lap ");
                m_StringBuilder.Append(i + 1);
                m_StringBuilder.Append(": ");

                if (i < lapTimes.Count)
                {
                    m_StringBuilder.AppendFormat(lapTimes[i].ToString("0.00"));
                }
                else
                {
                    m_StringBuilder.Append("--:--");
                }

                m_StringBuilder.Append('\n');
            }

        }

        void DisplayFinishedLapTimes()
        {
            List<float> lapTimes = m_Racer.GetLapTimes();
            if (lapTimes.Count > 1)
            {
                m_StringBuilder.Append("Lap:  ");
                m_StringBuilder.Append(lapTimes[lapTimes.Count - 1].ToString("0.00"));
                m_StringBuilder.Append('\n');
                m_StringBuilder.Append("Last: ");
                m_StringBuilder.Append(lapTimes[lapTimes.Count - 2].ToString("0.00"));
                m_StringBuilder.Append('\n');
            }
        }

        void DisplayLastAndCurrentLapTimes()
        {
            float currentLapTime = m_Racer.GetLapTime();
            m_StringBuilder.Append("Lap:  ");
            m_StringBuilder.Append(currentLapTime.ToString("0.00"));
            m_StringBuilder.Append('\n');

            List<float> lapTimes = m_Racer.GetLapTimes();
            if (lapTimes.Count > 0)
            {
                m_StringBuilder.Append("Last: ");
                m_StringBuilder.Append(lapTimes[lapTimes.Count-1].ToString("0.00"));
                m_StringBuilder.Append('\n');
            }
        }

        void DisplayCurrentLapTime()
        {
            float currentLapTime = m_Racer.GetLapTime();
            //if (Mathf.Approximately(currentLapTime, 0f))
            //    return;

            m_StringBuilder.Append("Lap: ");
            m_StringBuilder.Append(currentLapTime.ToString("0.00"));
            m_StringBuilder.Append('\n');
        }

        //void DisplaySessionBestLapTime()
        //{
        //    float bestLapTime = trackManager.SessionBestLap;
        //    if (Mathf.Approximately(bestLapTime, -1f))
        //        return;

        //    m_StringBuilder.Append("Session Best Lap: ");
        //    m_StringBuilder.Append(bestLapTime.ToString("0.00"));
        //    m_StringBuilder.Append('\n');
        //}

        //void DisplaySessionBestRaceTime()
        //{
        //    float bestLapTime = trackManager.SessionBestRace;
        //    if (Mathf.Approximately(bestLapTime, -1f))
        //        return;

        //    m_StringBuilder.Append("Session Best Race: ");
        //    m_StringBuilder.Append(bestLapTime.ToString("0.00"));
        //    m_StringBuilder.Append('\n');
        //}

        //void DisplayHistoricalBestLapTime()
        //{
        //    float bestLapTime = trackManager.HistoricalBestLap;
        //    if (Mathf.Approximately(bestLapTime, -1f))
        //        return;

        //    m_StringBuilder.Append("Best Lap Ever: ");
        //    m_StringBuilder.Append(bestLapTime.ToString("0.00"));
        //    m_StringBuilder.Append('\n');
        //}

        //void DisplayHistoricalBestRaceTime()
        //{
        //    float bestLapTime = trackManager.HistoricalBestRace;
        //    if (Mathf.Approximately(bestLapTime, -1f))
        //        return;

        //    m_StringBuilder.Append("Best Race Ever: ");
        //    m_StringBuilder.Append(bestLapTime.ToString("0.00"));
        //    m_StringBuilder.Append('\n');
        //}

        /// <summary>
        /// Call this function to change what information is currently being displayed.  This causes a GCAlloc.
        /// </summary>
        /// <param name="newDisplay">A collection of the new options for the display.</param>
        /// <exception cref="ArgumentOutOfRangeException">One or more of the display options is not valid.</exception>
        public void RebindDisplayOptions(List<DisplayOptions> newDisplay)
        {
            m_DisplayCalls.Clear();
            //m_DisplayCalls.Add(DisplayFakeRecordTime);

            for (int i = 0; i < newDisplay.Count; i++)
            {
                switch (newDisplay[i])
                {
                    case DisplayOptions.FinishedLaps:
                        m_DisplayCalls.Add(DisplayFinishedLapTimes);
                        break;
                    case DisplayOptions.LastAndCurrentLaps:
                        m_DisplayCalls.Add(DisplayLastAndCurrentLapTimes);
                        break;
                    case DisplayOptions.CurrentLap:
                        m_DisplayCalls.Add(DisplayCurrentLapTime);
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Call this function to change the racer about which the information is being displayed.
        /// </summary>
        public void RebindRacer(IRacer newRacer)
        {
            m_Racer = newRacer;
        }
    }
}