using UnityEngine;
using System.Collections;

public class MorphingAvatar : MonoBehaviour
{

    [SerializeField]
    private Mesh _dstMesh;
    [SerializeField]
    private Mesh _srcMesh;

    private Mesh _mesh;
    private bool _initDone;
    private float _time;
    private float _speed;

    void Start()
    {
        _time = 0.0f;
        _mesh = Instantiate(srcMesh) as Mesh;

        GetComponent<SkinnedMeshRenderer>().sharedMesh = _mesh;

        _initDone = true;
        if (dstMesh == null)
        {
            _initDone = false;
            return;
        }

        if (srcMesh == null)
        {
            _initDone = false;
            return;
        }

        if (dstMesh.vertexCount != srcMesh.vertexCount)
        {
            _initDone = false;
            return;
        }
    }

    void Update()
    {
        if (_initDone)
        {
            float deltaTime = Time.deltaTime * _speed;
            _time += deltaTime;
            float tmp = Mathf.Clamp(_time, 0, 1);
            Morph(tmp);
        }
    }

    void Morph(float t)
    {
        Vector3[] v0 = srcMesh.vertices;
        Vector3[] v1 = dstMesh.vertices;
        Vector3[] vdst = new Vector3[_mesh.vertexCount];
        for (int i = 0; i < vdst.Length; i++)
        {
            vdst[i] = Vector3.Lerp(v0[i], v1[i], t);
        }
        GetComponent<SkinnedMeshRenderer>().sharedMesh.vertices = vdst;
        GetComponent<SkinnedMeshRenderer>().sharedMesh.RecalculateBounds();
    }

    public Mesh dstMesh
    {
        get
        {
            return _dstMesh;
        }
        set
        {
            _dstMesh = value;
        }
    }

    public Mesh srcMesh
    {
        get
        {
            return _srcMesh;
        }
        set
        {
            _srcMesh = value;
        }
    }

    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }
}