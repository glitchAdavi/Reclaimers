using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using static UnityEngine.EventSystems.EventTrigger;

[ExecuteAlways]
public class TileService : MonoBehaviour
{
    [SerializeField] private Grid grid;

    [SerializeField] private Tilemap cTilemap;
    [SerializeField] private List<Vector3Int> cTiles = new List<Vector3Int>();
    private List<Vector3Int> cTilesAux = new List<Vector3Int>();

    [SerializeField] private Tilemap fTilemap;
    [SerializeField] private List<Vector3Int> fTiles = new List<Vector3Int>();
    private List<Vector3Int> fTilesAux = new List<Vector3Int>();

    [SerializeField] private Tilemap fDetailsTilemap;
    [SerializeField] private Tilemap wTilemap;
    [SerializeField] private Tilemap wTopTilemap;

    [SerializeField] private GameObject meshParent;
    [SerializeField] private List<GameObject> meshList = new List<GameObject>();
    [SerializeField] private GameObject colliderParent;
    [SerializeField] private List<GameObject> colliderList = new List<GameObject>();

    private List<List<Vector3Int>> groups = new List<List<Vector3Int>>();
    private int globalMax = 64;



    public bool generateMeshes = false;
    public bool clearMeshes = false;
    public bool generateColliders = false;
    public bool clearColliders = false;
    public bool generateAll = false;
    public bool clearAll = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (generateAll)
        {
            generateAll = false;
            ProcessMesh();
            ProcessCollider();
        }

        if (generateMeshes)
        {
            generateMeshes = false;
            ProcessMesh();
        }

        if (generateColliders)
        {
            generateColliders = false;
            ProcessCollider();
        }

        if (clearAll)
        {
            clearAll = false;
            meshParent.GetComponent<NavMeshSurface>().RemoveData();
            DestroyChildren(meshParent);
            meshList.Clear();
            DestroyChildren(colliderParent);
            colliderList.Clear();
        }

        if (clearMeshes)
        {
            clearMeshes = false;
            meshParent.GetComponent<NavMeshSurface>().RemoveData();
            DestroyChildren(meshParent);
            meshList.Clear();
        }

        if (clearColliders)
        {
            clearColliders = false;
            DestroyChildren(colliderParent);
            colliderList.Clear();
        }

    }

    #region Mesh
    private void ProcessMesh()
    {
        DestroyChildren(meshParent);
        fTiles = GetAllTiles(fTilemap);

        fTiles.Sort((x, y) => Vector3IntSort(x, y));
        fTilesAux = new List<Vector3Int>(fTiles);

        groups.Clear();
        groups = GenerateMeshes();

        SpawnMeshes();

        BakeMeshes();
    }

    private List<List<Vector3Int>> GenerateMeshes()
    {
        int max = globalMax;

        List<List<Vector3Int>> allMeshes = new List<List<Vector3Int>>();

        while(fTilesAux.Count > 0 && max > 0)
        {
            List<Vector3Int> corner = new List<Vector3Int>();
            corner.Add(fTilesAux[0]);
            List<Vector3Int> mesh = CalculateGroup(fTilesAux);

            if (mesh.Count > 0)
            {
                mesh.Sort((x, y) => Vector3IntSort(x, y));
                allMeshes.Add(mesh);
                foreach (Vector3Int v in mesh)
                {
                    fTilesAux.Remove(v);
                }
                fTilesAux.Sort((x, y) => Vector3IntSort(x, y));
                Debug.Log(fTilesAux.Count);
                max--;
            } else
            {
                Debug.Log("Empty Mesh");
            }
        }

        return allMeshes;
    }

    private void SpawnMeshes()
    {
        if (groups.Count <= 0) return;
        meshList.Clear();

        foreach (List<Vector3Int> v in groups)
        {
            Vector3 center = GetCenterOfCollection(v);
            center = center + new Vector3(grid.cellSize.x / 2, 0f, grid.cellSize.y / 2);
            Vector3 size = (v[v.Count - 1] - v[0]) + new Vector3(1, 1, 1);
            size = new Vector3(size.x * grid.cellSize.x, size.y * grid.cellSize.y, size.z * grid.cellSize.z);

            GameObject mesh = new GameObject("NavMeshPlatform");
            mesh.isStatic = true;
            mesh.transform.parent = meshParent.transform;

            MeshFilter filter = mesh.AddComponent<MeshFilter>();
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
            filter.mesh = go.GetComponent<MeshFilter>().sharedMesh;
            MeshRenderer renderer = mesh.AddComponent<MeshRenderer>();
            renderer.materials = new Material[0];
            DestroyImmediate(go);
            
            //GameObject mesh = GameObject.CreatePrimitive(PrimitiveType.Plane);

            mesh.transform.position = new Vector3(center.x, 0f, center.z);
            mesh.transform.localScale = new Vector3(size.x / 10, 1f, size.y / 10);
            //mesh.transform.Rotate(Vector3.right, 90f);

            meshList.Add(mesh);
        }
    }

    private void BakeMeshes()
    {
        meshParent.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    #endregion

    #region Collider
    private void ProcessCollider()
    {
        DestroyChildren(colliderParent);
        cTiles = GetAllTiles(cTilemap);

        cTiles.Sort((x, y) => Vector3IntSort(x, y));
        cTilesAux = new List<Vector3Int>(cTiles);

        groups.Clear();
        groups = GenerateColliders();

        SpawnColliders();
    }

    private List<List<Vector3Int>> GenerateColliders()
    {
        int max = globalMax;

        List<List<Vector3Int>> allColliders = new List<List<Vector3Int>>();

        while(cTilesAux.Count > 0 && max > 0)
        {
            List<Vector3Int> corner = new List<Vector3Int>();
            corner.Add(cTilesAux[0]);
            List<Vector3Int> collider = CalculateGroup(cTilesAux);

            if (collider.Count > 0)
            {
                collider.Sort((x, y) => Vector3IntSort(x, y));
                allColliders.Add(collider);
                foreach(Vector3Int v in collider)
                {
                    cTilesAux.Remove(v);
                }
                cTilesAux.Sort((x, y) => Vector3IntSort(x, y));
                max--;
            }
            else
            {
                Debug.Log("Empty Collider");
            }
        }

        return allColliders;
    }

    private void SpawnColliders()
    {
        if (groups.Count <= 0) return;
        colliderList.Clear();

        float extra = 0.05f;

        foreach (List<Vector3Int> v in groups)
        {
            Vector3 center = GetCenterOfCollection(v);
            center = center + new Vector3(grid.cellSize.x / 2, 0f, grid.cellSize.y / 2);
            Vector3 size = (v[v.Count - 1] - v[0]) + new Vector3(1, 1, 1);
            size = new Vector3(size.x * grid.cellSize.x + extra, size.z * grid.cellSize.z + 2.8f, size.y * grid.cellSize.y + extra);

            GameObject collider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            collider.name = "EdgeCollider";
            collider.isStatic = true;
            collider.transform.parent = colliderParent.transform;
            collider.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            collider.layer = 22;

            collider.transform.localScale = size;

            collider.transform.position = new Vector3(center.x, 1.4f, center.z);

            colliderList.Add(collider);
        }

    }



    #endregion

    private List<Vector3Int> CalculateGroup(List<Vector3Int> toCheck)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        List<Vector3Int> tempX = new List<Vector3Int>();
        List<Vector3Int> tempY = new List<Vector3Int>();

        Vector3Int currentStart = toCheck[0];
        Vector3Int currentCorner = Vector3Int.zero;
        Vector3Int currentAdd = new Vector3Int(1, 1, 0);

        if (toCheck.Contains(currentStart)) result.Add(currentStart);

        for (int i = 1; i < 128; i++)
        //while (currentAdd.x > 0 && currentAdd.y > 0)
        {
            tempX.Clear();
            tempY.Clear();

            if (currentAdd.x < 1 && currentAdd.y < 1) break;

            if (!toCheck.Contains(currentCorner + currentAdd + currentStart))
            {
                if (toCheck.Contains(currentCorner + currentStart + new Vector3Int(1, 0, 0)))
                {
                    currentAdd.y = 0;
                }
                else if (toCheck.Contains(currentCorner + currentStart + new Vector3Int(0, 1, 0)))
                {
                    currentAdd.x = 0;
                }
                else break;
            }

            currentCorner += currentAdd;
            Debug.Log(currentAdd);

            if (currentAdd.x > 0)
            {
                for (int k = 1; k <= currentCorner.y; k++)
                {
                    if (toCheck.Contains(currentCorner - new Vector3Int(0, k, 0) + currentStart))
                    {
                        tempY.Add(currentCorner - new Vector3Int(0, k, 0) + currentStart);
                    }
                    else
                    {
                        Debug.Log($"Failed in {new Vector3Int(currentCorner.x, k, 0)}");
                        tempY.Clear();
                        currentAdd.x = 0;
                        currentCorner -= new Vector3Int(1, 0, 0);
                        break;
                    }
                }
            }

            if (currentAdd.y > 0)
            {
                for (int j = 1; j <= currentCorner.x; j++)
                {
                    if (toCheck.Contains(currentCorner - new Vector3Int(j, 0, 0) + currentStart))
                    {
                        tempX.Add(currentCorner - new Vector3Int(j, 0, 0) + currentStart);
                    }
                    else
                    {
                        Debug.Log($"Failed in {new Vector3Int(j, currentCorner.y, 0)}");
                        tempX.Clear();
                        currentAdd.y = 0;
                        currentCorner -= new Vector3Int(0, 1, 0);
                        break;
                    }
                }
            }

            if (currentAdd.x > 0 || currentAdd.y > 0) result.Add(currentCorner + currentStart);
            foreach (Vector3Int v in tempX)
            {
                result.Add(v);
            }
            foreach (Vector3Int v in tempY)
            {
                result.Add(v);
            }
        }

        return result;
    }


    #region PawnInfo
    public Vector3Int GetPawnTilePos(Pawn p)
    {
        return grid.WorldToCell(p.transform.position);
    }

    public List<Vector3Int> GetAllTilesInRangeFromPos(Pawn p, int width, int height, int thickness = 1)
    {
        return GetAllTilesInRangeFromPos(GetPawnTilePos(p), width, height, thickness);
    }
    public List<Vector3Int> GetAllTilesInRangeFromPos(Vector3 pos, int width, int height, int thickness = 1)
    {
        return GetAllTilesInRangeFromPos(Vector3Int.FloorToInt(pos), width, height, thickness);
    }
    public List<Vector3Int> GetAllTilesInRangeFromPos(Vector3Int pos, int width, int height, int thickness = 1)
    {
        if (thickness <= 0) return new List<Vector3Int>();

        Vector3Int center = pos;
        List<Vector3Int> result = new List<Vector3Int>();

        for (int i = 0; i < thickness; i++)
        {
            width++;
            height++;
            foreach (Vector3Int v in fTiles)
            {
                if (v.x == center.x + width && v.y <= center.y + height && v.y >= center.y - height)
                {
                    result.Add(v);
                }

                if (v.x == center.x - width && v.y <= center.y + height && v.y >= center.y - height)
                {
                    result.Add(v);
                }

                if (v.y == center.y + height && v.x <= center.x + width && v.x >= center.x - width)
                {
                    result.Add(v);
                }

                if (v.y == center.y - height && v.x <= center.x + width && v.x >= center.x - width)
                {
                    result.Add(v);
                }
            }
        }

        return result;
    }



    #endregion


    #region Utilities
    private int Vector3IntSort(Vector3Int v1, Vector3Int v2)
    {
        if (v1.x == v2.x && v1.y == v2.y) return 0;

        if (v1.y > v2.y) return 1;
        if (v1.y < v2.y) return -1;

        if (v1.y == v2.y)
        {
            if (v1.x > v2.x) return 1;
            if (v1.x < v2.x) return -1;
        }
        return 0;
    }

    private Vector3 GetCenterOfCollection(List<Vector3Int> collection)
    {
        if (collection.Count < 1) return Vector3.zero;
        Vector3 result = (grid.CellToWorld(collection[collection.Count - 1]) - grid.CellToWorld(collection[0])) / 2;
        return (grid.CellToWorld(collection[0]) + result);
    }

    private Vector3 AbsVector3(Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    public Vector3Int PosToTile(Vector3 pos)
    {
        return grid.WorldToCell(pos);
    }

    public Vector3 TileToPos(Vector3Int tile)
    {
        return grid.CellToWorld(tile) + (new Vector3(grid.cellSize.x, 0, grid.cellSize.y) / 2);
    }

    private List<Vector3Int> GetAllTiles(Tilemap tilemap)
    {
        List<Vector3Int> result = new List<Vector3Int>();

        tilemap.CompressBounds();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(pos) != null) result.Add(pos);
        }

        return result;
    }

    private void DestroyChildren(GameObject parent)
    {
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
        if (allChildren.Length > 0)
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                DestroyImmediate(allChildren[i].gameObject);
            }
        }
    }

    public bool SamplePosition(Vector3 pos)
    {
        return NavMesh.SamplePosition(pos, out NavMeshHit hit, 0.5f, NavMesh.AllAreas);
    }
    #endregion

}
