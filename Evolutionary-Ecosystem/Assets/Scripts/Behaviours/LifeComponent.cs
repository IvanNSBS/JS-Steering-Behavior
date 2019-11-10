using UnityEngine;

public class LifeComponent : MonoBehaviour {

    private float m_EnergyUsage = 0.7f;
    [HideInInspector] public Rigidbody2D m_RigidBody;
    [Header("Life Parameters")]
    public float m_LifeTime = 500.0f;
    public float m_TimeToDeathByHunger = 150.0f;
    public float m_TimeToDeathByThirst = 80.0f;
    public float m_TotalHealth = 1.0f;
    public float m_TotalEnergy = 100.0f;
    public float m_TotalReproductionUrge = 100.0f;
    public float m_RemainingLifetime;
    public float m_CurrentHealth, m_CurrentEnergy;
    public float m_CurrentHunger, m_CurrentThirst;
    public float m_CurrentReproductionUrge;
    [HideInInspector] public Genes m_AgentGenes;
    private void Start() {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_AgentGenes = GetComponent<Agent>().m_AgentGenes;
        m_RemainingLifetime = m_LifeTime;
        m_CurrentHunger = 0;
        m_CurrentThirst = 0;
        m_CurrentEnergy = m_TotalEnergy;
        m_CurrentReproductionUrge = 0;
    }

    private void Update() {
        m_RemainingLifetime -= Time.deltaTime/m_LifeTime;
        m_CurrentEnergy -= Time.deltaTime*m_RigidBody.velocity.magnitude*m_EnergyUsage;
        m_CurrentHunger += Time.deltaTime/m_TimeToDeathByHunger;
        m_CurrentThirst += Time.deltaTime/m_TimeToDeathByThirst;


        var sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.Lerp(Color.red, Color.green, m_RemainingLifetime/m_LifeTime);
    }
}