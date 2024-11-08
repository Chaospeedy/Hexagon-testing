using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SectionFace{
    public List<Vector3> vertices {get; private set;}
    public List<int> triangles {get; private set;}
    public List<Vector2> uvs {get; private set;}

    public SectionFace (List<Vector3> vertices, List<int> triangles, List<Vector2> uvs){
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
    }
}
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexagonSection : MonoBehaviour
{
    private Mesh m_mesh;
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;

    private List<SectionFace> m_faces;

    public Material material;
    public float innerSize;
    public float outerSize;
    public float height;
    public bool isFlatTopped;
    public int sectionNumber;

    public bool isColliding = false;

    public GameObject collidingWith;


    public void Awake(){
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        m_mesh = new Mesh();

        m_mesh.name = "Hex";

        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = material;
    }

    private void OnEnable(){
        DrawMesh();
    }


    public void DrawMesh(){
        DrawFaces(sectionNumber);
        CombineFaces();
    }

    private void DrawFaces(int sectionNumber){
        m_faces = new List<SectionFace>();

        //top faces
        
        m_faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, sectionNumber));
        

        //bottom faces
        
        m_faces.Add(CreateFace(innerSize, outerSize, -height / 2f, -height / 2f, sectionNumber, true));
        

        //outer faces
        
        m_faces.Add(CreateFace(outerSize, outerSize, height / 2f, -height / 2f, sectionNumber, true));
        

        //inner faces
        
        m_faces.Add(CreateFace(innerSize, innerSize, height / 2f, -height / 2f, sectionNumber, false));
        

    }

    private void CombineFaces(){

        List<Vector3> verticies = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for(int i = 0; i < m_faces.Count; i++){
            verticies.AddRange(m_faces[i].vertices);
            uvs.AddRange(m_faces[i].uvs);

            int offset = (4 * i);
            foreach(int triangle in m_faces[i].triangles){
                tris.Add(triangle + offset);
            }
        }

        m_mesh.vertices = verticies.ToArray();
        m_mesh.triangles = tris.ToArray();
        m_mesh.uv = uvs.ToArray();
        m_mesh.RecalculateNormals();
    }

    private SectionFace CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false){

        Vector3 pointA = GetPoint(innerRad ,heightB, point);
        Vector3 pointB = GetPoint(innerRad ,heightB, (point < 5) ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRad ,heightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad ,heightA, point);

        List<Vector3> vertices = new List<Vector3>() {pointA, pointB, pointC, pointD};
        List<int> triangles = new List<int>() {0, 1, 2, 2, 3, 0};
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)};

        if (reverse){
            vertices.Reverse();
        }

        return new SectionFace(vertices, triangles, uvs);
    }

    protected Vector3 GetPoint(float size, float height, int index){
        float angle_deg = isFlatTopped ? 60 * index : 60*index-30;
        float angle_rad = Mathf.PI / 180f * angle_deg;

        return new Vector3((size * Mathf.Cos(angle_rad)), height, size * Mathf.Sin(angle_rad));
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Section"){
            isColliding = true;
            collidingWith = collision.gameObject;
        }

        
    }

    void OnTriggerExit(Collider collision) {
		if (collision.tag == "Section") {
			isColliding = false;
            collidingWith = null;
		}
	}
}

