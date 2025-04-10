using UnityEngine;

namespace MyVampireSurvivors
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
    public class ItemSO : ScriptableObject
    {
        public string name;
        public Sprite icon;
        public string description;
    }
}