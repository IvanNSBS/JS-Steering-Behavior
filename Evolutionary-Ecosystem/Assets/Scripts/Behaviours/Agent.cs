using UnityEngine;

public class Agent : MonoBehaviour {
    public SteerComponent m_SteerBehavior;
    public Genes m_AgentGenes;
    private void Start() {
        m_SteerBehavior = GetComponent<SteerComponent>();
    }

    private void Update()
    {
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2[] targets = {mouse_pos};
        // Seek(new Vector2(10, 0), 10000);
        var s = m_SteerBehavior.Seek(targets, 100000, m_AgentGenes);
        m_SteerBehavior.ApplyForce(s);
    }
}