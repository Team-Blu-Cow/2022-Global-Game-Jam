using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] LevelEndCanvas m_levelEndCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //blu.App.GetModule<blu.SceneModule>().SwitchScene(SceneManager.GetActiveScene().name, blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);
            m_levelEndCanvas.ShowCanvas();
        }        
    }

}
