using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone_vision : MonoBehaviour
{
    [SerializeField] public static float NORMAL_FOV = 40f;
    [SerializeField] public static float NORMAL_VIEWDISTANCE = 10f;

    [SerializeField] private int rayCount = 50;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float K_CONTROL_EXTEND_VIEWDISTANCE = 0.1f;
    [SerializeField] private float K_CONTROL_EXTEND_FOV = 0.1f;

    private Model1 fov;
    private Model1 viewDistance;

    private Mesh mesh;
    private Vector3 origin;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;

        fov = new Model1(0.01f, 0.01f, NORMAL_FOV);
        viewDistance = new Model1(0.01f, 0.01f, NORMAL_VIEWDISTANCE);

    }

    private void FixedUpdate()
    {
        fov.ProportionalControl(Time.deltaTime, K_CONTROL_EXTEND_FOV);
        viewDistance.ProportionalControl(Time.deltaTime, K_CONTROL_EXTEND_VIEWDISTANCE);
    }

    private void LateUpdate()
    {
        UpdateMesh();
    }

    public void SetReferenceFov(float value)
    {
        fov.SetReferenceValue(value);
    }

    public void SetReferenceViewDistance(float value)
    {
        viewDistance.SetReferenceValue(value);
    }

    private void UpdateMesh()
    {
        float current_fov = fov.GetCurrentValue();
        float current_viewDistance = viewDistance.GetCurrentValue();

        float angle = current_fov / 2f;
        float angleIncrease = current_fov / rayCount;
        
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];


        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        Vector2 direction = (Utils.GetMouseWorldPosition() - transform.position).normalized;
        Vector2 realPosition = transform.position;
        float realAngle = Utils.GetAngleFormVectorFloat(direction) + angle;

        for (int i = 0; i <= rayCount; i++)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(realPosition, Utils.GetVectorFromAngle(realAngle), current_viewDistance, layerMask);

            float distance = current_viewDistance;
            if (raycastHit2D.collider != null)
            {
                 distance = (raycastHit2D.point-realPosition).magnitude;
            }

            Vector3 vertex = origin + Utils.GetVectorFromAngle(angle) * distance;
            vertices[vertexIndex] = vertex;


            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
            realAngle -= angleIncrease;

        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(this.origin, Vector3.one * 1000f);
    }

}
