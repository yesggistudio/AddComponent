using IndieMarc.Platformer;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Development_Tool;

namespace UnityTemplateProjects.Jaeyun.Script.Actor
{
    public class Actor : MonoBehaviour
    {

        public bool isLocked;

        private List<Drag> _drags = new List<Drag>();
        

        private Material _myMat;

        private Shader _defaultShader;
        private Shader _OutlineShader;
        
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");


        public int player_id;

        [Header("Movement")]
        public float move_accel = 20f;
        public float move_deccel = 20f;
        public float move_max = 5f;


        [Header("Jump")]
        public bool can_jump = true;
        public bool double_jump = true;
        //public bool jump_on_enemies = true;
        public float jump_strength = 5f;
        public float jump_time_min = 0.4f;
        public float jump_time_max = 0.8f;
        public float jump_gravity = 5f;
        public float jump_fall_gravity = 10f;
        public float jump_move_percent = 0.75f;
        public LayerMask raycast_mask;
        public float ground_raycast_dist = 0.1f;




        private Rigidbody2D rigid;
        private CapsuleCollider2D capsule_coll;
        private ContactFilter2D contact_filter;
        private Vector2 coll_start_h;
        private Vector2 coll_start_off;

        private bool was_grounded = false;
        private bool is_grounded = false;
        private bool is_fronted = false;
        private bool is_jumping = false;
        private bool is_double_jump = false;

        private Vector2 move;


        private void Awake()
        {
            _myMat = GetComponent<SpriteRenderer>().material;
            _defaultShader = Shader.Find("Custom/2D Sprite");
            _OutlineShader = Shader.Find("Shader Graphs/2D DrawOutline");

            rigid = this.GetComponent<Rigidbody2D>();
            capsule_coll = GetComponent<CapsuleCollider2D>();
            coll_start_h = capsule_coll.size;
            coll_start_off = capsule_coll.offset;


        }

        private void FixedUpdate()
        {
            //움직임.
            ControlManager controls = ControlManager.Get(player_id);

            Vector3 move_input = controls.GetMove();
            float desiredSpeed = Mathf.Abs(move_input.x) > 0.1f ? move_input.x * move_max : 0f;
            float acceleration = Mathf.Abs(move_input.x) > 0.1f ? move_accel : move_deccel;
            acceleration = !is_grounded ? jump_move_percent * acceleration : acceleration;
            move.x = Mathf.MoveTowards(move.x, desiredSpeed, acceleration * Time.fixedDeltaTime);

            was_grounded = is_grounded;
            is_grounded = DetectObstacle(Vector3.down);
            is_fronted = IsFronted();

        }

        private bool DetectObstacle(Vector3 dir)
        {
            bool grounded = false;
            Vector2[] raycastPositions = new Vector2[3];

            Vector2 raycast_start = rigid.position;
            Vector2 orientation = dir.normalized;
            bool is_up_down = Mathf.Abs(orientation.y) > Mathf.Abs(orientation.x);
            Vector2 perp_ori = is_up_down ? Vector2.right : Vector2.up;
            float radius = GetCapsuleRadius();

            if (capsule_coll != null && is_up_down)
            {
                //Adapt raycast to collider
                raycast_start = GetCapsulePos(dir);
            }

            float ray_size = radius + ground_raycast_dist;
            raycastPositions[0] = raycast_start - perp_ori * radius / 2f;
            raycastPositions[1] = raycast_start;
            raycastPositions[2] = raycast_start + perp_ori * radius / 2f;

            for (int i = 0; i < raycastPositions.Length; i++)
            {
                Debug.DrawRay(raycastPositions[i], orientation * ray_size);
                if (RaycastObstacle(raycastPositions[i], orientation * ray_size))
                    grounded = true;
            }
            return grounded;
        }


        private bool IsFronted()
        {
            bool obstacle = DetectObstacle(GetFacing());
            bool box = RaycastObstacle<Box>(GetCapsulePos(GetFacing()), GetFacing());
            bool enemy = RaycastObstacle<Enemy>(GetCapsulePos(GetFacing()), GetFacing());
            return obstacle && !box && !enemy;
        }
        public Vector2 GetFacing()
        {
            return Vector2.right * Mathf.Sign(transform.localScale.x);
        }

        public Vector2 GetSize()
        {
            if (capsule_coll != null)
                return new Vector2(Mathf.Abs(transform.localScale.x) * capsule_coll.size.x, Mathf.Abs(transform.localScale.y) * capsule_coll.size.y);
            return new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        }
        private float GetCapsuleRadius()
        {
            return GetSize().x * 0.5f;
        }
        private Vector3 GetCapsulePos(Vector3 dir)
        {
            Vector2 orientation = dir.normalized;
            Vector2 raycast_offset = capsule_coll.offset + orientation * Mathf.Abs(capsule_coll.size.y * 0.5f - capsule_coll.size.x * 0.5f);
            return rigid.position + raycast_offset * capsule_coll.transform.lossyScale.y;
        }

        public bool RaycastObstacle(Vector2 pos, Vector2 dir)
        {
            RaycastHit2D[] hitBuffer = new RaycastHit2D[5];
            Physics2D.Raycast(pos, dir.normalized, contact_filter, hitBuffer, dir.magnitude);
            for (int j = 0; j < hitBuffer.Length; j++)
            {
                if (hitBuffer[j].collider != null && hitBuffer[j].collider != capsule_coll && !hitBuffer[j].collider.isTrigger)
                {
                    return true;
                }
            }
            return false;
        }

        public GameObject RaycastObstacle<T>(Vector2 pos, Vector2 dir)
        {
            RaycastHit2D[] hitBuffer = new RaycastHit2D[5];
            Physics2D.Raycast(pos, dir.normalized, contact_filter, hitBuffer, dir.magnitude);
            for (int j = 0; j < hitBuffer.Length; j++)
            {
                if (hitBuffer[j].collider != null && hitBuffer[j].collider != capsule_coll && !hitBuffer[j].collider.isTrigger)
                {
                    if (hitBuffer[j].collider.GetComponent<T>() != null)
                        return hitBuffer[j].collider.gameObject;
                }
            }
            return null;
        }



        public void DrawNormal()
        {
            _myMat.shader = _defaultShader;
        }
        
        public void DrawOutline()
        {
            _myMat.shader = _OutlineShader;
            if (isLocked)
            {
                _myMat.SetColor(OutlineColor, Color.magenta);
            }
            else
            {
                _myMat.SetColor(OutlineColor, Color.cyan);
            }
        }

        public void AddDrag(Drag drag)
        {
            _drags.Add(drag);
        }
        
        public void RemoveDrag(Drag drag)
        {
            _drags.Remove(drag);
            ReSortDrags();
        }

        private void ReSortDrags()
        {
            foreach (var drag in _drags)
            {
                drag.SortUpperActorHead();
            }
        }

        public Vector2 GetComponentPos(Drag drag)
        {
            var result = transform.position;

            var index = _drags.FindIndex(x => x == drag);

            result.y += + (index + 1) * .6f;

            return result;
        }

    }
}