using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class BestPractice : MonoBehaviour
{
    void Start()
    {
        
    }

    private static readonly int s_Solution_AID = Shader.PropertyToID("Solution_A");
    private static readonly int s_Solution_BID = Shader.PropertyToID("Solution_B");
    private static readonly int s_ColorID = Shader.PropertyToID("color");
    private static readonly int s_PositionID = Shader.PropertyToID("position");
    private static readonly int s_SpawnCountID = Shader.PropertyToID("spawnCount");

    private VisualEffect m_TargetVisualEffect;
    private VFXEventAttribute m_EventAttribute;
    private float m_WaitTime = 1.0f;

    struct SpawnSourceData
    {
        public Vector3 color;
        public Vector3 position;
    }

    void Update()
    {
        m_WaitTime -= Time.deltaTime;
        if (m_WaitTime < 0.0f)
        {
            m_WaitTime = 1.0f;

            if (m_TargetVisualEffect == null)
                m_TargetVisualEffect = GetComponent<VisualEffect>();

            if (m_EventAttribute == null)
                m_EventAttribute = m_TargetVisualEffect.CreateVFXEventAttribute();

            var spawnSources = new SpawnSourceData[3];
            for (int source = 0; source < spawnSources.Length; ++source)
            {
                var color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
                var spawnPosition = Random.insideUnitCircle * 3.0f;
                spawnSources[source] = new SpawnSourceData()
                {
                    color = new Vector3(color.r, color.g, color.b),
                    position = spawnPosition
                };
            }

            uint number_of_particle = 32;

            //Solution A : Simple, one event for every particles, 96 SendEvent
            m_EventAttribute.SetFloat(s_SpawnCountID, (float)1.0f);
            for (int source = 0; source < spawnSources.Length; ++source)
            {
                m_EventAttribute.SetVector3(s_ColorID, spawnSources[source].color);
                for (int hit = 0; hit < number_of_particle; ++hit)
                {
                    var particlePosion = spawnSources[source].position + Random.onUnitSphere * 0.5f;
                    m_EventAttribute.SetVector3(s_PositionID, particlePosion);
                    m_TargetVisualEffect.SendEvent(s_Solution_AID, m_EventAttribute);
                }
            }

            //Solution B : Reduce the amount of data/event doing Random.onUnitSphere in VFX, 3 SendEvent
            m_EventAttribute.SetFloat(s_SpawnCountID, (float)number_of_particle);
            for (int source = 0; source < spawnSources.Length; ++source)
            {
                m_EventAttribute.SetVector3(s_ColorID, spawnSources[source].color);
                m_EventAttribute.SetVector3(s_PositionID, spawnSources[source].position);
                m_TargetVisualEffect.SendEvent(s_Solution_BID, m_EventAttribute);
            }
        }

    }
}
