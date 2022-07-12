/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using FishNet.Component.Transforming;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ep1
//
//public class FieldOfView : MonoBehaviour
//{
//    #region 구코드
//    /*
//    
//    [SerializeField] private LayerMask layerMask;
//    [SerializeField] private Transform anchor;
//    [SerializeField] private Camera mainCamera;
//    private Mesh mesh;
//    private float fov;
//    private float viewDistance;
//    private Vector3 origin;
//    private float startingAngle;
//
//    private void Start()
//    {
//        mesh = new Mesh();
//        GetComponent<MeshFilter>().mesh = mesh;
//        fov = 90f;
//        viewDistance = 50f;
//        origin = Vector3.zero;
//        mainCamera = Camera.main;
//        anchor = GameObject.Find("Player(Clone)").transform;
//    }
//
//
//    private void LateUpdate()
//    {
//        if (mainCamera == null)
//        {
//            mainCamera = Camera.main;
//            return;
//        }
//        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//        Physics.Raycast(ray, out var hit, float.MaxValue, layerMask);
//        var dir = hit.point - anchor.position;
//        dir.y = 0.1f;
//        dir.Normalize();
//
//        SetAimDirection(dir);
//        SetOrigin(anchor.position);
//
//        int rayCount = 50;
//
//        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
//        Vector2[] uv = new Vector2[vertices.Length];
//        int[] triangles = new int[rayCount * 3];
//
//        vertices[0] = origin;
//
//        int vertexIndex = 1;
//        int triangleIndex = 0;
//        for (int i = 0; i <= rayCount; i++)
//        {
//            Vector3 vertex;
//            var gak = Quaternion.Euler(0, i - 25f, 0) * dir;
//            Physics.Raycast(origin, gak, out var raycastHit, viewDistance, layerMask);
//            if (raycastHit.collider == null)
//            {
//                // No hit
//                vertex = origin + gak * viewDistance;
//            }
//            else
//            {
//                // Hit object
//                vertex = raycastHit.point;
//            }
//            vertices[vertexIndex] = vertex;
//
//            if (i > 0)
//            {
//                triangles[triangleIndex + 0] = 0;
//                triangles[triangleIndex + 1] = vertexIndex - 1;
//                triangles[triangleIndex + 2] = vertexIndex;
//
//                triangleIndex += 3;
//            }
//
//            vertexIndex++;
//        }
//
//
//        mesh.vertices = vertices;
//        mesh.uv = uv;
//        mesh.triangles = triangles;
//        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
//    }
//
//    public void SetOrigin(Vector3 origin)
//    {
//        this.origin = origin;
//    }
//
//    public void SetAimDirection(Vector3 aimDirection)
//    {
//        startingAngle = UtilsClass.GetAngleFromVectorFloat(aimDirection) + fov / 2f;
//    }
//
//    public void SetFoV(float fov)
//    {
//        this.fov = fov;
//    }
//
//    public void SetViewDistance(float viewDistance)
//    {
//        this.viewDistance = viewDistance;
//    }
//
//    */
//    #endregion
//
//    public float veiwRadius;
//    [Range(0, 360)]
//    public float veiwAngle;
//
//    public LayerMask targetMask;
//    public LayerMask obstacleMask;
//
//    [HideInInspector]
//    public List<Transform> visibleTargets = new List<Transform>();
//
//
//    private void Awake()
//    {
//        if (!GetComponent<NetworkTransform>().IsOwner)
//        {
//            gameObject.SetActive(false);
//        }
//    }
//
//    private void Start()
//    {
//        StartCoroutine("FindTargetsWithDelay", 0.2f);
//    }
//
//    IEnumerator FindTargetsWithDelay(float delay)
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(delay);
//            FindVisibleTarget();
//        }
//    }
//
//
//    void FindVisibleTarget()
//    {
//        visibleTargets.Clear();
//        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, veiwRadius, targetMask);
//
//        for (int i = 0; i < targetsInViewRadius.Length; i++)
//        {
//            Transform target = targetsInViewRadius[i].transform;
//            Vector3 dirToTarget = (target.position - transform.position).normalized;
//            if (Vector3.Angle(transform.forward, dirToTarget) < veiwAngle / 2)
//            {
//                float dstToTarget = Vector3.Distance(transform.position, target.position);
//
//                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
//                {
//                    visibleTargets.Add(target);
//                }
//            }
//        }
//    }
//
//
//
//    public Vector3 DirFormAngle(float angleInDegrees, bool angleIsGlobal)
//    {
//        if (!angleIsGlobal)
//        {
//            angleInDegrees += transform.eulerAngles.y;
//        }
//        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
//    }
//}
//
//
#endregion


#region ep2


public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    public GameObject around;
    Mesh viewMesh;


    void Start()
    {

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("FindTargetsWithDelay", .2f);
    }



    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }


    private NetworkTransform NT = null;
    private bool active = false;

    void LateUpdate()
    {
        if (NT == null)
        {
            NT = gameObject.GetComponent<NetworkTransform>();
            return;
        }

        if (!active)
        {
            if (NT.IsOwner)
            {
                active = true;
                around.SetActive(true);
                viewMeshFilter.gameObject.SetActive(true);
            }
            else
                return;
        }

        DrawFieldOfView();
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    target.GetComponent<Invisible>()?.IncreaseVisibleCount(120);
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + new Vector3(0f, 0.1f, 0f);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}

#endregion
