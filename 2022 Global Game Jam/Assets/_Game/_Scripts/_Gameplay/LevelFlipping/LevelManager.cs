using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int m_currentSlice;
    [SerializeField] private float m_editorSliceDistance = 0;

    [SerializeField] private List<LevelSlice> m_slices;

    [SerializeField] private float m_sliceDepth = 1;
    [SerializeField] private float m_levelWidth = 10;

    [SerializeField] private List<GameObject> m_dynamicObjects;
    public GameObject m_playerObject;

    [SerializeField] private CinemachineBrain m_cameraBrain;

    [SerializeField] private float m_cameraBlend = 1;
    public float cameraBlend
    { get { return m_cameraBlend; } }

    public bool topDown;

    // this is temp stuff, change this to work with game state singletons (or whatever they are called)
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

            foreach(var slice in slices)
            {
                slice.manager = this;
            }

            m_slices = new List<LevelSlice>(slices);

            SetDepthLimits(m_editorSliceDistance);
        }

        if(m_cameraBrain == null)
        {
            m_cameraBrain = FindObjectOfType<CinemachineBrain>();
        }

    }

    public void OnEnable()
    {
        blu.App.GetModule<blu.GameStateModule>().OnStateChangeEvent += OnStateChange;
    }

    public void OnDisable()
    {
        blu.App.GetModule<blu.GameStateModule>().OnStateChangeEvent -= OnStateChange;
    }

    public void Start()
    {
        if (!m_dynamicObjects.Contains(m_playerObject))
            m_dynamicObjects.Add(m_playerObject);
        foreach (var obj in m_dynamicObjects)
        {
            var transparencyController = obj.GetComponent<ObjectTransparencyController>();
            if (transparencyController)
                transparencyController.manager = this;
        }

        m_cameraBrain.m_DefaultBlend.m_Time = m_cameraBlend;

        AlignDynamicObjects();
        SetDepthLimits();
        SpawnWalls();
    }

    void OnStateChange(blu.GameStateModule.RotationState state)
    {
        if(state == blu.GameStateModule.RotationState.SIDE_ON)
        {
            topDown = false;
            m_currentSlice = FindClosestSlice(m_playerObject);
            TransitionToSlice(m_currentSlice);
        }
        else if(state == blu.GameStateModule.RotationState.TOP_DOWN)
        {
            topDown = true;
            TransitionToTopDown();
        }
    }

    private void Update()
    {
        // this is temporary, just to demonstrate functionality
        /*if(UnityEngine.InputSystem.Keyboard.current.qKey.wasReleasedThisFrame)
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
            
        }*/
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
        Camera.main.gameObject.GetComponent<CameraViewSpots>().CameraMoveToSideView();

        // Align Objects to slice
        AlignDynamicObjects();

        m_playerObject.GetComponent<PlayerController>().SetXPos();

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
        // Move camera
        Camera.main.gameObject.GetComponent<CameraViewSpots>().CameraMoveToTopDownView();

        // Enable all other slices
        int i = 0;
        foreach (var slice in m_slices)
        {
            if (i != m_currentSlice)
                slice.SetSliceEnabled(true);
            i++;
        }
    }

    public int FindClosestSlice(GameObject obj)
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

    public LevelSlice GetSlice(int i)
    {
        if (i > m_slices.Count - 1)
            return null;

        return m_slices[i];
    }

    public int SliceCount()
    {
        return m_slices.Count;
    }

    private void SpawnWalls()
    {
        List<GameObject> walls = new List<GameObject>();
        float wallHeight = 5.0f;


        for (int i = 0; i < 4; i++)
        {
            walls.Add(new GameObject());
            walls[i].name = "Wall " + (i + 1);
            walls[i].transform.parent = this.transform;
            walls[i].AddComponent<BoxCollider>();
        }

        //Top
        walls[0].transform.position = new Vector3(-1, 0, m_levelWidth / 2.0f);
        walls[0].GetComponent<BoxCollider>().size = new Vector3(1, wallHeight, m_levelWidth);

        //Left
        walls[1].transform.position = new Vector3(((m_slices.Count * m_sliceDepth) / 2.0f) - 0.5f, 0, -1);
        walls[1].GetComponent<BoxCollider>().size = new Vector3((m_slices.Count * m_sliceDepth), wallHeight, 1);

        //Bottom
        walls[2].transform.position = new Vector3((m_slices.Count * m_sliceDepth), 0, m_levelWidth / 2.0f);
        walls[2].GetComponent<BoxCollider>().size = new Vector3(1, wallHeight, m_levelWidth);

        //Right
        walls[3].transform.position = new Vector3(((m_slices.Count * m_sliceDepth) / 2.0f) - 0.5f, 0, m_levelWidth + 0.5f);
        walls[3].GetComponent<BoxCollider>().size = new Vector3((m_slices.Count * m_sliceDepth), wallHeight, 1);
    }
}
