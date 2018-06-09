using System;
using UnityEngine;

namespace Managers.StageBoss
{
    public class StageBossManager3 : StageBossManager
    {
        private readonly GameObject[] _bossList = new GameObject[10];
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
            Debug.Log("Start Boss Fight 3");
            _started = true;
            for (var i = 0; i < 10; i++)
            {
                float angle = 90 + (5*i);
                const float magnitude = 70;
                var pos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * magnitude, 0,
                    Mathf.Sin(Mathf.Deg2Rad * angle) * magnitude);
                var monster = Instantiate(Resources.Load<GameObject>("Monsters/Bearbot"));
                monster.transform.position += pos;
                monster.transform.localScale = new Vector3(2f, 2f, 2f);
                monster.name = "ElderBearbot";
                monster.GetComponent<AbstractMonster>().Hp = 2;
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