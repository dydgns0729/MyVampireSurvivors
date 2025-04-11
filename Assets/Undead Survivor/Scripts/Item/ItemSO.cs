using UnityEngine;

namespace MyVampireSurvivors
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
    public class ItemSO : ScriptableObject
    {
        public new string name;
        public bool stackable;
        public Sprite icon;
        [TextArea]
        public string description;

        public Transform prefab;
    }
}