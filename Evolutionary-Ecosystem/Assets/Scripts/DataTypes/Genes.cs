using UnityEngine;


[System.Serializable]
public class Genes
{
    [SerializeField] public float m_MaxSpeed = 5.0f;
    [SerializeField] public float m_MaxForce = 5.0f;
    [SerializeField] public float m_MaxBrake = 20.0f;
    public float m_SightRadius = 2.0f;
    public float m_ForgetRadius = 4.0f;
    float[] m_Genes;
    bool m_IsMale;

    // Genes()
    // {
    //     // m_IsMale = Random.Range(0.0f, 1.0f) < 0.5;
    //     m_MaxSpeed = 5.0f;
    //     m_MaxForce = 5.0f;
    // }
}