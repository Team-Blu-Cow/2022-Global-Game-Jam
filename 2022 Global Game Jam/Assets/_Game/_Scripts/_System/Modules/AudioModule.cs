using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace blu
{
    public class AudioModule : Module
    {
        public enum FloorMaterial
        {
            SNOW = 0,
            STONE = 1
        }

        [SerializeField]
        private EventReference m_BGM;

        private EventInstance m_BGMinstance;

        [SerializeField]
        private EventReference[] m_events;

        private List<EventInstance> m_instances = new List<EventInstance>();
        private PLAYBACK_STATE m_playbackState;

        public void PlayFootstep(GameObject in_emitter, FloorMaterial in_material)
        {
            foreach (EventInstance eventInstance in m_instances)
            {
                eventInstance.getPlaybackState(out m_playbackState);
                if (m_playbackState == PLAYBACK_STATE.PLAYING)
                {
                    return;
                }
            }

            PlayerController cacheController = in_emitter.GetComponent<PlayerController>();
            ConsoleProDebug.Watch("Magnitude", cacheController.rb.velocity.magnitude.ToString());
            ConsoleProDebug.Watch("IsGrounded", cacheController.pInfo.IsGrounded.ToString());
            //check velocity magnitude
            //check if grounded
            if (cacheController.rb.velocity.magnitude < 2 || !cacheController.pInfo.IsGrounded)
            {
                return;
            }

            switch (in_material)
            {
                case FloorMaterial.SNOW:
                    m_instances[0].start();
                    break;

                case FloorMaterial.STONE:
                    m_instances[1].start();
                    break;

                default:
                    break;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            m_BGMinstance = RuntimeManager.CreateInstance(m_BGM);

            for (int i = 0; i < m_events.Length; i++)
            {
                m_instances.Add(RuntimeManager.CreateInstance(m_events[i]));
            }

            m_BGMinstance.start();
        }
    }
}