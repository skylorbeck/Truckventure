using UnityEngine;

[CreateAssetMenu(fileName = "Biome", menuName = "Biome", order = 0)]
public class Biome: ScriptableObject
{
    public GameObject[] obstacles;
    public GameObject[] buildings;
}