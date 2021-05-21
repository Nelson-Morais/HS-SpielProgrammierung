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

    //Variables Touched State
    public Material touchedMaterial;
    float touchedTimer = 0.0f;
    public float maxTouchedTime = 5.0f;

    //Variables Dead State
    float deadTimer = 0.0f;
    public float maxDeadTime = 3.0f;

    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
        meshRend.material = defaultMaterial;
        bCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (state == State.Dead)
        {
            //Count down timer until respawn of platform
            deadTimer -= Time.deltaTime;
            if (deadTimer <= 0.0f)
            {
                ChangeState(State.Default);
                deadTimer = 0.0f;
                meshRend.enabled = true;
                bCollider.enabled = true;
                meshRend.material = defaultMaterial;
            }
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
            deadTimer = maxDeadTime;
            meshRend.enabled = false;
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
