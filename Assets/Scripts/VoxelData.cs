using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class VoxelData : MonoBehaviour {

    public Position pos;
    public Vector3 endPos;
    public Vector3 startPos;
    public float frac;

    public void MovePos(Vector3 _pos)
    {
        pos.Value = _pos;
    }

}
