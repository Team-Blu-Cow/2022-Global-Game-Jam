using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace blu
{
    public class SettingsModule : Module
    {
        public struct AudioSettings
        {
            public bool mute;
            public float masterVolume;

            public float musicVolume;
            public float sfxVolume;
            public FMOD.Studio.Bus master;
            public FMOD.Studio.Bus music;
            public FMOD.Studio.Bus sfx;
        }

        public struct GraphicsSettings
        {
            public Resolution screenResolution;
            public bool fullscreen;
        }

        public AudioSettings audioSettings;
        public GraphicsSettings graphicsSettings;

        //Call Order:
        // Awake > Enable > Init > Start
        protected override void Awake()
        {
            base.Awake();
        }

        public override void Initialize() //
        {
            base.Initialize();
            InitAudioSettings();
            InitGraphicsSettings();
        }

        private void InitAudioSettings()
        {
            audioSettings.masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.25f);
            audioSettings.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            audioSettings.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            audioSettings.mute = PlayerPrefs.GetInt("MuteAudio", 0) != 0;

            audioSettings.master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
            audioSettings.music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
            audioSettings.sfx = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        }

        private void InitGraphicsSettings()
        {
            graphicsSettings.screenResolution.width = PlayerPrefs.GetInt("Width", 1600);
            graphicsSettings.screenResolution.height = PlayerPrefs.GetInt("Height", 800);
            graphicsSettings.screenResolution.refreshRate = PlayerPrefs.GetInt("RefreshRate", 60);
            graphicsSettings.fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) != 0;
        }

        public void Update()
        {
            audioSettings.master.setVolume(audioSettings.masterVolume);
            audioSettings.master.setVolume(audioSettings.masterVolume);
            audioSettings.music.setVolume(audioSettings.musicVolume);
            audioSettings.sfx.setVolume(audioSettings.sfxVolume);
        }

        protected override void OnApplicationQuit()
        {
            // audio assignments
            PlayerPrefs.SetFloat("MasterVolume", audioSettings.masterVolume);
            PlayerPrefs.SetFloat("MusicVolume", audioSettings.musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", audioSettings.sfxVolume);
            PlayerPrefs.SetInt("MuteAudio", (audioSettings.mute ? 0 : 1));

            // graphics assignments
            PlayerPrefs.SetInt("Width", graphicsSettings.screenResolution.width);
            PlayerPrefs.SetInt("Height", graphicsSettings.screenResolution.height);
            PlayerPrefs.SetInt("RefreshRate", graphicsSettings.screenResolution.refreshRate);
            PlayerPrefs.SetInt("Fullscreen", (graphicsSettings.fullscreen ? 0 : 1));

            base.OnApplicationQuit();
        }
    }
}