using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PageTextController : MonoBehaviour
{
    [SerializeField] TextMeshPro TopText;
    [SerializeField] TextMeshPro SideText;

    [SerializeField, HideInInspector] LevelSelectController m_controller;

    private void OnValidate()
    {
        m_controller = FindObjectOfType<LevelSelectController>();
    }

    private void Update()
    {

        string text = "Page " + (m_controller.PageNum + 1).ToString() + " of " + (m_controller.MaxPageNum).ToString();

        TopText.text = text;
        SideText.text = text;

    }


}
