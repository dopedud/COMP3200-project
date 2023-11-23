using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAILooker : MonoBehaviour {

    [SerializeField, Min(0)] private float _viewRadius;
    public float viewRadius { get { return _viewRadius; } }
    [SerializeField, Range(0, 360)] private float _viewAngle;
    public float viewAngle { get { return _viewAngle; } }

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;

    private Transform _player;
    public Transform player { get { return _player; } }

    [SerializeField] private float meshResolution, edgeDstThreshold;
	[SerializeField] private int edgeResolveIterations;
	
	private float maskCutawayDst = .1f;

	private MeshFilter viewMeshFilter;
	private Mesh viewMesh;

	private void Awake() {
		viewMeshFilter = GetComponent<MeshFilter>();
        viewMesh = new Mesh { name = "View Mesh" };
        viewMeshFilter.mesh = viewMesh;
	}

    private void Update() => FindPlayer();
	private void LateUpdate() => DrawFOV();

    private void FindPlayer() {
        Collider[] targets = Physics.OverlapSphere(transform.position, _viewRadius, playerMask);

        if (targets.Length == 0) return;

        Transform player = targets[0].transform;

        float dist = Vector3.Distance(player.localPosition, transform.localPosition);
        Vector3 dir = (player.localPosition - transform.localPosition).normalized;

        if (Vector3.Angle(transform.forward, dir) > _viewAngle / 2) return;
        if (Physics.Raycast(transform.position, dir, dist, obstacleMask)) return;

        _player = player;
    }

    private void DrawFOV() {
        	int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		    float stepAngleSize = viewAngle / stepCount;
		    List<Vector3> viewPoints = new List<Vector3>();
		    ViewCastInfo oldViewCast = new ViewCastInfo();
		    for (int i = 0; i <= stepCount; i++) {
			    float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			    ViewCastInfo newViewCast = ViewCast(angle);

			    if (i > 0) {
				    bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
				    if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded)) {
					    EdgeInfo edge = FindEdge (oldViewCast, newViewCast);
					    if (edge.pointA != Vector3.zero) viewPoints.Add (edge.pointA);
						if (edge.pointB != Vector3.zero) viewPoints.Add (edge.pointB);
					}

				}

				viewPoints.Add (newViewCast.point);
				oldViewCast = newViewCast;
			}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount-2) * 3];

		vertices [0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++) {
			vertices [i + 1] = transform.InverseTransformPoint(viewPoints [i]) + Vector3.forward * maskCutawayDst;

			if (i < vertexCount - 2) {
				triangles [i * 3] = 0;
				triangles [i * 3 + 1] = i + 1;
				triangles [i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear();

		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
    }

    public Vector3 DirFromAngle(float angle, bool global) {
        if (!global) angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

	private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < edgeResolveIterations; i++) {
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast (angle);

			bool edgeDstThresholdExceeded = Mathf.Abs (minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
			if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
				minAngle = angle;
				minPoint = newViewCast.point;
			} else {
				maxAngle = angle;
				maxPoint = newViewCast.point;
			}
		}

		return new EdgeInfo (minPoint, maxPoint);
	}

	private ViewCastInfo ViewCast(float globalAngle) {
		Vector3 dir = DirFromAngle (globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast (transform.position, dir, out hit, viewRadius, obstacleMask)) 
		return new ViewCastInfo (true, hit.point, hit.distance, globalAngle); 
		else return new ViewCastInfo (false, transform.position + dir * viewRadius, viewRadius, globalAngle);
	}

	public struct ViewCastInfo {
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(bool hit, Vector3 point, float dst, float angle) {
			this.hit = hit;
			this.point = point;
			this.dst = dst;
			this.angle = angle;
		}
	}

	public struct EdgeInfo {
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 pointA, Vector3 pointB) {
			this.pointA = pointA;
			this.pointB = pointB;
		}
	}

}
