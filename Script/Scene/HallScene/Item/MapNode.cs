using UnityEngine;

namespace Script.Scene.Hall.Item
{
    public class MapNode : MonoBehaviour
    {
        public GameObject Icon;

        public Vector3 GetIconPosition()
        {
            return Icon.transform.position;
        }
    }
}