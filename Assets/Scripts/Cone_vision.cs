using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone_vision : MonoBehaviour
{
    [SerializeField] public float NORMAL_FOV = 40f;
    [SerializeField] public float TARGET_FOV = 20f;

    [SerializeField] public float NORMAL_VIEWDISTANCE = 20f;
    [SerializeField] public float TARGET_VIEWDISTANCE = 40f;

    [SerializeField] private int rayCount = 50;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private double K_CONTROL_EXTEND_VIEWDISTANCE = 0.1;
    [SerializeField] private double K_CONTROL_EXTEND_FOV = 0.1;

    [SerializeField] private bool controlled = true;

    private float reference_fov;
    private float reference_viewDistance;

    private float current_fov;
    private float current_viewDistance;

    private Mesh mesh;
    private Vector3 origin;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;

        reference_fov = NORMAL_FOV;
        reference_viewDistance = NORMAL_VIEWDISTANCE;

        current_fov = NORMAL_FOV;
        current_viewDistance = NORMAL_VIEWDISTANCE;
    }

    private void FixedUpdate()
    {
        ProportionalControlFovExtend(Time.deltaTime);
        ProportionalControlViewDistanceExtend(Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (controlled) control();
        UpdateMesh();

    }

    private void control()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            reference_fov = TARGET_FOV;
            reference_viewDistance = TARGET_VIEWDISTANCE;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            reference_fov = NORMAL_FOV;
            reference_viewDistance = NORMAL_VIEWDISTANCE;
        }
    }

    private void ProportionalControlFovExtend(float deltaT)
    {
        float error = reference_fov - current_fov;
        float force = (float)(K_CONTROL_EXTEND_FOV * error);
        current_fov = Utils.euler(deltaT, current_fov, force, 0.01f, 0.01f);
    }

    private void ProportionalControlViewDistanceExtend(float deltaT)
    {
        float error = reference_viewDistance - current_viewDistance;
        float force = (float)(K_CONTROL_EXTEND_VIEWDISTANCE * error);
        current_viewDistance = Utils.euler(deltaT, current_viewDistance, force, 0.01f, 0.01f);
    }

    public void setTargetExtended()
    {
        reference_fov = TARGET_FOV;
        reference_viewDistance = TARGET_VIEWDISTANCE;
    }

    public void setTargetNormal()
    {
        reference_fov = NORMAL_FOV;
        reference_viewDistance = NORMAL_VIEWDISTANCE;
    }

    public void SetFov(float fov)
    {
        this.current_fov = fov;
    }

    private void UpdateMesh()
    {
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
