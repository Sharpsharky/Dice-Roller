namespace DiceRollingGame
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    public class GameManager : SerializedMonoBehaviour
    {
        [OdinSerialize] private List<IManager> managers;
        
        private void Awake()
        {
            InitializeManagers();
        }

        private void InitializeManagers()
        {
            foreach (var manager in managers)
            {
                manager.Initialize();
            }
        }
    }
}