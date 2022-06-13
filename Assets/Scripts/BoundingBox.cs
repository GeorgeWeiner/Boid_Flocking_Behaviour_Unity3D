using UnityEngine;

namespace DefaultNamespace
{
    public class BoundingBox : MonoBehaviour
    {
        public float scale;
        public readonly Vector3[] _vertices = new Vector3[8];

        private void OnValidate()
        {
            ChangeBounds();
        }

        private void ChangeBounds()
        {
            var position = transform.position;
            var x = position.x;
            var y = position.y;
            var z = position.z;
            
            _vertices[0] = new Vector3(x - scale, y - scale, z - scale);
            _vertices[1] = new Vector3(x - scale, y - scale, z + scale);
            _vertices[2] = new Vector3(x + scale, y - scale, z + scale);
            _vertices[3] = new Vector3(x + scale, y - scale, z - scale);
            _vertices[4] = new Vector3(x - scale, y + scale, z - scale);
            _vertices[5] = new Vector3(x - scale, y + scale, z + scale);
            _vertices[6] = new Vector3(x + scale, y + scale, z + scale);
            _vertices[7] = new Vector3(x + scale, y + scale, z - scale);
        }

        private void OnDrawGizmos()
        {
            foreach (var corner in _vertices)
            {
                Gizmos.DrawSphere(corner, 1f);
            }
            Gizmos.DrawLine(_vertices[0], _vertices[4]);
            Gizmos.DrawLine(_vertices[1], _vertices[5]);
            Gizmos.DrawLine(_vertices[2], _vertices[6]);
            Gizmos.DrawLine(_vertices[3], _vertices[7]);
            
            Gizmos.DrawLine(_vertices[0], _vertices[1]);
            Gizmos.DrawLine(_vertices[1], _vertices[2]);
            Gizmos.DrawLine(_vertices[2], _vertices[3]);
            Gizmos.DrawLine(_vertices[3], _vertices[0]);
            
            Gizmos.DrawLine(_vertices[4], _vertices[5]);
            Gizmos.DrawLine(_vertices[5], _vertices[6]);
            Gizmos.DrawLine(_vertices[6], _vertices[7]);
            Gizmos.DrawLine(_vertices[7], _vertices[4]);
        }
    }
}