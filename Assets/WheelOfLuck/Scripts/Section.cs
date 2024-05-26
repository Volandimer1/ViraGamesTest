using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Section : MonoBehaviour
{
    public int _sortingOrder = 101;
    public MeshFilter _meshFilter;
    public MeshRenderer _meshRenderer;
    public SpriteRenderer _spriteRenderer;
    public GameObject _imagePivot;
    private Mesh _mesh;

    private const float _radius = 2.7f;

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void CreateSection(Sprite sprite, float angle)
    {
        if (sprite != null)
        {
            _spriteRenderer.sprite = sprite;
        }

        if (angle < 45)
        {
            Vector3 originalScale = _spriteRenderer.transform.localScale;
            _spriteRenderer.transform.localScale = new Vector3(originalScale.x * (angle/45), originalScale.y, originalScale.z);
        }

        _mesh = new Mesh();

        float angleRad = angle * Mathf.Deg2Rad;
        Vector3[] vertices;
        int[] triangles;

        _imagePivot.transform.eulerAngles = new Vector3(0, 0, (angle / 2) - 90);

        if (angle <= 90)
        {
            vertices = new Vector3[4];
            triangles = new int[6];


            vertices[0] = new Vector3(0, 0);
            vertices[1] = new Vector3(Mathf.Cos(0) * _radius, Mathf.Sin(0) * _radius);
            vertices[2] = new Vector3(Mathf.Cos(angleRad) * _radius, Mathf.Sin(angleRad) * _radius);
            vertices[3] = new Vector3(_radius, _radius);

            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;
            triangles[3] = 2;
            triangles[4] = 3;
            triangles[5] = 1;
        }
        else
        {
            vertices = new Vector3[5];
            triangles = new int[9];


            vertices[0] = new Vector3(0, 0);
            vertices[1] = new Vector3(Mathf.Cos(0) * _radius, Mathf.Sin(0) * _radius);
            vertices[2] = new Vector3(Mathf.Cos(angleRad) * _radius, Mathf.Sin(angleRad) * _radius);
            vertices[3] = new Vector3(_radius, _radius);
            vertices[4] = new Vector3(-_radius, _radius);

            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;
            triangles[3] = 2;
            triangles[4] = 3;
            triangles[5] = 1;
            triangles[6] = 2;
            triangles[7] = 4;
            triangles[8] = 3;
        }

        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _meshFilter.mesh = _mesh;
        _meshRenderer.sortingOrder = _sortingOrder;
    }

    public void RotateSection(float angle)
    {
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
