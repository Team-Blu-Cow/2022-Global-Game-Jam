using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] LevelEndCanvas m_levelEndCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            blu.App.GetModule<blu.IOModule>().SetLevelComplete(SceneManager.GetActiveScene().name, true);

            m_levelEndCanvas.ShowCanvas(m_levelEndCanvas.GetComponentInChildren<Canvas>());
            PlayerController player = other.GetComponent<PlayerController>();
            player.PlayerInput.PlayerControls.Disable();
            player.PlayerInput.UI.Enable();
        }        
    }

}
