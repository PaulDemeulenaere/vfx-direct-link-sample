using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class BasicSample : MonoBehaviour
{
    void Start()
    {
        
    }

    private VisualEffect m_TargetVisualEffect;
    private VFXEventAttribute m_EventAttribute;
    private float m_WaitTime = 0.2f;

    private static readonly int s_FireID = Shader.PropertyToID("fire");
    private static readonly int s_ColorID = Shader.PropertyToID("color");
    private static readonly int s_SpawnCountID = Shader.PropertyToID("spawnCount");

    void Update()
    {
        m_WaitTime -= Time.deltaTime;
        if (m_WaitTime < 0.0f)
        {
            m_WaitTime = 0.2f;

            if (m_TargetVisualEffect == null)
                m_TargetVisualEffect = GetComponent<VisualEffect>();

            if (m_EventAttribute == null)
                m_EventAttribute = m_TargetVisualEffect.CreateVFXEventAttribute();

            //Spawn N Red
            m_EventAttribute.SetFloat(s_SpawnCountID, Random.Range(2.0f, 8.0f));
            m_EventAttribute.SetVector3(s_ColorID, new Vector3(1, 0, 0));
            m_TargetVisualEffect.SendEvent(s_FireID, m_EventAttribute);

            //Spawn N Green
            m_EventAttribute.SetFloat(s_SpawnCountID, Random.Range(2.0f, 8.0f));
            m_EventAttribute.SetVector3(s_ColorID, new Vector3(0, 1, 0));
            m_TargetVisualEffect.SendEvent(s_FireID, m_EventAttribute);

            //Spawn N Blue
            m_EventAttribute.SetFloat(s_SpawnCountID, Random.Range(2.0f, 8.0f));
            m_EventAttribute.SetVector3(s_ColorID, new Vector3(0, 0, 1));
            m_TargetVisualEffect.SendEvent(s_FireID, m_EventAttribute);
        }
    }
}
