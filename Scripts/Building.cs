using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buildings/Building")]
public class Building : ScriptableObject
{
    public string Name;
    public GameObject prefab;

    public int width;
    public int height;

    public Dictionary<Resource, int> resourceToBuild = new Dictionary<Resource, int>();
}
