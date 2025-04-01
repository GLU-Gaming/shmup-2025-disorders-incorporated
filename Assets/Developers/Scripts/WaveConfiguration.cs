using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWaveConfiguration", menuName = "Wave Configuration")]
public class WaveConfiguration : ScriptableObject
{
    public List<EnemyConfiguration> enemyConfigurations;
}
