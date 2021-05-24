using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipToePlatform : MonoBehaviour
{
    enum State
    {
        Default,
        Touched,
        Dead,
    }

    State state = State.Default;
    MeshRenderer meshRend;
    BoxCollider bCollider;
    public bool isPath;
    public Material defaultMaterial;
    private Shader shader;

    //Variables Touched State
    public Material touchedMaterial;
    float touchedTimer = 0.0f;
    public float maxTouchedTime = 5.0f;

    //Variables Dead State
    float deadTimer = 0.0f;
    public float maxDeadTime = 3.0f;
    public float dissolveTime = 1.0f;
    
    

    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
        meshRend.material = defaultMaterial;
        bCollider = GetComponent<BoxCollider>();
        meshRend.material.SetFloat("_Vanishing",0.0f);
    }

    void Update()
    {
        if (state == State.Dead)
        {
            //Count up timer until respawn of platform
            deadTimer += Time.deltaTime;
            if (deadTimer > maxDeadTime)
            {
                ChangeState(State.Default);
                deadTimer = 0.0f;
                bCollider.enabled = true;
                meshRend.material = defaultMaterial;
            }
            meshRend.material.SetFloat("_VanishingThreshold", Mathf.Clamp01(Mathf.InverseLerp(0,dissolveTime,deadTimer)));
        }
        if (state == State.Touched)
        {
            //Count down timer until reversion to unlit platform
            touchedTimer -= Time.deltaTime;
            if (touchedTimer <= 0.0f)
            {
                ChangeState(State.Default);
                touchedTimer = 0.0f;
                meshRend.material = defaultMaterial;
            }
        }
    }

    private void ChangeState(State s)
    {
        state = s;
    }

    public void CharacterTouches()
    {
        if (!isPath)
        {
            ChangeState(State.Dead);
            bCollider.enabled = false;
        }
        else
        {
            if (state == State.Touched)
            {
                touchedTimer = maxTouchedTime;
            }
            else
            {
                ChangeState(State.Touched);
                touchedTimer = maxTouchedTime;
                meshRend.material = touchedMaterial;
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (isPath)
        {
            Gizmos.DrawCube(transform.position, transform.localScale + Vector3.one * 0.3f);
        }
    }
}
