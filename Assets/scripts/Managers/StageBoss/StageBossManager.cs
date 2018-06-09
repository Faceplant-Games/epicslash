using UnityEngine;

namespace Managers.StageBoss
{
    public abstract class StageBossManager : MonoBehaviour
    {
        private void Start()
        {
            Game.StageBossManager = this;
        }

        public abstract void StartBossFight();
    }
}