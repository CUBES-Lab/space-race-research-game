using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KartGame.KartSystems;
using UnityEngine;

namespace KartGame.Track
{
    /// <summary>
    /// A MonoBehaviour to deal with all the time and positions for the racers.
    /// </summary>
    public class TrackManager : MonoBehaviour
    {
        [Tooltip ("The name of the track in this scene.  Used for track time records.  Must be unique.")]
        public string trackName;
        [Tooltip ("Number of laps for the race.")]
        public int raceLapTotal;
        [Tooltip ("Number of seconds until the race is forced to end.")]
        public int raceMaxSeconds;
        [Tooltip ("All the checkpoints for the track in the order that they should be completed starting with the start/finish line checkpoint.")]
        public List<Checkpoint> checkpoints = new List<Checkpoint> ();
        [Tooltip("Reference to an object responsible for repositioning karts.")]
        public KartRepositioner kartRepositioner;
        public CollectableManager collectableManager;
        private IRacer m_racer;

        bool m_IsRaceRunning;
        bool m_IsRaceStopped; // need to trigger the event here
        Dictionary<IRacer, Checkpoint> m_RacerNextCheckpoints = new Dictionary<IRacer, Checkpoint> (16);
        //LeaderboardRecord m_leaderboardRecord;

        public bool IsRaceRunning => m_IsRaceRunning;
        public bool IsRaceStopped => m_IsRaceStopped;

        public bool hitFirstCheckpoint;

        public float GetLeaderboardScore()
        {
            return 0.0f;//m_leaderboardRecord.GetScore();
        }

        public float GetLeaderboardRaceTime()
        {
            return 0.0f;//m_leaderboardRecord.GetRaceTime();
        }

        void Awake ()
        {
            if(checkpoints.Count < 3)
                Debug.LogWarning ("There are currently " + checkpoints.Count + " checkpoints set on the Track Manager.  A minimum of 3 is recommended but kart control will not be enabled with 0.");

            //m_leaderboardRecord.Load(uid);
        }

        void OnEnable ()
        {
            for (int i = 0; i < checkpoints.Count; i++)
            {
                checkpoints[i].OnKartHitCheckpoint += CheckRacerHitCheckpoint;
            }
        }

        void OnDisable ()
        {
            for (int i = 0; i < checkpoints.Count; i++)
            {
                checkpoints[i].OnKartHitCheckpoint -= CheckRacerHitCheckpoint;
            }
        }

        public void StartTrackManager ()
        {
            if(checkpoints.Count == 0)
                return;
            
            Object[] allRacerArray = FindObjectsOfType<Object> ().Where (x => x is IRacer).ToArray ();

            for (int i = 0; i < allRacerArray.Length; i++)
            {
                IRacer racer = allRacerArray[i] as IRacer;
                m_RacerNextCheckpoints.Add (racer, checkpoints[0]);
                racer.DisableControl();
                m_racer = racer;
            }
            if (collectableManager == null)
            {
                collectableManager = GameObject.Find("CollectableManager").GetComponent<CollectableManager>();
            }
        }

        /// <summary>
        /// Starts the timers and enables control of all racers.
        /// </summary>
        public void StartRace ()
        {
            m_IsRaceRunning = true;

            foreach (KeyValuePair<IRacer, Checkpoint> racerNextCheckpoint in m_RacerNextCheckpoints)
            {
                racerNextCheckpoint.Key.EnableControl ();
                racerNextCheckpoint.Key.UnpauseTimer ();
            }

            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Start Race", "The race has started.");
        }

        /// <summary>
        /// Stops the timers and disables control of all racers, also saves the historical records.
        /// </summary>
        public void StopRace ()
        {
            m_IsRaceRunning = false;

            foreach (KeyValuePair<IRacer, Checkpoint> racerNextCheckpoint in m_RacerNextCheckpoints)
            {
                racerNextCheckpoint.Key.DisableControl ();
                racerNextCheckpoint.Key.PauseTimer ();
            }

            //m_leaderboardRecord.Save();

            // Log the final score and race time
            ParticipantInfo participantInfo = GameObject.Find("ParticipantInfo").GetComponent<ParticipantInfo>();
            KartCollect kartCollect = GameObject.Find("Kart").GetComponent<KartCollect>();
            Racer kartRacer = GameObject.Find("Kart").GetComponent<Racer>();
            string uid = participantInfo.GetUID();
            int score = kartCollect.score;
            float raceTime = kartRacer.GetRaceTime();
            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Stop Race", "Session score: " + score.ToString() + ", Race Time: " + raceTime.ToString("0.000"));

            // Save race session
            GameObject gameManager = GameObject.Find("GameManager");
            RaceSession raceSession = gameManager.GetComponent<RaceSession>();
            raceSession.SetValues(uid, score, raceTime);
            raceSession.SaveNewSession();

            m_IsRaceStopped = true;
        }

        public void Update ()
        {
            if (m_racer.GetRaceTime() >= raceMaxSeconds)
            {
                StopRace();
            }
        }

        void CheckRacerHitCheckpoint (IRacer racer, Checkpoint checkpoint)
        {
            if (!m_IsRaceRunning)
            {
                StartCoroutine (CallWhenRaceStarts (racer, checkpoint));
                return;
            }

            if (m_RacerNextCheckpoints.ContainsKeyValuePair (racer, checkpoint))
            {
                m_RacerNextCheckpoints[racer] = checkpoints.GetNextInCycle (checkpoint);
                RacerHitCorrectCheckpoint (racer, checkpoint);
                if(checkpoints.FindIndex(x => x == checkpoint) == 1)
                {
                    hitFirstCheckpoint = true;
                }
            }
            else
            {
                RacerHitIncorrectCheckpoint (racer, checkpoint);
            }
        }

        IEnumerator CallWhenRaceStarts (IRacer racer, Checkpoint checkpoint)
        {
            while (!m_IsRaceRunning)
            {
                yield return null;
            }

            CheckRacerHitCheckpoint (racer, checkpoint);
        }

        void RacerHitCorrectCheckpoint (IRacer racer, Checkpoint checkpoint)
        {
            if (checkpoint.isStartFinishLine)
            {
                int racerCurrentLap = racer.GetCurrentLap ();
                if (racerCurrentLap > 0)
                {
                    if (racerCurrentLap == raceLapTotal)
                    {
                        //float score = racer.GetScore();
                        float raceTime = racer.GetRaceTime();

                        //if (score > m_leaderboardRecord.score || (score == m_leaderboardRecord.score && raceTime < m_leaderboardRecord.raceTime))
                        //    m_leaderboardRecord.SetRecord(score, raceTime);

                        racer.DisableControl();
                        racer.PauseTimer();
                    }
                    else
                    {
                        collectableManager.ResetCollectables();
                    }
                }

                if (CanEndRace ())
                    StopRace ();

                racer.HitStartFinishLine ();
                hitFirstCheckpoint = false;
            }
        }

        bool CanEndRace ()
        {
            foreach (KeyValuePair<IRacer, Checkpoint> racerNextCheckpoint in m_RacerNextCheckpoints)
            {
                if (racerNextCheckpoint.Key.GetCurrentLap () < raceLapTotal)
                    return false;
            }

            return true;
        }

        void RacerHitIncorrectCheckpoint (IRacer racer, Checkpoint checkpoint)
        {
            // No implementation required by template.
        }

        /// <summary>
        /// This function should be called when a kart gets stuck or falls off the track.
        /// It will find the last checkpoint the kart went through and reposition it there.
        /// </summary>
        /// <param name="movable">The movable representing the kart.</param>
        public void ReplaceMovable (IMovable movable)
        {
            IRacer racer = movable.GetRacer ();
            
            if(racer == null)
                return;
            
            Checkpoint nextCheckpoint = m_RacerNextCheckpoints[racer];
            int lastCheckpointIndex = (checkpoints.IndexOf (nextCheckpoint) + checkpoints.Count - 1) % checkpoints.Count;
            Checkpoint lastCheckpoint = checkpoints[lastCheckpointIndex];

            bool isControlled = movable.IsControlled ();
            movable.DisableControl ();
            kartRepositioner.OnRepositionComplete += ReenableControl;

            kartRepositioner.Reposition (lastCheckpoint, movable, isControlled);
        }

        void ReenableControl (IMovable movable, bool doEnableControl)
        {
            if(doEnableControl)
                movable.EnableControl ();
            kartRepositioner.OnRepositionComplete -= ReenableControl;
        }
    }
}