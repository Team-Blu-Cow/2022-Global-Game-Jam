using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using blu;

public class MuteAudio : MonoBehaviour
{
    private Toggle _mute;

    private void OnEnable()
    {
        _mute = GetComponent<Toggle>();

        _mute.isOn = App.GetModule<SettingsModule>().audioSettings.mute;
    }

    public void ToggleMuteAudio(bool in_value)
    {
        App.GetModule<SettingsModule>().audioSettings.mute = in_value;
        //SettingsModule.Save();
    }
}