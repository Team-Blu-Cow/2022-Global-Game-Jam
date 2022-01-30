using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blu;
using UnityEngine.SceneManagement;
using TMPro;

// dont read this script
// its a fucking crime


public class LevelSelectController : MonoBehaviour
{
    const int m_maxLevelCount = 20;
    int m_pageNum = 0;

    public int PageNum { get => m_pageNum; }
    public int MaxPageNum 
    { get {
            int textCount = m_textMeshes.Count;
            int pageCount = (m_maxLevelCount + textCount - 1) / textCount;
            return pageCount; 
    } }


    LevelManager m_manager;

    blu.IOModule iomodule;

    [SerializeField] List<TextMeshPro> m_textMeshes = new List<TextMeshPro>();
    [SerializeField] List<TextMeshPro> m_sideTextMeshes = new List<TextMeshPro>();


    // the color is flickering and i dont know why
    // we will set these colors in the update loop
    List<Color> m_colors = new List<Color>();
    List<string> scenesInBuild = new List<string>();


    Color levelCompleted = Color.green;
    Color levelUncompleted = Color.red;
    Color levelNotFound = Color.grey;


    private void OnValidate()
    {
        m_manager = FindObjectOfType<LevelManager>();
    }

    private void Awake()
    {
        // http://answers.unity.com/answers/1394340/view.html
        // fuck i hate unity sometimes
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        iomodule = App.GetModule<IOModule>();

        for(int i = 0; i < m_textMeshes.Count; i++)
        {
            m_colors.Add(Color.black);
        }

        UpdateTextMeshes();
    }

    // Update is called once per frame
    private void Update()
    {
        
        for(int i = 0; i < m_textMeshes.Count; i++)
        {
            Color c = m_textMeshes[i].color;

            c.r = m_colors[i].r;
            c.g = m_colors[i].g;
            c.b = m_colors[i].b;

            m_textMeshes[i].color = c;
            m_sideTextMeshes[i].color = c;
        }
    }

    public void NextPage()
    {

        if (m_pageNum == MaxPageNum - 1) return;

        m_pageNum++;
        UpdateTextMeshes();
    }

    public void LastPage()
    {
        if (m_pageNum == 0) return;

        m_pageNum--;
        UpdateTextMeshes();
    }

    void UpdateTextMeshes()
    {
        int textCount = m_textMeshes.Count;

        for (int i = 0; i < textCount; i++)
        {

            LevelSelectInteract interact = m_textMeshes[i].gameObject.GetComponentInParent<LevelSelectInteract>();

            int levelNum = (m_pageNum * textCount) + i + 1;
            m_textMeshes[i].text = "Level " + levelNum.ToString();
            m_sideTextMeshes[i].text = "Level " + levelNum.ToString();

            if(LevelExists(levelNum))
            {
                if(iomodule.IsLevelCompleted(levelNum))
                {
                    m_colors[i] = levelCompleted;
                }
                else
                {
                    m_colors[i] = levelUncompleted;
                }

                if(interact)
                {
                    interact.sceneName = SceneNameFromIndex(levelNum);
                    interact.enabled = true;
                }

            }
            else
            {
                m_colors[i] = levelNotFound;
                interact.enabled = false;
            }
        }
    }

    private string SceneNameFromIndex(int index)
    {
        return "Level " + index.ToString();
    }

    public bool LevelExists(int index)
    {
        string sceneName = SceneNameFromIndex(index);

        if (scenesInBuild.Contains(sceneName))
        { 
            return true;
        }

        return false;
    }

}
