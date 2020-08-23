using IndieMarc.Platformer;
using System.Collections.Generic;
using Jaeyun.Script.GameEvent_System;
using UnityEngine;
using UnityEngine.Events;
using UnityTemplateProjects.Jaeyun.Script.Development_Tool;
using UnityTemplateProjects.Jaeyun.Script.Level;
using System.Collections;
using DG.Tweening.Plugins.Options;
using TMPro;
using System.Data.Common;

namespace UnityTemplateProjects.Jaeyun.Script.Actor
{
    public class Actor : MonoBehaviour,IListener
    {

        public bool isLocked;
        public bool isRealTime;

        private List<Drag> _drags = new List<Drag>();
        [SerializeField]
        public List<IndivisibleComponent> indivisibleComponents;

        public float buttonHeight = 1;

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
        public float jump_strength = 5f;
        public float jump_time_min = 0.4f;
        public float jump_time_max = 0.8f;
        public float jump_gravity = 5f;
        public float jump_fall_gravity = 10f;
        public float jump_move_percent = 0.75f;
        public LayerMask raycast_mask;
        public float ground_raycast_dist = 0.1f;



        [Header("Climb")]
        public float climb_speed = 2f;

        public UnityAction onDeath;
        public UnityAction onHit;
        public UnityAction onJump;
        public UnityAction onLand;
        public UnityAction onClimb;

        private Rigidbody2D rigid;
        private CapsuleCollider2D capsule_coll;
        private ContactFilter2D contact_filter;
        private Vector2 coll_start_h;
        private Vector2 coll_start_off;
        private Vector3 start_scale;
        private Vector3 last_ground_pos;


        private PlayerCharacterState state;
        private Vector2 move;

        private bool was_grounded = false;
        private bool is_grounded = false;
        private bool is_fronted = false;
        private bool is_jumping = false;
        private bool is_double_jump = false;
        private Vector3 grab_pos;

        private float state_timer = 0f;
        private float jump_timer = 0f;
        private float hit_timer = 0f;

        private static List<Actor> character_list = new List<Actor>();

        private SpriteRenderer _spriteRenderer;

        public bool[] _canComAbility = new bool[4];

        public ParticleSystem destroyParticle;
        public GameObject DmgObj;
        public GameEvent gameEvent;

        void Awake()
        {
            _myMat = GetComponent<SpriteRenderer>().material;
            _defaultShader = Shader.Find("Custom/2D Sprite");
            _OutlineShader = Shader.Find("Shader Graphs/2D DrawOutline");
            _spriteRenderer = GetComponent<SpriteRenderer>();

            character_list.Add(this);
            rigid = GetComponent<Rigidbody2D>();
            capsule_coll = GetComponent<CapsuleCollider2D>();
            coll_start_h = capsule_coll.size;
            coll_start_off = capsule_coll.offset;
            start_scale = transform.localScale;
            //average_ground_pos = transform.position;
            last_ground_pos = transform.position;

            contact_filter = new ContactFilter2D();
            contact_filter.layerMask = raycast_mask;
            contact_filter.useLayerMask = true;
            contact_filter.useTriggers = false;
        }


        private void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                _canComAbility[i] = false;
            }
            EventManager.Instance.AddListener(EVENT_TYPE.GameOver, this);
        }

        public void OnEvent(EVENT_TYPE eventType, Component sender, Object param = null)
        {
            switch (eventType)
            {

                case EVENT_TYPE.GameOver:

                    // 마지막 대사 이벤트 실행
                    _canComAbility[3] = true;

                    break;
                default:
                    break;

            }
        }

        public void GameStartSetting()
        {

            for (int i = 0; i < 4; i++)
            {
                _canComAbility[i] = false;
            }

            foreach (var drag in _drags)
            {
                var componentType = drag.GetComponentType();
                CheckComponent(componentType);
            }
            
            foreach (var indivisibleComponent in indivisibleComponents)
            {
                var componentType = indivisibleComponent.GetComponentType();
                CheckComponent(componentType);
            }
        }

        private void CheckComponent(ComponentType componentType)
        {
            var typeOfComponent = componentType.GetType();

            if (typeOfComponent == typeof(MoveComponent))
            {
                _canComAbility[0] = true;
                return;
            }
            if (typeOfComponent == typeof(JumpComponent))
            {
                _canComAbility[1] = true;
                return;
            }
            if (typeOfComponent == typeof(DestroyComponent))
            {
                _canComAbility[2] = true;

                StartCoroutine(DestoryCoroutine());

                return;
            }
            
            if (typeOfComponent == typeof(GameOverComponent))
            {
                var levelManager = FindObjectOfType<LevelManager>();
                levelManager.GameOver();
                return;
            }
        }


        void OnDestroy()
        {
            character_list.Remove(this);
        }


        //Handle physics
        void FixedUpdate()
        {
            
            PlayerControls controls = PlayerControls.Get(player_id);

            UpdateMove();
            //Movement velocity
            was_grounded = is_grounded;
            is_grounded = DetectObstacle(Vector3.down);
            is_fronted = IsFronted();


            if (state == PlayerCharacterState.Normal)
            {
                UpdateFacing();
                UpdateJump();

                //Move

                    move.x = is_fronted ? 0f : move.x;
                    rigid.velocity = move;

                    CheckForFloorTrigger();
            }

            if (state == PlayerCharacterState.Dead)
            {
                move.x = 0f;
                UpdateJump(); //Keep falling
                rigid.velocity = move;
            }


        }

        //Handle render and controls
        void Update()
        {

            hit_timer += Time.deltaTime;
            state_timer += Time.deltaTime;
            //grounded_timer += Time.deltaTime;

            //Controls
            PlayerControls controls = PlayerControls.Get(player_id);

            if (state == PlayerCharacterState.Normal)
            {
                if (controls.GetJumpDown())
                    Jump();

            }
            //Reset when fall

        }


        IEnumerator DestoryCoroutine()
        {
            yield return new WaitForSeconds(3f);

            _canComAbility[2] = false;

            if (gameObject.tag == "Player")
            {
               // Kill();
                gameEvent.Raise();
            }
            else if (gameObject.tag == "Bomb")
            {
                DmgObj.SetActive(true);

                foreach (var drag in _drags)
                {
                    drag.Bomb();
                }
                destroyParticle.Play();
                SoundManager.Instance.SoundFx(0);
                yield return new WaitForSeconds(0.55f);

                gameObject.SetActive(false);
            }
            else
            {
                destroyParticle.Play();
                SoundManager.Instance.SoundFx(3);
                yield return new WaitForSeconds(0.5f);
                gameObject.SetActive(false);

            }

        }

        public void DRockFx()
        {
            StartCoroutine(DRockCoroutine());
        }

        IEnumerator DRockCoroutine()
        {
            destroyParticle.Play();

            SoundManager.Instance.SoundFx(3);

            yield return new WaitForSeconds(0.55f);

            gameObject.SetActive(false);
        }

        public void DBombFx()
        {


            StartCoroutine(DBombCoroutine());
        }

        IEnumerator DBombCoroutine()
        {

            yield return new WaitForSeconds(0.1f);
            DmgObj.SetActive(true);
            destroyParticle.Play();
            SoundManager.Instance.SoundFx(0);

            yield return new WaitForSeconds(0.55f);
            gameObject.SetActive(false);
        }



        private void UpdateFacing()
        {
            if (!_canComAbility[0])
            {
                return;
            }


            if (Mathf.Abs(move.x) > 0.01f)
            {
                float side = (move.x < 0f) ? -1f : 1f;
                transform.localScale = new Vector3(start_scale.x * side, start_scale.y, start_scale.z);
            }
        }

        private void UpdateMove()
        {
            if (!_canComAbility[0])
            {
                return;
            }
            PlayerControls controls = PlayerControls.Get(player_id);
            Vector3 move_input = controls.GetMove();
            float desiredSpeed = Mathf.Abs(move_input.x) > 0.1f ? move_input.x * move_max : 0f;
            float acceleration = Mathf.Abs(move_input.x) > 0.1f ? move_accel : move_deccel;
            acceleration = !is_grounded ? jump_move_percent * acceleration : acceleration;
            move.x = Mathf.MoveTowards(move.x, desiredSpeed, acceleration * Time.fixedDeltaTime);

        }


        private void UpdateJump()
        {
            PlayerControls controls = PlayerControls.Get(player_id);

            //Jump
            jump_timer += Time.fixedDeltaTime;

            //Jump end timer
            if (is_jumping && !controls.GetJumpHold() && jump_timer > jump_time_min)
                is_jumping = false;
            if (is_jumping && jump_timer > jump_time_max)
                is_jumping = false;

            //Jump hit ceil


            //Add jump velocity
            if (!is_grounded)
            {
                //Falling
                float gravity = !is_jumping ? jump_fall_gravity : jump_gravity; //떨어질때 중력상승
                move.y = Mathf.MoveTowards(move.y, -move_max * 2f, gravity * Time.fixedDeltaTime);
            }
            else if (!is_jumping)
            {
                //Grounded
                move.y = 0f;
            }



            if (!was_grounded && is_grounded)
            {
                if (onLand != null)
                    onLand.Invoke();
            }
        }



        public void Jump(bool force_jump = false)
        {
            if (!_canComAbility[1])
            {
                return;
            }

            if (can_jump && (!force_jump))
            {
                if (is_grounded || force_jump || (!is_double_jump && double_jump))
                {
                    is_double_jump = !is_grounded;
                    move.y = jump_strength;
                    jump_timer = 0f;
                    is_jumping = true;
                    state = PlayerCharacterState.Normal;
                    state_timer = 0f;
                    if (onJump != null)
                        onJump.Invoke();
                }
            }
        }



        private void CheckForFloorTrigger()
        {
            //Platform
            Vector3 center = GetCapsulePos(Vector3.down);
            float radius = GetCapsuleRadius() + ground_raycast_dist;
            GameObject platform = RaycastObstacle<PlatformMoving>(center, Vector3.down * radius);
            if (platform && platform.GetComponent<PlatformMoving>())
            {
                PlatformMoving pmoving = platform.GetComponent<PlatformMoving>();
                pmoving.OnCharacterStep();
                rigid.position += new Vector2(pmoving.GetMove().x, pmoving.GetMove().y) * Time.fixedDeltaTime;
            }

            //Enemy
            GameObject enemy_trigger = RaycastObstacle<Enemy>(center, Vector3.down * radius);
            if (enemy_trigger && enemy_trigger.GetComponent<Enemy>())
            {
                Enemy etrigger = enemy_trigger.GetComponent<Enemy>();
                TouchEnemy(etrigger);
            }

            //Floor trigger
            GameObject floor_trigger = RaycastObstacle<FloorTrigger>(center, Vector3.down * radius);
            if (floor_trigger && floor_trigger.GetComponent<FloorTrigger>())
            {
                FloorTrigger ftrigger = floor_trigger.GetComponent<FloorTrigger>();
                ftrigger.Activate();
            }
        }

        private bool IsFronted()
        {
            bool obstacle = DetectObstacle(GetFacing());
            bool box = RaycastObstacle<Box>(GetCapsulePos(GetFacing()), GetFacing());
            bool enemy = RaycastObstacle<Enemy>(GetCapsulePos(GetFacing()), GetFacing());
            return obstacle && !box && !enemy;
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

        public void Teleport(Vector3 pos)
        {
            transform.position = pos;
            last_ground_pos = pos;
            move = Vector2.zero;
            is_jumping = false;
        }

        public void SetRespawnPoint(Vector3 pos)
        {
            last_ground_pos = pos;
        }

        public void Kill()
        {
            if (!IsDead())
            {
                state = PlayerCharacterState.Dead;
                rigid.velocity = Vector2.zero;
                move = Vector2.zero;
                state_timer = 0f;
                capsule_coll.enabled = false;

                if (onDeath != null)
                    onDeath.Invoke();
            }
        }

        public PlayerCharacterState GetState()
        {
            return state;
        }

        public Vector2 GetMove()
        {
            return move;
        }

        public Vector2 GetFacing()
        {
            return Vector2.right * Mathf.Sign(transform.localScale.x);
        }

        public bool IsJumping()
        {
            return is_jumping;
        }

        public bool IsGrounded()
        {
            return is_grounded;
        }
        




        public bool IsDead()
        {
            return state == PlayerCharacterState.Dead;
        }

        public Vector2 GetSize()
        {
            if (capsule_coll != null)
                return new Vector2(Mathf.Abs(transform.localScale.x) * capsule_coll.size.x, Mathf.Abs(transform.localScale.y) * capsule_coll.size.y);
            return new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        }

        private Vector3 GetCapsulePos(Vector3 dir)
        {
            Vector2 orientation = dir.normalized;
            Vector2 raycast_offset = capsule_coll.offset + orientation * Mathf.Abs(capsule_coll.size.y * 0.5f - capsule_coll.size.x * 0.5f);
            return rigid.position + raycast_offset * capsule_coll.transform.lossyScale.y;
        }

        private float GetCapsuleRadius()
        {
            return GetSize().x * 0.5f;
        }

        private void TouchEnemy(Enemy enemy)
        {
            Vector3 diff = GetCapsulePos(Vector3.down) - enemy.transform.position;
            if (diff.y > 0f)
            {
                Jump(true); //Bounce on enemy
               // enemy.Kill(); //Kill enemy
            }
            else
            {
                //  TakeDamage(enemy.damage);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (IsDead())
                return;


            if(collision.gameObject.tag== "Enemy")
            {
                //여기서 닿아서 죽기.
                Debug.Log("col");
                Kill();

            }



        }

        public static Actor GetNearest(Vector3 pos, float range = 99999f, bool alive_only = false)
        {
            Actor nearest = null;
            float min_dist = range;
            foreach (Actor character in GetAll())
            {
                if (!alive_only || !character.IsDead())
                {
                    float dist = (pos - character.transform.position).magnitude;
                    if (dist < min_dist)
                    {
                        min_dist = dist;
                        nearest = character;
                    }
                }
            }
            return nearest;
        }

        public static Actor Get(int player_id)
        {
            foreach (Actor character in GetAll())
            {
                if (character.player_id == player_id)
                {
                    return character;
                }
            }
            return null;
        }

        public static List<Actor> GetAll()
        {
            return character_list;
        }









        public void DrawNormal()
        {
            _myMat.shader = _defaultShader;
        }
        
        public void DrawOutline()
        {
            _myMat.shader = _OutlineShader;
            
            _myMat.SetColor(OutlineColor, Color.cyan * 2);
        }

        public void AddIndivisible(IndivisibleComponent indivisible)
        {
            indivisibleComponents.Add(indivisible);
        }
        
        public void RemoveIndivisible(IndivisibleComponent indivisible)
        {
            indivisibleComponents.Remove(indivisible);
        }

        public void AddDrag(Drag drag)
        {
            _drags.Add(drag);
            if (isRealTime)
            {
                GameStartSetting();
                drag.SetInteractable(false);
            }
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

        public Vector2 GetDragPos(Drag drag)
        {

            var targetSprite = _spriteRenderer.sprite;
            var croppedRect = GetCroppedRect(targetSprite);
            
            var spriteOffset = GetSpriteOffset(croppedRect, _spriteRenderer);
            
            var targetHead = transform.position + new Vector3(spriteOffset.x, spriteOffset.y ,0);

            var index = _drags.FindIndex(x => x == drag);

            targetHead.y += + (indivisibleComponents.Count + index + 1 ) * buttonHeight;

            return targetHead;
        }
        
        Rect GetCroppedRect(Sprite targetSprite)
        {
            var result = new Rect(targetSprite.textureRectOffset.x / targetSprite.pixelsPerUnit,
                targetSprite.textureRectOffset.y / targetSprite.pixelsPerUnit,
                targetSprite.textureRect.width / targetSprite.pixelsPerUnit,
                targetSprite.textureRect.height / targetSprite.pixelsPerUnit);

            return result;
        }
        
                    
        Vector2 GetSpriteOffset(Rect croppedRect, SpriteRenderer spriteRenderer)
        {
            var result = new Vector2(spriteRenderer.bounds.extents.x - croppedRect.center.x,
                -spriteRenderer.bounds.extents.y + croppedRect.yMax);
            return result;
        }

        public Vector2 GetIndivisiblePos(IndivisibleComponent indivisibleComponent)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            
            var targetSprite = spriteRenderer.sprite;
            var croppedRect = GetCroppedRect(targetSprite);
            
            var spriteOffset = GetSpriteOffset(croppedRect, spriteRenderer);
            
            var index = indivisibleComponents.FindIndex(x => x == indivisibleComponent);
            
            var targetHead = transform.position + new Vector3(spriteOffset.x, spriteOffset.y ,0);
            
            targetHead.y += + (index + 1) * buttonHeight;

            return targetHead;
        }
        
        

    }
}