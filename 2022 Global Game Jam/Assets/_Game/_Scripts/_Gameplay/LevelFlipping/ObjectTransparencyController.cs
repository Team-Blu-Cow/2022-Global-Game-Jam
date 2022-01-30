using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectTransparencyController : MonoBehaviour
{
    [SerializeField, HideInInspector] private Material m_material;
    [SerializeField, HideInInspector] private List<TextMeshPro> m_tmPro;
    [SerializeField, HideInInspector] private List<SpriteRenderer> m_sr;


    [SerializeField] private float m_opacity = 1;
    [SerializeField] private float m_targetOpacity = 1;

    [SerializeField] private LevelManager m_manager;

    [SerializeField] bool overrideGreyScale = true;

    [SerializeField] List<GameObject> disabledObjects = new List<GameObject>();

    public enum ObjectType
    {
        Static,
        Dynamic
    }

    [SerializeField] private ObjectType m_type = ObjectType.Static;

    public LevelManager manager
    {
        set { m_manager = value; }
    }

    public void OnEnable()
    {
        blu.App.GetModule<blu.GameStateModule>().OnStateChangeEvent += OnStateChange;
    }

    public void OnDisable()
    {
        blu.App.GetModule<blu.GameStateModule>().OnStateChangeEvent -= OnStateChange;
    }

    static bool AlmostEqual(float a, int b)
    {
        return (Mathf.RoundToInt(a) == b);
    }

    public void OnStateChange(blu.GameStateModule.RotationState state)
    {
        if (state == blu.GameStateModule.RotationState.SIDE_ON)
        {
            
            foreach (var obj in disabledObjects)
            {
                
                if (!AlmostEqual(transform.position.x, m_manager.currentSlice))
                {
                    obj.SetActive(false);
                }
            }
            
        }
        else if (state == blu.GameStateModule.RotationState.TOP_DOWN)
        {
            foreach (var obj in disabledObjects)
            {
                obj.SetActive(true);
            }
        }
    }

    public void OnValidate()
    {
        if(m_manager == null)
            m_manager = FindObjectOfType<LevelManager>();

        // Matthew this code doesn't work!!!!!!!!
        // its stupid and i hate it and has undone 15 minutes of tedious work!!!!!
        /*if(m_manager)
        {
            int closest = m_manager.FindClosestSlice(this.gameObject);

            for(int s = 0; s < m_manager.SliceCount(); s++)
            {
                LevelSlice slice = m_manager.GetSlice(s);
                if (s == closest)
                {
                    if (m_type == ObjectType.Static)
                    {
                        slice.AddStaticObject(this.gameObject);
                        transform.SetParent(slice.transform);
                        transform.localPosition = new Vector3(0, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        slice.AddDynamicObject(gameObject);
                    }

                    if (TryGetComponent<RectTransform>(out RectTransform component))
                    {
                        component.localPosition = new Vector3(0, component.position.y, component.position.z);
                    }
                }
                else
                {
                    slice.RemoveStaticObject(this.gameObject);
                    slice.RemoveDynamicObject(gameObject);
                }
            }
        }*/

        m_tmPro = new List<TextMeshPro>(GetComponentsInChildren<TextMeshPro>());
        m_sr = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
    }

    public float opacity
    {
        get { return m_opacity; }
        set { m_opacity = value; }
    }

    public float targetOpacity
    {
        get { return m_targetOpacity; }
        set { m_targetOpacity = value; }
    }

    private void Start()
    {
        if (TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            m_material = meshRenderer.material;
        }

        if(!m_manager.topDown)
        {
            if(m_opacity < 0.5f)
            {
                foreach (var obj in disabledObjects)
                {
                    obj.SetActive(false);
                }
            }
        }

        
    }

    public float height;
    public float lerpval;

    // Update is called once per frame
    void Update()
    {
        if(m_material)
        {
            if (overrideGreyScale)
            {
                height = transform.position.y;
                lerpval = Mathf.InverseLerp(-1, 5, height);
                m_material.SetFloat("_Grayscale", lerpval);
            }
            //m_material.SetFloat("_GreyScale", m_opacity);
            m_material.SetFloat("_Opacity", m_opacity);
        }

        for(int i = 0; i < m_tmPro.Count; i++)
        {
            if(m_tmPro[i])
            {
                Color color = m_tmPro[i].color;
                m_tmPro[i].color = new Color(color.r, color.b, color.g, m_opacity);
            }
        }

        for (int i = 0; i < m_sr.Count; i++)
        {
            if (m_sr[i])
            {
                Color color = m_sr[i].color;
                m_sr[i].color = new Color(color.r, color.b, color.g, m_opacity);
            }
        }
    }

    public void FadeOut(float time)
    {
        LeanTween.value(gameObject, 1, 0, time).setEaseInOutCirc().setOnUpdate(SetOpacity);
    }

    public void FadeIn(float time)
    {
        LeanTween.value(gameObject, 0, 1, time).setEaseInOutCirc().setOnUpdate(SetOpacity);
    }

    public void SetOpacity(float f)
    {
        m_opacity = f;
    }
}
