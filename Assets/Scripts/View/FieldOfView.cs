using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFormVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;

    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 MousePosition = Vector3.zero;
        MousePosition.x = SharedContent.MousePosition.x;
        MousePosition.y = SharedContent.MousePosition.y;

        return MousePosition;
    }



    [SerializeField] private float fov = 90f;
    [SerializeField] private int rayCount = 50;
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private LayerMask layerMask;


    // Start is called before the first frame update



    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;
    

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        startingAngle = 0;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
      
        //print("Orin 3" + this.origin);

      

        float angle = startingAngle;
        float angleIncrease = fov/rayCount;
         
 
        Vector3[] vertices = new Vector3[rayCount+1+1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount*3];


        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);

            
            if (raycastHit2D.collider == null)
            {
                // No hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else 
            {
                vertex = raycastHit2D.point;

            }
            vertices[vertexIndex] = vertex; 



            if(i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }


            vertexIndex++;
            angle -= angleIncrease;

        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    public void UpdatePositionAndDirection(Vector3 newOrigin, Vector3 aimDirection)
    {
        SetOrigin(newOrigin);
        SetAimDirection(aimDirection);
    }

    public void SetOrigin(Vector3 newOrigin)
    {
        //print("Or1 " + origin);
        this.origin.x = newOrigin.x;
        this.origin.y = newOrigin.y;
        //print("Or2 " + origin);
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        this.startingAngle = GetAngleFormVectorFloat(aimDirection);
    }


}
