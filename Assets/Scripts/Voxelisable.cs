using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxelisable : MonoBehaviour {

    public SkinnedMeshRenderer sMRend;
    public GameObject voxel;
    [HideInInspector]
    public Mesh mesh;
    [HideInInspector]
    public Vector3 [] verts;
    [HideInInspector]
    public Transform [] bones;
    [HideInInspector]
    public List<GameObject> voxels;
    [HideInInspector]
    public Texture2D tex;
    public int dealthDelay;
    [HideInInspector]
    public List<GameObject> points;
    [HideInInspector]
    public GameObject [] tempVox;
    [HideInInspector]
    public List<Vector3> voxStarts;
    public bool undie;
    public bool die;
    [HideInInspector]
    public bool started = true;
    [HideInInspector]
    public float frac;
    [HideInInspector]
    public int cur;

    public GameObject SpawnVoxel(Vector3 pos)
    {
        return (Instantiate (voxel, pos, Quaternion.identity));
    }

    public void DestroyCollider ()
    {
        Destroy (this.gameObject.GetComponent<Collider> ());
    }

    public void StartDissolve ()
    {
        StartCoroutine (Dissolve (dealthDelay, voxels));
        die = false;
    }

    public IEnumerator Dissolve (int delay, List<GameObject> voxels)
    {
        yield return new WaitForSeconds (delay);
        for (int i = 0; i < voxels.Count; i += 4)
        {
            try
            {
                for (int a = 0; a < 4; a++)
                {

                    voxels [i + a].transform.parent = null;
                    voxels [i + a].GetComponent<Rigidbody> ().isKinematic = false;
                    voxels [i + a].GetComponent<MeshRenderer> ().material.color = Color.red;
                    //voxels[i + a].GetComponent<Rigidbody>().AddExplosionForce(250, transform.position, 5);

                }

            }
            catch
            {
            
            }
            yield return new WaitForSeconds (0.00001f);
        }
        yield return new WaitForSeconds (3);
        undie = true;
    }

}
