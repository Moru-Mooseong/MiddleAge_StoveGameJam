using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTone<T> : MonoBehaviour where T : SingleTone<T>
{
    protected static T m_instance;
    public static T instance { get { if (m_instance == null) m_instance = FindObjectOfType<T>(); return m_instance; } }

    protected virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = (T)this;
        }
        else if (m_instance != this)
        {
            GameObject.Destroy(gameObject);
            return;
        }
    }




}