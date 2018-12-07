using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Rotator : MonoBehaviour {

    public float speed;
	
}

class RotatorSystem : ComponentSystem
{

    struct Components
    {
        public Rotator rotator;
        public Transform transform;
    }

    protected override void OnUpdate ()
    {
        float deltaTime = Time.deltaTime;
        foreach(var e in GetEntities<Components> ())
        {
            e.transform.Rotate (Vector3.one * e.rotator.speed * deltaTime);
        }
    }

}