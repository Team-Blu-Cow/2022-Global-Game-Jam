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

    enum State
    {
        TopDown,
        SideScroller
    }

    [SerializeField] private State m_state;

    public void OnValidate()
    {
        SetDepthLimits(m_editorSliceDistance);
    }

    public void Start()
    {
        AlignDynamicObjects();
        SetDepthLimits();
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
        // Disable all other slices
    }

    void TransitionToTopDown()
    {
        // Move Camera
        // Enable all other slices
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

    void AlignDynamicObjects()
    {
        foreach (var obj in m_dynamicObjects)
        {
            float dist = float.MaxValue;
            int closest = 0;
            int index = 0;
            foreach (var slice in m_slices)
            {
                var newDist = Vector3.Distance(slice.transform.position, obj.transform.position);
                if(newDist < dist)
                {
                    dist = newDist;
                    closest = index;
                }
                index++;
            }

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
