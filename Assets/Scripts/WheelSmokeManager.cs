using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WheelSmoke : IEnumerable<ParticleSystem>
{
    public ParticleSystem FLWheel;
    public ParticleSystem FRWheel;
    public ParticleSystem RLWheel;
    public ParticleSystem RRWheel;

    public IEnumerator<ParticleSystem> GetEnumerator()
    {
        yield return FLWheel;
        yield return FRWheel;
        yield return RLWheel;
        yield return RRWheel;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class WheelSmokeManager : MonoBehaviour
{
    public WheelSmoke wheelSmoke;
    public WheelColliders wheelColls;
    public GameObject smokeParticle;

    void Start()
    {
        CreateSmokes();
    }

    void LateUpdate()
    {
        CheckSlips();
    }

    void CreateSmokes()
    {
        CreateSmoke(ref wheelSmoke.FRWheel, wheelColls.FRWheel);
        CreateSmoke(ref wheelSmoke.FLWheel, wheelColls.FLWheel);
        CreateSmoke(ref wheelSmoke.RLWheel, wheelColls.RLWheel);
        CreateSmoke(ref wheelSmoke.RRWheel, wheelColls.RRWheel);
    }

    void CreateSmoke(ref ParticleSystem smoke, WheelCollider coll)
    {
        smoke = Instantiate(smokeParticle, coll.transform.position - 2 * coll.radius * Vector3.up,
            Quaternion.identity, coll.transform).GetComponent<ParticleSystem>();
    }

    void CheckSlips()
    {
        List<(WheelCollider, ParticleSystem)> wheelPairs = new()
        {
            (wheelColls.FLWheel, wheelSmoke.FLWheel),
            (wheelColls.FRWheel, wheelSmoke.FRWheel),
            (wheelColls.RLWheel, wheelSmoke.RLWheel),
            (wheelColls.RRWheel, wheelSmoke.RRWheel),
        };

        foreach (var (coll, smoke) in wheelPairs)
        {
            coll.GetGroundHit(out WheelHit hit);
            float slipAllowance = 0.7f;
            float totalSlip = Mathf.Abs(hit.sidewaysSlip) + Mathf.Abs(hit.forwardSlip);

            if (totalSlip > slipAllowance) smoke.Play();
            else smoke.Stop();
        }
    }
}