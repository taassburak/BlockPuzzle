using UnityEngine;

public static class MeshTools
{
	public static void CalculateNormalsManaged(MeshFilter meshFilter)
	{
		var mesh = meshFilter.sharedMesh;
		var verts = mesh.vertices;
		var normals = mesh.normals;
		var tris = mesh.triangles;

		for (int i = 0; i < tris.Length; i += 3)
		{
			int tri0 = tris[i];
			int tri1 = tris[i + 1];
			int tri2 = tris[i + 2];
			Vector3 vert0 = verts[tri0];
			Vector3 vert1 = verts[tri1];
			Vector3 vert2 = verts[tri2];
            // Vector3 normal = Vector3.Cross(vert1 - vert0, vert2 - vert0);
			Vector3 normal = new Vector3()
			{
				x = vert0.y * vert1.z - vert0.y * vert2.z - vert1.y * vert0.z + vert1.y * vert2.z + vert2.y * vert0.z - vert2.y * vert1.z,
				y = -vert0.x * vert1.z + vert0.x * vert2.z + vert1.x * vert0.z - vert1.x * vert2.z - vert2.x * vert0.z + vert2.x * vert1.z,
				z = vert0.x * vert1.y - vert0.x * vert2.y - vert1.x * vert0.y + vert1.x * vert2.y + vert2.x * vert0.y - vert2.x * vert1.y
			};
			normals[tri0] += normal;
			normals[tri1] += normal;
			normals[tri2] += normal;
		}

		for (int i = 0; i < normals.Length; i++)
		{
            // normals [i] = Vector3.Normalize (normals [i]);
			Vector3 norm = normals[i];
			float invlength = 1.0f / (float) System.Math.Sqrt(norm.x * norm.x + norm.y * norm.y + norm.z * norm.z);
			normals[i].x = norm.x * invlength;
			normals[i].y = norm.y * invlength;
			normals[i].z = norm.z * invlength;
		}

		mesh.normals = normals;
	}
}