using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pose ce composant sur le PARENT d'un objet.
/// Il cherche automatiquement tous les MeshFilter dans les enfants
/// et casse chacun en fragments physiques indépendants.
/// Les faces ouvertes (bords de coupe) sont bouchées.
/// </summary>
public class MeshBreaker : MonoBehaviour
{
    [Header("Fragmentation")]
    [Tooltip("Nombre de morceaux par mesh enfant")]
    public int pieceCount = 8;

    [Header("Explosion")]
    [Tooltip("Force de l'explosion appliquée à chaque fragment")]
    public float explosionForce = 0.1f;

    [Tooltip("Rayon de l'explosion")]
    public float explosionRadius = 1f;

    [Header("Fragments")]
    [Tooltip("Détruire les fragments après N secondes (0 = jamais)")]
    public float fragmentLifetime = 5f;

    [Tooltip("Masse de chaque fragment")]
    public float fragmentMass = 0.1f;

    [Tooltip("PhysicsMaterial des fragments (optionnel)")]
    public PhysicsMaterial fragmentPhysicsMaterial;

    [Header("Matériau de coupe (faces internes)")]
    [Tooltip("Matériau appliqué aux faces de remplissage. Laisse vide pour réutiliser le matériau de l'enfant.")]
    public Material cutMaterial;

    // -------------------------------------------------------------------------
    // Point d'entrée public — appelable depuis n'importe où
    // -------------------------------------------------------------------------

    /// <summary>
    /// Lance la casse en utilisant le 'pieceCount' défini dans l'Inspector.
    /// </summary>
    [ContextMenu("Break")]
    public void Break()
    {
        // On s'assure d'avoir au moins 2 morceaux
        int pieces = Mathf.Max(2, pieceCount);

        // Chercher tous les MeshFilter dans les enfants (exclut le parent s'il n'en a pas)
        MeshFilter[] childFilters = GetComponentsInChildren<MeshFilter>(includeInactive: true);

        if (childFilters.Length == 0)
        {
            Debug.LogWarning($"[MeshBreaker] Aucun MeshFilter trouvé dans les enfants de '{gameObject.name}'.");
            return;
        }

        // Calculer le centre global (bounds de tous les enfants combinés) pour le centre de l'explosion
        Bounds globalBounds = new Bounds(transform.position, Vector3.zero);
        foreach (MeshFilter mf in childFilters)
        {
            if (mf.sharedMesh != null)
            {
                // Convertir les bounds locales en monde
                Bounds worldBounds = TransformBounds(mf.transform, mf.sharedMesh.bounds);
                globalBounds.Encapsulate(worldBounds);
            }
        }
        Vector3 explosionCenter = globalBounds.center;

        // Casser chaque enfant trouvé
        foreach (MeshFilter mf in childFilters)
        {
            if (mf == null || mf.sharedMesh == null) continue;

            MeshRenderer mr = mf.GetComponent<MeshRenderer>();
            if (mr == null) continue;

            BreakSingleMesh(mf, mr, pieces, explosionCenter);
        }

        // Désactiver le parent (et donc masquer tous les enfants d'origine)
        gameObject.SetActive(false);
    }

    // -------------------------------------------------------------------------
    // Casse un seul MeshFilter enfant
    // -------------------------------------------------------------------------

    private void BreakSingleMesh(MeshFilter mf, MeshRenderer mr, int pieces, Vector3 explosionCenter)
    {
        Mesh originalMesh   = mf.sharedMesh;
        Material[] mats     = mr.sharedMaterials;
        Transform  srcTrans = mf.transform;

        // Bounds locales → points Voronoi dans l'espace local du mesh
        Bounds localBounds = originalMesh.bounds;
        Vector3[] voronoiPoints = GenerateVoronoiPoints(pieces, localBounds);

        // Répartir les triangles
        List<List<int>> cells = AssignTrianglesToCells(originalMesh, voronoiPoints);

        // Matériau de coupe
        Material capMat = cutMaterial != null
            ? cutMaterial
            : (mats != null && mats.Length > 0 ? mats[0] : new Material(Shader.Find("Standard")));

        string baseName = mf.gameObject.name;

        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].Count == 0) continue;

            GameObject fragment = BuildFragment(originalMesh, cells[i], mats, capMat, baseName, i);
            if (fragment == null) continue;

            // Positionner le fragment dans le monde exactement comme le mesh source enfant
            fragment.transform.SetPositionAndRotation(srcTrans.position, srcTrans.rotation);
            fragment.transform.localScale = srcTrans.lossyScale;

            // Rigidbody + explosion
            Rigidbody rb = fragment.AddComponent<Rigidbody>();
            rb.mass = fragmentMass;
            rb.AddExplosionForce(
                explosionForce,
                explosionCenter,
                explosionRadius,
                0.5f,
                ForceMode.Impulse
            );

            if (fragmentLifetime > 0f)
                Destroy(fragment, fragmentLifetime);
        }
    }

    // -------------------------------------------------------------------------
    // Points Voronoi (espace local du mesh)
    // -------------------------------------------------------------------------

    private static Vector3[] GenerateVoronoiPoints(int count, Bounds bounds)
    {
        Vector3[] points = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            points[i] = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
        return points;
    }

    // -------------------------------------------------------------------------
    // Répartition des triangles par cellule Voronoi
    // -------------------------------------------------------------------------

    private static List<List<int>> AssignTrianglesToCells(Mesh mesh, Vector3[] voronoiPoints)
    {
        int cellCount = voronoiPoints.Length;
        var cells = new List<List<int>>(cellCount);
        for (int i = 0; i < cellCount; i++) cells.Add(new List<int>());

        Vector3[] vertices  = mesh.vertices;
        int[]     triangles = mesh.triangles;

        for (int t = 0; t < triangles.Length; t += 3)
        {
            Vector3 centroid = (vertices[triangles[t]] +
                                vertices[triangles[t + 1]] +
                                vertices[triangles[t + 2]]) / 3f;

            int   closest = 0;
            float minDist = float.MaxValue;
            for (int p = 0; p < voronoiPoints.Length; p++)
            {
                float d = (centroid - voronoiPoints[p]).sqrMagnitude;
                if (d < minDist) { minDist = d; closest = p; }
            }

            cells[closest].Add(triangles[t]);
            cells[closest].Add(triangles[t + 1]);
            cells[closest].Add(triangles[t + 2]);
        }

        return cells;
    }

    // -------------------------------------------------------------------------
    // Construction d'un fragment avec cap filling
    // -------------------------------------------------------------------------

    private GameObject BuildFragment(Mesh originalMesh, List<int> triangleIndices,
                                     Material[] materials, Material capMat,
                                     string baseName, int fragmentIndex)
    {
        Vector3[] origVerts   = originalMesh.vertices;
        Vector3[] origNormals = originalMesh.normals;
        Vector2[] origUVs     = originalMesh.uv;

        bool hasNormals = origNormals != null && origNormals.Length > 0;
        bool hasUVs     = origUVs     != null && origUVs.Length     > 0;

        // --- Sub-mesh 0 : triangles originaux ---
        var indexMap = new Dictionary<int, int>();
        var verts    = new List<Vector3>();
        var normals  = new List<Vector3>();
        var uvs      = new List<Vector2>();
        var tris     = new List<int>();

        foreach (int oldIdx in triangleIndices)
        {
            if (!indexMap.ContainsKey(oldIdx))
            {
                indexMap[oldIdx] = verts.Count;
                verts.Add(origVerts[oldIdx]);
                if (hasNormals && oldIdx < origNormals.Length) normals.Add(origNormals[oldIdx]);
                if (hasUVs     && oldIdx < origUVs.Length)      uvs.Add(origUVs[oldIdx]);
            }
            tris.Add(indexMap[oldIdx]);
        }

        if (verts.Count == 0) return null;

        // --- Détecter les arêtes de bord ---
        var edgeCount = new Dictionary<long, int>();
        var edgeVerts = new Dictionary<long, (int a, int b)>();

        for (int t = 0; t < triangleIndices.Count; t += 3)
        {
            RegisterEdge(triangleIndices[t],     triangleIndices[t + 1], edgeCount, edgeVerts);
            RegisterEdge(triangleIndices[t + 1], triangleIndices[t + 2], edgeCount, edgeVerts);
            RegisterEdge(triangleIndices[t + 2], triangleIndices[t],     edgeCount, edgeVerts);
        }

        var borderEdges = new List<(int a, int b)>();
        foreach (var kv in edgeCount)
            if (kv.Value == 1) borderEdges.Add(edgeVerts[kv.Key]);

        // --- Edge loops → caps ---
        List<List<int>> loops = BuildEdgeLoops(borderEdges);
        var capTris = new List<int>();

        foreach (List<int> loop in loops)
        {
            if (loop.Count < 3) continue;

            Vector3 centroid = Vector3.zero;
            foreach (int idx in loop) centroid += origVerts[idx];
            centroid /= loop.Count;

            Vector3 loopNormal = ComputeLoopNormal(loop, origVerts, centroid);

            int pivotIndex = verts.Count;
            verts.Add(centroid);
            normals.Add(loopNormal);
            uvs.Add(new Vector2(0.5f, 0.5f));

            var loopNewIndices = new List<int>();
            foreach (int origIdx in loop)
            {
                loopNewIndices.Add(verts.Count);
                verts.Add(origVerts[origIdx]);
                normals.Add(loopNormal);
                Vector3 localPos = origVerts[origIdx] - centroid;
                uvs.Add(new Vector2(localPos.x * 0.5f + 0.5f, localPos.z * 0.5f + 0.5f));
            }

            for (int k = 0; k < loopNewIndices.Count; k++)
            {
                int next   = (k + 1) % loopNewIndices.Count;
                Vector3 e1 = verts[loopNewIndices[k]]    - centroid;
                Vector3 e2 = verts[loopNewIndices[next]] - centroid;

                if (Vector3.Dot(Vector3.Cross(e1, e2), loopNormal) > 0)
                {
                    capTris.Add(pivotIndex);
                    capTris.Add(loopNewIndices[k]);
                    capTris.Add(loopNewIndices[next]);
                }
                else
                {
                    capTris.Add(pivotIndex);
                    capTris.Add(loopNewIndices[next]);
                    capTris.Add(loopNewIndices[k]);
                }
            }
        }

        // --- Mesh final ---
        var mesh = new Mesh();
        mesh.name        = $"{baseName}_Fragment_{fragmentIndex}";
        mesh.subMeshCount = 2;
        mesh.vertices    = verts.ToArray();

        bool normalsFull = normals.Count == verts.Count;
        bool uvsFull     = uvs.Count     == verts.Count;
        if (normalsFull) mesh.normals = normals.ToArray();
        if (uvsFull)     mesh.uv      = uvs.ToArray();

        mesh.SetTriangles(tris,    0);
        mesh.SetTriangles(capTris, 1);
        mesh.RecalculateBounds();
        if (!normalsFull) mesh.RecalculateNormals();

        // --- GameObject ---
        var go = new GameObject($"{baseName}_Fragment_{fragmentIndex}");

        go.AddComponent<MeshFilter>().mesh = mesh;

        var newMr = go.AddComponent<MeshRenderer>();
        Material surfaceMat = materials != null && materials.Length > 0
            ? materials[0]
            : new Material(Shader.Find("Standard"));
        newMr.materials = new[] { surfaceMat, capMat };

        var mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;
        mc.convex     = true;
        if (fragmentPhysicsMaterial != null) mc.material = fragmentPhysicsMaterial;

        return go;
    }

    // -------------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------

    private static long EdgeKey(int a, int b)
    {
        int lo = Mathf.Min(a, b), hi = Mathf.Max(a, b);
        return ((long)lo << 32) | (uint)hi;
    }

    private static void RegisterEdge(int a, int b,
        Dictionary<long, int> count, Dictionary<long, (int, int)> verts)
    {
        long key = EdgeKey(a, b);
        if (count.ContainsKey(key)) count[key]++;
        else { count[key] = 1; verts[key] = (a, b); }
    }

    private static List<List<int>> BuildEdgeLoops(List<(int a, int b)> edges)
    {
        var adj = new Dictionary<int, List<int>>();
        foreach (var (a, b) in edges)
        {
            if (!adj.ContainsKey(a)) adj[a] = new List<int>();
            if (!adj.ContainsKey(b)) adj[b] = new List<int>();
            adj[a].Add(b);
            adj[b].Add(a);
        }

        var visited = new HashSet<int>();
        var loops   = new List<List<int>>();

        foreach (int start in adj.Keys)
        {
            if (visited.Contains(start)) continue;

            var loop     = new List<int>();
            int current  = start;
            int previous = -1;

            while (true)
            {
                visited.Add(current);
                loop.Add(current);

                int next = -1;
                foreach (int nb in adj[current])
                {
                    if (nb == previous) continue;
                    if (!visited.Contains(nb))          { next = nb; break; }
                    if (nb == start && loop.Count > 2)  { next = -2; break; }
                }

                if (next < 0) break;
                previous = current;
                current  = next;
            }

            if (loop.Count >= 3) loops.Add(loop);
        }

        return loops;
    }

    private static Vector3 ComputeLoopNormal(List<int> loop, Vector3[] verts, Vector3 centroid)
    {
        Vector3 normal = Vector3.zero;
        for (int i = 0; i < loop.Count; i++)
        {
            Vector3 cur  = verts[loop[i]]                      - centroid;
            Vector3 next = verts[loop[(i + 1) % loop.Count]]   - centroid;
            normal += Vector3.Cross(cur, next);
        }
        return normal == Vector3.zero ? Vector3.up : normal.normalized;
    }

    private static Bounds TransformBounds(Transform t, Bounds localBounds)
    {
        Vector3 center = t.TransformPoint(localBounds.center);
        var result = new Bounds(center, Vector3.zero);

        Vector3 ext = localBounds.extents;
        result.Encapsulate(t.TransformPoint(localBounds.center + new Vector3( ext.x,  ext.y,  ext.z)));
        result.Encapsulate(t.TransformPoint(localBounds.center + new Vector3(-ext.x,  ext.y,  ext.z)));
        result.Encapsulate(t.TransformPoint(localBounds.center + new Vector3( ext.x, -ext.y,  ext.z)));
        result.Encapsulate(t.TransformPoint(localBounds.center + new Vector3( ext.x,  ext.y, -ext.z)));
        result.Encapsulate(t.TransformPoint(localBounds.center + new Vector3(-ext.x, -ext.y,  ext.z)));
        result.Encapsulate(t.TransformPoint(localBounds.center + new Vector3( ext.x, -ext.y, -ext.z)));
        result.Encapsulate(t.TransformPoint(localBounds.center + new Vector3(-ext.x,  ext.y, -ext.z)));
        result.Encapsulate(t.TransformPoint(localBounds.center + new Vector3(-ext.x, -ext.y, -ext.z)));

        return result;
    }
}