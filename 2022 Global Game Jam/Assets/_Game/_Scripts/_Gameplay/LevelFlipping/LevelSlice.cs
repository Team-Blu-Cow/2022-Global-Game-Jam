using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSlice : MonoBehaviour
{
    // set this lists values in editor
    [SerializeField] private List<GameObject> m_staticObjects;

    // this list is serialized for debug purposes only, don't add anything to it in editor!!
    [SerializeField, Tooltip("this list is serialized for debug purposes only, don't add anything to it in editor!!") ]
    private List<GameObject> m_dynamicObjects;

    [SerializeField] private SliceDimentions m_depthLimits;

    [SerializeField] private LevelManager m_manager;

    public LevelManager manager
    {
        set { m_manager = value; }
    }

    public SliceDimentions sliceDepth
    {
        get { return m_depthLimits; }
        set { m_depthLimits = value; }
    }

    private void OnValidate()
    {
        for(int i = m_staticObjects.Count - 1; i >= 0; i--)
        {
            if(m_staticObjects[i] == null)
                m_staticObjects.RemoveAt(i);
        }
    }

    public void AddDynamicObject(GameObject obj)
    {
        if (m_dynamicObjects.Contains(obj))
            return;
        m_dynamicObjects.Add(obj);
        SetDynamicObjectPosition(obj);
    }

    private void Start()
    {
        foreach(var obj in m_staticObjects)
        {
            var transparencyController = obj.GetComponent<ObjectTransparencyController>();
            if (transparencyController)
                transparencyController.manager = m_manager;
        }
    }

    public void RemoveDynamicObject(GameObject obj)
    {
        if (m_dynamicObjects.Contains(obj))
            m_dynamicObjects.Remove(obj);
    }

    public void SetSliceEnabled(bool enabled)
    {
        if(enabled)
        {
            foreach (var obj in m_dynamicObjects)
            {
                var mat = obj.GetComponent<ObjectTransparencyController>();
                if(mat)
                    mat.FadeIn(m_manager.cameraBlend);
            }

            foreach (var obj in m_staticObjects)
            {
                var mat = obj.GetComponent<ObjectTransparencyController>();
                if (mat)
                    mat.FadeIn(m_manager.cameraBlend);
            }
        }
        else
        {
            foreach (var obj in m_dynamicObjects)
            {
                var mat = obj.GetComponent<ObjectTransparencyController>();
                if (mat)
                    mat.FadeOut(m_manager.cameraBlend);
            }

            foreach (var obj in m_staticObjects)
            {
                var mat = obj.GetComponent<ObjectTransparencyController>();
                if (mat)
                    mat.FadeOut(m_manager.cameraBlend);
            }
        }
    }

    public void SetDepth(float offset, float depth, float width)
    {
        m_depthLimits.offset = offset;
        m_depthLimits.depth = depth;
        m_depthLimits.width = width;
    }

    public void SetPosition(Vector3 basePos, float offset)
    {
        float x = basePos.x;
        float y = transform.position.y;
        float z = transform.position.z;
        transform.position = new Vector3(x+m_depthLimits.offset + offset, y, z);

        foreach (var obj in m_dynamicObjects)
        {
            SetDynamicObjectPosition(obj);
        }

        // this has been moved to ObjectTransparencyController::OnValidate()
        // objects now automagicly change their parents when they are moved within the scene

        //foreach (var obj in m_staticObjects)
        //{
        //    obj.transform.localPosition = new Vector3(0, obj.transform.position.y, obj.transform.position.z);
        //}
    }

    public void SetDynamicObjectPosition(GameObject obj)
    {
        obj.transform.position = new Vector3(transform.position.x, obj.transform.position.y, obj.transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z + (m_depthLimits.width*0.5f)), new Vector3(1, 1, m_depthLimits.width));
    }

    public void AddStaticObject(GameObject obj)
    {
        if(m_staticObjects.Contains(obj) == false)
        {
            m_staticObjects.Add(obj);
        }
    }

    public void RemoveStaticObject(GameObject obj)
    {
        if (m_staticObjects.Contains(obj))
        {
            m_staticObjects.Remove(obj);
        }
    }

}

public struct SliceDimentions
{
    [SerializeField] public float offset;
    [SerializeField] public float depth;
    [SerializeField] public float width;
}