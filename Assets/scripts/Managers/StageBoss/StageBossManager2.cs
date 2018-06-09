using System;
using UnityEngine;

namespace Managers.StageBoss
{
    public class StageBossManager2 : StageBossManager
    {
        private readonly GameObject[] _bossList = new GameObject[5];
        private bool _started;

        private void Update()
        {
            if (_started && AreBossesDead())
            {
                Game.GameManager.StageUp();
            }
        }

        public override void StartBossFight()
        {
            Debug.Log("Start Boss Fight 2");
            _started = true;
            for (var i = 0; i < 5; i++)
            {
                float angle = 90 + (5*i);
                const float magnitude = 70;
                var pos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * magnitude, 0,
                    Mathf.Sin(Mathf.Deg2Rad * angle) * magnitude);
                var monster = Instantiate(Resources.Load<GameObject>("Monsters/Dragon"));
                monster.transform.position += pos;
                monster.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                monster.name = "BabyDragon";
                monster.GetComponent<AbstractMonster>().Hp = 5;
                monster.GetComponent<AbstractMonster>().Experience = 0;
                monster.SetActive(true);
                _bossList[i] = monster;            
            }
        }

        private bool AreBossesDead()
        {
            return !Array.Find<GameObject>(_bossList, boss => boss != null);
        }
    }
}