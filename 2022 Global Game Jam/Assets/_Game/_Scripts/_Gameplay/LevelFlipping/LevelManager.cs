using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int m_sliceCount;
    [SerializeField] private int m_currentSlice;
    [SerializeField] private float m_editorSliceDistance = 0;

    [SerializeField] private List<LevelSlice> m_slices;

    [SerializeField] private float m_sliceDepth = 1;
    [SerializeField] private float m_levelWidth = 10;

    [SerializeField] private List<GameObject> m_dynamicObjects;
    [SerializeField] private GameObject m_playerObject;

    enum State
    {
        TopDown,
        SideScroller
    }

    [SerializeField] private State m_state;

    public void OnValidate()
    {
        if(!Application.isPlaying)
            SetDepthLimits(m_editorSliceDistance);
    }

    public void Start()
    {
        AlignDynamicObjects();
        SetDepthLimits();
    }

    private void Update()
    {
        if(UnityEngine.InputSystem.Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            if(m_state == State.SideScroller)
            {
                TransitionToTopDown();
                m_state = State.TopDown;
            }
            else
            {
                m_currentSlice = FindClosestSlice(m_playerObject);
                TransitionToSlice(m_currentSlice);
                m_state = State.SideScroller;
            }
            
        }
    }

    void SetDepthLimits(float offset = 0)
    {
        float currentDepth = 0;
        foreach(var slice in m_slices)
        {
            slice.SetDepth(currentDepth, m_sliceDepth, m_levelWidth);
            slice.SetPosition(transform.position, offset*currentDepth);
            currentDepth += m_sliceDepth;
        }
    }

    void TransitionToSlice(int index)
    {
        // Move camera

        AlignDynamicObjects();

        // Disable all other slices
        int i = 0;
        foreach (var slice in m_slices)
        {
            if (i != index)
                slice.SetSliceEnabled(false);
            i++;
        }
    }

    void TransitionToTopDown()
    {
        // Move Camera
        // Enable all other slices
        int i = 0;
        foreach (var slice in m_slices)
        {
            slice.SetSliceEnabled(true);
        }
    }

    void OrganiseDynamicObjects()
    {
        foreach (var obj in m_dynamicObjects)
        {
            foreach(var slice in m_slices)
            {
                if(obj.transform.position.x >= slice.GetLowerBound() && obj.transform.position.x < slice.GetUpperBound())
                {
                    slice.AddDynamicObject(obj);
                    foreach (var otherSlice in m_slices)
                    {
                        if (otherSlice != slice)
                            otherSlice.RemoveDynamicObject(obj);
                    }
                    break;
                }
            }
        }
    }

    int FindClosestSlice(GameObject obj)
    {
        float dist = float.MaxValue;
        int closest = 0;
        int index = 0;
        foreach (var slice in m_slices)
        {
            var newDist = Vector3.Distance(slice.transform.position, obj.transform.position);
            if (newDist < dist)
            {
                dist = newDist;
                closest = index;
            }
            index++;
        }

        return closest;
    }

    void AlignDynamicObjects()
    {
        foreach (var obj in m_dynamicObjects)
        {
            int closest = FindClosestSlice(obj);

            LevelSlice closestSlice = m_slices[closest];
            closestSlice.AddDynamicObject(obj);
            foreach (var otherSlice in m_slices)
            {
                if (otherSlice != closestSlice)
                    otherSlice.RemoveDynamicObject(obj);
            }

            closestSlice.SetDynamicObjectPosition(obj);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * 10));
    }

}
