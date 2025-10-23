namespace DiceRollingGame.Dice
{
    using System.Linq;
    using TMPro;
    using UnityEngine;

    public struct DiceSide 
    {
        public Vector3 Normal;
        public TextMeshPro TextMesh;
    }
    
    public class DiceNumbersSpreader : MonoBehaviour
    {
        [SerializeField] private TextMeshPro numberPrefab;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private Transform numbersContainer;
        [SerializeField] private float numberOffset = 0.01f;
        [SerializeField] private int numberOfFaceEdges = 5;

        private Vector3[] faceVertices;
        private DiceSide[] diceSides;

        public DiceSide[] GetSpreadNumbers()
        {
            if(numberOfFaceEdges < 3)
                Debug.LogError($"A dice should never have less than 3 sides!");
            
            faceVertices = new Vector3[numberOfFaceEdges];

            var mesh = meshFilter.mesh;
            var vertices = mesh.vertices;
            diceSides = new DiceSide[vertices.Length/numberOfFaceEdges];

            var faceNumber = 0;
            
            for (var i = 0; i < vertices.Length; i += numberOfFaceEdges)
            {
                for (var j = 0; j < numberOfFaceEdges; j++)
                {
                    faceVertices[j] = vertices[i + j];
                }
                
                var middle = CalculateMiddlePoint();
                InstantiateNumberObject(middle, faceNumber);

                var vertexA = vertices[i];
                var vertexB = vertices[i + 1];
                var vertexC = vertices[i + 2];

                diceSides[faceNumber].Normal = Vector3.Cross(vertexB - vertexA, vertexC - vertexA).normalized;
                faceNumber++;
            }

            return diceSides;
        }

        private Vector3 CalculateMiddlePoint()
        {
            var sum = faceVertices.Aggregate(Vector3.zero, (current, pos) => current + pos);
            return sum / faceVertices.Length;
        }

        private void InstantiateNumberObject(Vector3 position, int faceNumber)
        {
            var numberMesh = Instantiate(numberPrefab, numbersContainer);
            diceSides[faceNumber].TextMesh = numberMesh;
            
            var normal = Vector3.Cross(faceVertices[1] - faceVertices[0], faceVertices[2] - faceVertices[0]).normalized;
            var numberTransform = numberMesh.transform;
            
            numberTransform.rotation = transform.rotation * Quaternion.LookRotation(normal, Vector3.forward) * Quaternion.Euler(0, 180, 0);
            numberTransform.position = transform.position + position - numberTransform.forward * numberOffset;
        }
    }
}