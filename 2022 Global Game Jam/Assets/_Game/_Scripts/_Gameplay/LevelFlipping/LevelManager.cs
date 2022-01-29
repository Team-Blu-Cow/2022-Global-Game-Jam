using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int m_currentSlice;
    [SerializeField] private float m_editorSliceDistance = 0;

    [SerializeField] private List<LevelSlice> m_slices;

    [SerializeField] private float m_sliceDepth = 1;
    [SerializeField] private float m_levelWidth = 10;

    [SerializeField] private List<GameObject> m_dynamicObjects;
    [SerializeField] private GameObject m_playerObject;

    // this is also all temp stuff, change to work with game state singletons (or whatever they are called)
    enum State
    {
        TopDown,
        SideScroller
    }

    [SerializeField] private State m_state;

    public void OnValidate()
    {
        if (!Application.isPlaying)
        {
            LevelSlice[] slices = transform.GetComponentsInChildren<LevelSlice>();

            m_slices = new List<LevelSlice>(slices);

            SetDepthLimits(m_editorSliceDistance);
        }
    }

    public void Start()
    {
        if (!m_dynamicObjects.Contains(m_playerObject))
            m_dynamicObjects.Add(m_playerObject);
        AlignDynamicObjects();
        SetDepthLimits();
    }

    private void Update()
    {
        // this is temporary, just to demonstrate functionality
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
        // TODO: Move camera

        // Align Objects to slice
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
        // TODO: Move camera

        // Enable all other slices
        foreach (var slice in m_slices)
        {
            slice.SetSliceEnabled(true);
        }
    }

    int FindClosestSlice(GameObject obj)
    {
        float dist = float.MaxValue;
        int closest = 0;
        int index = 0;
        foreach (var slice in m_slices)
        {
            var newDist = Vector3.Distance(new Vector3(slice.transform.position.x, 0, 0), new Vector3(obj.transform.position.x, 0, 0));
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
