using UnityEngine;
using UnityEngine.Events;

namespace Managers.StageBoss
{
    public class StageBossManager1 : StageBossManager
    {
        private GameObject _boss;
        private bool _started;

        private void Update()
        {
            if (_started && _boss && !_boss.activeSelf)
            {
                Game.GameManager.StageUp();
            }
        }

        public override void StartBossFight()
        {
            Debug.Log("Start Boss Fight 1");
            _started = true;
            const float angle = 90;
            const float magnitude = 70;
            var pos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * magnitude, 0,
                Mathf.Sin(Mathf.Deg2Rad * angle) * magnitude);
            var monster = GetComponent<GameManager>().GetMonsterGenerator().GetMonsterFromName("Spider");
            monster.transform.position += pos;
            monster.transform.localScale = new Vector3(5, 5, 5);
            monster.name = "BigSpider";
            monster.GetComponent<AbstractMonster>().Hp = 500;
            monster.GetComponent<AbstractMonster>().Experience = 0;
            _boss = monster;
        }
    }
}