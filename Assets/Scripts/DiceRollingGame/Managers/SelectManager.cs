namespace DiceRollingGame.Managers
{
    using Input;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;
    using UnityEngine;

    public class SelectManager : SerializedMonoBehaviour, IManager
    {
        [SerializeField] private InputActions input;
        [SerializeField] private Camera camera;
        [OdinSerialize] private IRoller roller;

        private IDice currentlyIndicatedDice;
        private IDice currentlySelectedGameObject;

        private const string InteractiveLayer = "InteractiveLayer";

        public void Initialize()
        {
            input = new InputActions();
            input.Enable();
            roller.Initialize(camera);
        }
        
        private void Update()
        {
            FindTargetables();

            if (input.Gameplay.GrabDice.WasPerformedThisFrame())
            {
                TryClickTargetable();
            }
            else if (input.Gameplay.GrabDice.WasReleasedThisFrame())
            {
                TryReleaseTargetable();
            }
        }
        
        private void FindTargetables()
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 999f, LayerMask.GetMask(InteractiveLayer)))
            {
                if (hit.transform.gameObject.TryGetComponent<IDice>(out var targetable))
                {
                    if (currentlyIndicatedDice != targetable)
                    {
                        IndicateTarget(targetable);
                    }
                }
                else
                {
                    currentlyIndicatedDice = null;
                }
            }
            else
            {
                currentlyIndicatedDice = null;
            }
        }

        private void TryClickTargetable()
        {
            if (currentlyIndicatedDice == null || !currentlyIndicatedDice.IsSelectable) 
                return;
            
            currentlySelectedGameObject = currentlyIndicatedDice;
            roller.Grab(currentlyIndicatedDice);
        }
        
        private void TryReleaseTargetable()
        {
            if (currentlySelectedGameObject == null) 
                return;
            
            currentlySelectedGameObject = null;
            roller.ThrowCurrentTargetable();
        }
        
        private void IndicateTarget(IDice dice)
        {
            currentlyIndicatedDice = dice;
        }
    }
}