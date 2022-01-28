using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSlice : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_staticObjects;
    [SerializeField] private List<GameObject> m_dynamicObjects;

    [SerializeField] private SliceDimentions m_depthLimits;

    public SliceDimentions sliceDepth
    {
        get { return m_depthLimits; }
        set { m_depthLimits = value; }
    }

    public void AddDynamicObject(GameObject obj)
    {
        if (m_dynamicObjects.Contains(obj))
            return;
        m_dynamicObjects.Add(obj);
        obj.transform.position = new Vector3(transform.position.x, obj.transform.position.y, obj.transform.position.z);
    }


    public void RemoveDynamicObject(GameObject obj)
    {
        if (m_dynamicObjects.Contains(obj))
            m_dynamicObjects.Remove(obj);
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

        foreach (var obj in m_staticObjects)
        {
            obj.transform.localPosition = new Vector3(0, obj.transform.position.y, obj.transform.position.z);
        }
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

    public float GetLowerBound()
    {
        return transform.position.x - (m_depthLimits.depth * 0.5f);
    }

    public float GetUpperBound()
    {
        return transform.position.x + (m_depthLimits.depth * 0.5f);
    }
}

public struct SliceDimentions
{
    [SerializeField] public float offset;
    [SerializeField] public float depth;
    [SerializeField] public float width;
}