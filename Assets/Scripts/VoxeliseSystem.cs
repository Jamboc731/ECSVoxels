using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Entities;

public class VoxeliseSystem : ComponentSystem {

    struct Components
    {
        public Voxelisable voxelisable;
        public Transform transform;
    }

    protected override void OnStartRunning ()
    {

        foreach(var e in GetEntities<Components> ())
        {
            e.voxelisable.mesh = e.voxelisable.sMRend.sharedMesh;
            e.voxelisable.verts = e.voxelisable.mesh.vertices;
            e.voxelisable.voxels = new List<GameObject> ();
            e.voxelisable.points = new List<GameObject> ();
            for (int i = 0; i < e.voxelisable.verts.Length; i++)
            {
                if(Physics.OverlapBox(e.transform.TransformPoint(e.voxelisable.verts[i]), e.voxelisable.voxel.transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Voxels")).Length < 1)
                {
                    GameObject inst = e.voxelisable.SpawnVoxel (e.transform.TransformPoint(e.voxelisable.verts [i]));
                    e.voxelisable.voxels.Add (inst);
                    e.voxelisable.points.Add (new GameObject());
                    //e.voxelisable.points [i].transform.position = e.voxelisable.transform.TransformPoint(e.voxelisable.verts [i]);
                }
            }
            

            e.voxelisable.bones = e.voxelisable.sMRend.bones;
            int j = 0;
            foreach(GameObject vox in e.voxelisable.voxels)
            {
                e.voxelisable.points [j].transform.position = vox.transform.position;
                j++;
                float dist = Mathf.Infinity;
                float curDist = 0;
                Transform closestBone = e.transform;
                for(int i = 0; i < e.voxelisable.bones.Length; i++)
                {
                    curDist = Vector3.Distance (vox.transform.position, e.voxelisable.bones [i].position);
                    if(curDist < dist)
                    {
                        dist = curDist;
                        closestBone = e.voxelisable.bones [i];
                    }
                }
                vox.transform.SetParent (closestBone);

                Color color;
                RaycastHit hit;
                Ray ray = new Ray (vox.transform.position - ((closestBone.position) - vox.transform.position), closestBone.position - vox.transform.position);
                if(Physics.Raycast(ray, out hit, 2, LayerMask.GetMask ("Default")))
                {
                    e.voxelisable.tex = (Texture2D) hit.transform.gameObject.GetComponent<SkinnedMeshRenderer> ().material.mainTexture;
                    color = e.voxelisable.tex.GetPixel ((int) (hit.textureCoord [0] * e.voxelisable.tex.width), (int) (hit.textureCoord [1] * e.voxelisable.tex.height));
                    vox.GetComponent<MeshRenderer> ().material.color = color;
                }
            }
            e.voxelisable.sMRend.sharedMesh = null;
            e.voxelisable.DestroyCollider ();
 
            e.voxelisable.tempVox = e.voxelisable.voxels.ToArray ();
            e.voxelisable.voxels = e.voxelisable.tempVox.OrderBy (tempVox => tempVox.transform.position.y).ToList ();

            e.voxelisable.voxels.Reverse();
        }

        

    }

    protected override void OnUpdate()
    {
        foreach(var e in GetEntities <Components>())
        {
            if (e.voxelisable.die)
            {
                e.voxelisable.StartDissolve ();
            }

            if (e.voxelisable.undie)
            {

                if (e.voxelisable.started)
                {
                    int l = 0;
                    foreach(GameObject vox in e.voxelisable.voxels)
                    {
                        vox.AddComponent<VoxelData> ();
                        vox.GetComponent<VoxelData> ().endPos = e.voxelisable.points [l].transform.position;
                        vox.GetComponent<VoxelData> ().startPos = vox.transform.position;
                        vox.GetComponent<Rigidbody> ().isKinematic = true;
                        vox.GetComponent<VoxelData> ().enabled = true;
                        l++;
                        e.voxelisable.undie = false;
                    }
                    //e.voxelisable.started = false;
                    //e.voxelisable.voxStarts = new List<Vector3> ();

                    //foreach (GameObject vox in e.voxelisable.voxels)
                    //{
                    //    e.voxelisable.voxStarts.Add (vox.transform.TransformPoint (vox.transform.position));
                    //    vox.GetComponent<Rigidbody> ().isKinematic = true;
                    //}
                    //e.voxelisable.frac = 0;
                }
                e.voxelisable.cur = 0;

                ////////////////////////////

                //foreach (GameObject vox in voxels)
                //{
                //    Debug.Log (points [cur].transform.position);
                //    vox.transform.position = Vector3.Lerp (startPs [cur], points [cur].transform.position, frac);
                //    cur++;
                //}

                ////////////////////////////

                //foreach (GameObject vox in e.voxelisable.voxels)
                //{
                //    vox.transform.position = Vector3.Lerp (e.voxelisable.voxStarts [e.voxelisable.cur], e.voxelisable.points [e.voxelisable.cur].transform.position, e.voxelisable.frac);
                //    Debug.Log (string.Format ("{0}, {1}", e.voxelisable.cur, e.voxelisable.points[e.voxelisable.cur].transform.position));
                //    e.voxelisable.cur++;
                //    e.voxelisable.cur = Mathf.Clamp (e.voxelisable.cur, 0, e.voxelisable.voxels.Count-1);
                //}
                //e.voxelisable.frac += 0.5f * Time.deltaTime;
                //if (e.voxelisable.frac >= 1)
                //{
                //    e.voxelisable.undie = false;
                //}
            }

        }
    }
}
