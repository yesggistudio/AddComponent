using UnityEngine;
using UnityEngine.PlayerLoop;

namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
{
    [CreateAssetMenu(fileName = "Type C", menuName = "TempComponent/Type C", order = 0)]
    public class ComponentTypeC : ComponentType
    {
        //Move

        public int player_id;


        [Header("Movement")]
        public float move_accel = 20f;
        public float move_deccel = 20f;
        public float move_max = 5f;


        private Rigidbody2D rigid;
        private BoxCollider2D box_coll;
        private Vector2 coll_start_h;
        private Vector2 coll_start_off;






    }
}