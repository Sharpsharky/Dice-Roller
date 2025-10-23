namespace DiceRollingGame.Roller
{
    using System;
    using System.Collections;
    using DG.Tweening;
    using Dice;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public readonly struct Constraints
    {
        public readonly float min;
        public readonly float max;

        public Constraints(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
    
    public class DiceRollingController : MonoBehaviour, IRoller
    {
        [SerializeField] private DiceRollingSettings diceRollingSettings;
        [SerializeField] private DiceRandomRollingSettings diceRandomRollingSettings;
        [SerializeField] private Collider desktopCollider;
        [SerializeField] private Vector3 restingRot;

        public event Action<IDice> OnDiceTossed;
        
        private bool isBeingDragged;
        private IDice currentDice;
        private Vector3 currentPos;
        private new Camera camera;
        private Constraints xDesktopConstraints;
        private Constraints zDesktopConstraints;
        private Tween tween;
        private IEnumerator moveDiceToHand;

        private const float AngleOffset = 90;

        public void Initialize(Camera camera)
        {
            this.camera = camera;
            
            CalculateDesktopConstraints();
        }
        
        public void Grab(IDice dices)
        {
            PutToHand(dices);
        }

        public void ThrowCurrentTargetable()
        {
            var velocity = (GetTargetPosOfDice() - currentDice.Transform.position) * diceRollingSettings.DiceVelocityCoefficient;

            if (!isBeingDragged)
            {
                PutDiceToInitialPos();
                return;
            }

            isBeingDragged = false;

            if (velocity.magnitude > diceRollingSettings.MaxDiceVelocity)
            {
                velocity = velocity.normalized * diceRollingSettings.MaxDiceVelocity;
            }
            else if (velocity.magnitude < diceRollingSettings.MinSpeedToToss)
            {
                currentDice.Rigidbody.linearVelocity = Vector3.zero;
                currentDice.Rigidbody.angularVelocity = Vector3.zero;
                PutDiceToInitialPos();
                return;
            }
        
            currentDice.Rigidbody.linearVelocity = velocity;
            OnDiceTossed?.Invoke(currentDice);

            ResetDice();
        }

        public void RandomRoll(IDice dice)
        {
            if (!dice.IsSelectable)
                return;
            
            currentDice = dice;
            ThrowDieRandomly();
            currentDice.IsSelectable = false;
            OnDiceTossed?.Invoke(currentDice);
        }

        private void ThrowDieRandomly()
        {
            var randomDir = diceRandomRollingSettings.RandomDirectionOffset;
            var xRandomVel = Random.Range(-randomDir, randomDir);
            var yRandomVel = Random.Range(-randomDir, randomDir);
            
            var randomVelMagnitude = Random.Range(diceRandomRollingSettings.MinRandomVelocity, diceRandomRollingSettings.MaxRandomVelocity);

            var randomAngle = diceRandomRollingSettings.RandomAngleOffset;
            var xRandomRot = Random.Range(-randomAngle, randomAngle);
            var yRandomRot = Random.Range(-randomAngle, randomAngle);
            var zRandomRot = Random.Range(-randomAngle, randomAngle);
            
            var randomRotMagnitude = Random.Range(diceRandomRollingSettings.MinRandomRotation, diceRandomRollingSettings.MaxRandomRotation);
            
            currentDice.Rigidbody.linearVelocity = new Vector3(xRandomVel,1,yRandomVel).normalized * randomVelMagnitude;
            currentDice.Rigidbody.angularVelocity = new Vector3(xRandomRot,yRandomRot,zRandomRot).normalized * randomRotMagnitude;
        }
        
        private void Update()
        {
            TryFollowCursor();
        }

        private void PutToHand(IDice dices)
        {
            isBeingDragged = false;
            currentDice = dices;
            currentDice.IsSelectable = false;
            
            currentDice.Rigidbody.useGravity = false;
            currentDice.Rigidbody.angularVelocity = Vector3.zero;
            var targetPos = GetTargetPosOfDice();
            
            tween?.Kill();

            tween = currentDice.Transform.DOMove(targetPos, diceRollingSettings.FlyToHandTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (currentDice != null)
                {
                    currentPos = GetTargetPosOfDice();
                    isBeingDragged = true;
                }
            });
        }

        private void TryFollowCursor()
        {
            if (!isBeingDragged)
                return;
                        
            var newTargetPos = GetTargetPosOfDice();
            var direction = newTargetPos - currentPos;
            var rotatedVector = Quaternion.AngleAxis(AngleOffset, Vector3.up) * direction.normalized;
            var finalRotation = rotatedVector * Mathf.Pow(direction.magnitude * diceRollingSettings.VelocityCoefficient, diceRollingSettings.VelocityExponent);

            currentDice.Rigidbody.angularVelocity += finalRotation;
            
            currentPos = newTargetPos;
            currentDice.Rigidbody.MovePosition(currentPos);
        }
        
        private Vector3 GetTargetPosOfDice()
        {
            var plane = new Plane(Vector3.up, Vector3.up * diceRollingSettings.RollingHeight);
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (!plane.Raycast(ray, out var distance)) 
                return Vector3.zero;
            
            var rayHitPos = ray.GetPoint(distance);

            var diceExtents = currentDice.Collider.bounds.extents;
            rayHitPos.x = Mathf.Clamp(rayHitPos.x, xDesktopConstraints.min + diceExtents.x, xDesktopConstraints.max - diceExtents.x);
            rayHitPos.z = Mathf.Clamp(rayHitPos.z, zDesktopConstraints.min + diceExtents.z, zDesktopConstraints.max - diceExtents.z);
            
            return rayHitPos;
        }

        private void CalculateDesktopConstraints()
        {
            var desktopPos = desktopCollider.transform.position;
            var desktopExtents = desktopCollider.bounds.extents;
            
            xDesktopConstraints = new Constraints(-desktopExtents.x + desktopPos.x, desktopExtents.x + desktopPos.x);
            zDesktopConstraints = new Constraints(-desktopExtents.z + desktopPos.z, desktopExtents.z + desktopPos.z);
        }

        private void PutDiceToInitialPos()
        {
            tween?.Kill();
            if(moveDiceToHand != null)
                StopCoroutine(moveDiceToHand);

            var yDesktopSurface = desktopCollider.bounds.extents.y + desktopCollider.transform.position.y;
            var targetPos = new Vector3(currentDice.Transform.position.x, yDesktopSurface + currentDice.Collider.bounds.extents.y, currentDice.Transform.position.z);
            
            var posTween = currentDice.Transform.DOMove(targetPos, diceRollingSettings.FlyToHandTime)
                .SetEase(Ease.Linear);
            
            var rotTween = currentDice.Transform.DORotate(restingRot, diceRollingSettings.FlyToHandTime)
                .SetEase(Ease.Linear);

            tween = DOTween.Sequence().Join(posTween).Join(rotTween).OnComplete(() =>
            {
                currentDice.IsSelectable = true;
                ResetDice();
            });
        }

        private void ResetDice()
        {
            currentDice.Rigidbody.useGravity = true;
            currentDice = null;
        }
    }
}