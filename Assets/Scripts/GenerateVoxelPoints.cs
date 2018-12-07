using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public class GenerateVoxelPoints : MonoBehaviour {

    private static EntityManager enMan;
    private static MeshInstanceRenderer voxelRenderer;
    private static EntityArchetype voxelArchetype;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init ()
    {

        enMan = World.Active.GetOrCreateManager<EntityManager> ();

        voxelArchetype = enMan.CreateArchetype (typeof (Position), typeof (CanVoxelise));

    }

}
