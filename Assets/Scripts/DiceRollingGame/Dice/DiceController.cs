namespace DiceRollingGame.Dice
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class DiceController : MonoBehaviour, IDice
    {
        [SerializeField] private DiceNumbersSpreader diceNumbersSpreader;
        [SerializeField] private DiceNumerologySettings diceNumerologySettings;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new Collider collider;
        [SerializeField] private float moveDetectionFreq = 0.5f;
        [SerializeField] private float movementDetectionOffset = 0.1f;

        public event Action<IDice, int> OnFinishedMoving;

        private IEnumerator waitForRest;
        
        private readonly Dictionary<int, DiceSide> diceFaces = new();

        public bool IsSelectable { get; set; } = true;

        public Transform Transform => transform;
        public Rigidbody Rigidbody => rigidbody;
        public Collider Collider => collider;

        public void Initialize()
        {
            var sides = diceNumbersSpreader.GetSpreadNumbers();
            NumberSidesOfDice(sides);
        }
        
        private void Update()
        {
            GetCurrentScore();
        }

        public void StartWaitingForRest()
        {
            if(waitForRest != null)
                StopCoroutine(waitForRest);
            
            waitForRest = WaitForRest();
            StartCoroutine(waitForRest);
        }

        private IEnumerator WaitForRest()
        {
            while (IsMoving())
            {
                yield return new WaitForSeconds(moveDetectionFreq);
            }

            IsSelectable = true;
            OnFinishedMoving?.Invoke(this, GetCurrentScore());
        }

        private bool IsMoving()
        {
            var movementMagnitude = rigidbody.linearVelocity.magnitude + rigidbody.angularVelocity.magnitude;
            
            return !(movementMagnitude < movementDetectionOffset);
        }
        
        private int GetCurrentScore()
        {
            var closestDot = -1f;
            var closestSide = -1;

            var localVectorToMatch = transform.InverseTransformDirection(Vector3.up);
            
            foreach (var diceFace in diceFaces)
            {
                var dot = Vector3.Dot(diceFace.Value.Normal, localVectorToMatch);
                
                if (dot > closestDot)
                {
                    closestDot = dot;
                    closestSide = diceFace.Key;
                }
            }

            var x = transform.position.x;
            
            return closestSide;
        }
        
        private void NumberSidesOfDice(DiceSide[] sides)
        {
            for (var i = 0; i < sides.Length; i++)
            {
                sides[i].TextMesh.text = diceNumerologySettings.DiceNumbers[i].ToString();

                if (diceNumerologySettings.DiceNumbers[i] is 6 or 9)
                    sides[i].TextMesh.fontStyle = FontStyles.Underline;
                
                diceFaces.Add(diceNumerologySettings.DiceNumbers[i], sides[i]);
            }
        }
    }
}