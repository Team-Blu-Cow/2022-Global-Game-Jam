using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransparencyController : MonoBehaviour
{
    [SerializeField, HideInInspector] private Material m_material;

    [SerializeField] private float m_opacity = 1;
    [SerializeField] private float m_targetOpacity = 1;

    private LevelManager m_manager;

    public LevelManager manager
    {
        set { m_manager = value; }
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
        m_material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
       m_material.SetFloat("_Opacity", m_opacity);
    }

    public void FadeOut(float time)
    {
        LeanTween.value(gameObject, 1, 0, time).setEaseInOutCirc().setOnUpdate(SetOpacity);
    }

    public void FadeIn(float time)
    {
        LeanTween.value(gameObject, 0, 1, time).setEaseInOutCirc().setOnUpdate(SetOpacity);
    }

    void SetOpacity(float f)
    {
        m_opacity = f;
    }
}
