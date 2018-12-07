using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RebuildSystem : ComponentSystem {

    private List<Entity> ents;

    public struct Group
    { 
        public ComponentDataArray<Position> pos;
        public VoxelData voxDat;
        public int length;
    };

    [Inject] private Group voxGroup;

    protected override void OnUpdate ()
    {
        Debug.Log ("OnUpdate");
        float deltaTime = Time.deltaTime;

        for (int i = 0; i < voxGroup.length; i++)
        {
            var posit = voxGroup.pos [i];
            posit.Value = Vector3.Lerp (voxGroup.voxDat.startPos, voxGroup.voxDat.endPos, voxGroup.voxDat.frac);
            voxGroup.pos [i] = posit;
            voxGroup.voxDat.frac += 0.5f * deltaTime;
        }

        //foreach(var e in GetEntities<Group>())
        //{
        //    var position = e.pos;
        //    position.Value = Vector3.Lerp (e.voxDat.startPos, e.voxDat.endPos, e.voxDat.frac);
        //    e.voxDat.frac += 0.5f * deltaTime;

        //    e.voxDat.MovePos (position.Value);
        //    if (e.voxDat.frac >= 1)
        //    {
        //        e.voxDat.enabled = false;
        //    }

        //}
        
    }

}
