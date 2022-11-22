using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField] private int rayCount = 50;
    [SerializeField] private float fov = 40f;
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float minViewDistance = 2f;

    [SerializeField] private LayerMask layerMask;

    private LineRenderer lineRender;

    private Mesh mesh;
    private List<Vector2> edges = new List<Vector2>();
    private EdgeCollider2D detector;
    private MeshRenderer vision;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        detector = GetComponent<EdgeCollider2D>();
        lineRender = GetComponent<LineRenderer>();
        vision = GetComponent<MeshRenderer>();
        
    }

    private void Update()
    {
        UpdateMesh();
        UpdateLine();


    }

    private void UpdateMesh()
    {
        float current_fov = fov;
        float current_viewDistance = viewDistance;

        Vector3 origin = Vector3.zero;
        float angle = current_fov / 2f;
        float angleIncrease = current_fov / rayCount;

        Vector3[] vertices = new Vector3[2*(rayCount+1)];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[2 * (rayCount)* 3];

        int vertexIndex = 0;
        int triangleIndex = 0;

        float angleDirection = transform.rotation.eulerAngles.z * Mathf.PI / 180;
        Vector2 direction = new Vector2(Mathf.Cos(angleDirection), Mathf.Sin(angleDirection));
        Vector2 realPosition = transform.position;
        float realAngle = Utils.GetAngleFormVectorFloat(direction) + angle;

        edges.Clear();
        for (int i = 0; i <= rayCount; i++)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(realPosition, Utils.GetVectorFromAngle(realAngle), current_viewDistance, layerMask);

            float distance = current_viewDistance;
            if (raycastHit2D.collider != null)
            {
                distance = (raycastHit2D.point - realPosition).magnitude;
            }

            Vector3 vertexProx = origin + Utils.GetVectorFromAngle(angle) * minViewDistance;
            Vector3 vertexLonge = origin + Utils.GetVectorFromAngle(angle) * distance;

            vertices[vertexIndex] = vertexProx;
            vertices[vertexIndex+1] = vertexLonge;

            if(i == 0)
            {
                edges.Add(vertexProx);
            }

            if (i > 0)
            {
                edges.Add(vertices[vertexIndex - 1]);
                triangles[triangleIndex + 0] = vertexIndex - 1;
                triangles[triangleIndex + 1] = vertexIndex + 0;
                triangles[triangleIndex + 2] = vertexIndex + 1;

                triangleIndex += 3;
            }

            if(i < rayCount)
            {
                triangles[triangleIndex + 0] = vertexIndex + 0;
                triangles[triangleIndex + 1] = vertexIndex + 1;
                triangles[triangleIndex + 2] = vertexIndex + 2;

                triangleIndex += 3;
            }

            if(i == rayCount)
            {
                edges.Add(vertexLonge);
                edges.Add(vertexProx);
            }


            
            vertexIndex += 2;
            angle -= angleIncrease;
            realAngle -= angleIncrease;

        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);

        detector.SetPoints(edges);
    }

    public void UpdateLine()
    {
        lineRender.positionCount = edges.Count;
        for (int index = 0; index < edges.Count; index++) {
            Vector3 point = new Vector3(edges[index].x, edges[index].y);
            lineRender.SetPosition(index, point);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player")) Detected();
    }

    public void Show()
    {
        vision.enabled = true;
        lineRender.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Vision");
    }

    public void Hide()
    {
        vision.enabled = false;
        lineRender.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("HUD");
    }

    [SerializeField] private float MINIMUNS_RADIUS_FOR_WARNING = 10;
    private void Detected()
    {
        if (!isAlert) StartCoroutine(AlertEnemys());
    }

    private bool isAlert = false;
    private IEnumerator AlertEnemys()
    {
        isAlert = true;
        yield return new WaitForSeconds(5f);
        
        Vector2 current_position = this.gameObject.transform.position;
        foreach (Enemy enemy in Enemy.enemyList)
        {
            Vector2 deltaDist = current_position - (Vector2)enemy.transform.position;
            if ((deltaDist.magnitude) <= MINIMUNS_RADIUS_FOR_WARNING)
            {
                enemy.EnterChase();
            }
        }
        isAlert = false;
    }
}
