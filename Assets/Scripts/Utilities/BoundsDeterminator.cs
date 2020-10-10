using UnityEngine;

public static class BoundsDeterminator
{
    public static Bounds Determine(Transform prefab)
    {
        Bounds bounds = new Bounds();
        MeshFilter[] meshes = prefab.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter mesh in meshes)
        {
            Bounds meshBounds = mesh.sharedMesh.bounds;
            Transform meshTransform = mesh.transform;

            meshBounds.center += meshTransform.localPosition;
            meshBounds.size = Vector3.Scale(meshBounds.size, meshTransform.localScale);

            bounds.Encapsulate(meshBounds);
        }

        return bounds;
    }
}