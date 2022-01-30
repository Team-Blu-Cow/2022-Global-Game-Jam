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
    List<string> scenesInBuild = new List<string>();

    [SerializeField] Sprite completeSprite;
    [SerializeField] Sprite uncompleteSprite;
    [SerializeField] Sprite notFoundSprite;

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
        UpdateTextMeshes();
    }

    // Update is called once per frame
    private void Update()
    {
        return;
        for(int i = 0; i < m_textMeshes.Count; i++)
        {
            m_textMeshes[i].color = Color.gray;
            m_sideTextMeshes[i].color = Color.gray;
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

            SpriteRenderer sr_top = null;
            SpriteRenderer sr_side = null;

            {
                Transform p = m_textMeshes[i].transform.parent;
                sr_top = p.GetComponentInChildren<SpriteRenderer>();
            }

            {
                Transform p = m_sideTextMeshes[i].transform.parent;
                sr_side = p.GetComponentInChildren<SpriteRenderer>();
            }
            
            sr_top.color = Color.grey;
            sr_side.color = Color.grey;

            if (LevelExists(levelNum))
            {
                if(iomodule.IsLevelCompleted(levelNum))
                {
                    sr_top.sprite = completeSprite;
                    sr_side.sprite = completeSprite;
                }
                else
                {
                    sr_top.sprite = uncompleteSprite;
                    sr_side.sprite = uncompleteSprite;
                }

                if(interact)
                {
                    interact.sceneName = SceneNameFromIndex(levelNum);
                    interact.enabled = true;
                }

            }
            else
            {
                interact.enabled = false;
                sr_top.sprite = notFoundSprite;
                sr_side.sprite = notFoundSprite;
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
