//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class CharacterMovement : MonoBehaviour
//{
//    public int player_id;

//    [Header("Stats")]
//    public float max_hp = 100f;

//    [Header("Movement")]
//    public float move_accel = 20f;
//    public float move_deccel = 20f;
//    public float move_max = 5f;

//    [Header("Jump")]
//    public bool can_jump = true;
//    public bool double_jump = true; // Double() 이라는 컴포넌트에 의해 이중점프 나올수도 있으니 

//    public float jump_strength = 5f;
//    public float jump_time_min = 0.4f;
//    public float jump_time_max = 0.8f;
//    public float jump_gravity = 5f;
//    public float jump_fall_gravity = 10f;
//    public float jump_move_percent = 0.75f;
//    public LayerMask raycast_mask;
//    public float ground_raycast_dist = 0.1f;

//    [Header("Climb")]
//    public float climb_speed = 2f;


//    public UnityAction onDeath;
//    public UnityAction onHit;
//    public UnityAction onJump;
//    public UnityAction onLand;
//    public UnityAction onClimb;

//    private Rigidbody2D rigid;
//    private CapsuleCollider2D capsule_col;
//    private ContactFilter2D contact_filter;
//    private Vector2 col_start_h;
//    private Vector2 col_start_off;
//    private Vector3 start_scale;
//    private Vector3 last_ground_pos;

//    private CharacterStates.MovementStates state;
//    private Vector2 move;

//    private float hp;
//    private bool was_grounded = false;
//    private bool is_grounded = false;
//    private bool is_fronted = false;
//    private bool is_jumping = false;
//    private bool is_double_jump = false;

//    private float state_timer = 0f;
//    private float jump_timer = 0f;
//    private float hit_timer = 0f;

//    private static List<CharacterMovement> character_list = new List<CharacterMovement>();


//    private void Awake()
//    {

//        character_list.Add(this);
//        rigid = GetComponent<Rigidbody2D>();
//        capsule_col = GetComponent<CapsuleCollider2D>();
//        col_start_h = capsule_col.size;
//        col_start_off = capsule_col.offset;
//        start_scale = transform.localScale;

//        last_ground_pos = transform.position;
//        hp = max_hp;

//        contact_filter = new ContactFilter2D();
//        contact_filter.layerMask = raycast_mask;
//        contact_filter.useLayerMask = true;
//        contact_filter.useTriggers = false;
        
//    }

//    private void OnDestroy()
//    {
//        character_list.Remove(this);
//    }

//    private void FixedUpdate()
//    {

//        PlayerControls controls = PlayerControls.Get(player_id);

//        Vector3 move_input = controls.GetMove();
//        float desiredSpeed = Mathf.Abs(move_input.x) > 0.1f ? move_input.x * move_max : 0f;
//        float acceleration = Mathf.Abs(move_input.x) > 0.1f ? move_accel : move_deccel;
//        acceleration = !is_grounded ? jump_move_percent * acceleration : acceleration;
//        move.x = Mathf.MoveTowards(move.x, desiredSpeed, acceleration * Time.fixedDeltaTime);

//        was_grounded = is_grounded;
//        is_grounded = DetectObstacle(Vector3.down);
//        is_fronted = IsFronted();

//        if (state == CharacterStates.MovementStates.Normal)
//        {
//            UpdateJump();

//            //Move
//            move.x = is_fronted ? 0f : move.x;
//            rigid.velocity = move;

//            CheckForFloorTrigger();
//        }

//        if (state == CharacterStates.MovementStates.Climb)
//        {
//            move = controls.GetMove() * climb_speed;
//            rigid.velocity = move;
//        }

//        if (state == CharacterStates.MovementStates.Dead)
//        {
//            move.x = 0f;
//            UpdateJump(); //Keep falling
//            rigid.velocity = move;
//        }
//    }

//    private void Update()
//    {
//        hit_timer += Time.deltaTime;
//        state_timer += Time.deltaTime;

//        PlayerControls controls = PlayerControls.Get(player_id);

//        if (state == CharacterStates.MovementStates.Normal)
//        {
//            if (controls.GetJumpDown())
//                Jump();

//            Ladder ladder = Ladder.GetOverlapLadder(gameObject);
//            if (ladder && controls.GetMove().y > 0.1f && state_timer > 0.7f)
//                Climb();


//        }

//        if (state == CharacterStates.MovementStates.Climb)
//        {
//            Ladder ladder = Ladder.GetOverlapLadder(gameObject);
//            if (ladder == null)
//            {
//                state = CharacterStates.MovementStates.Normal;
//                state_timer = 0f;
//            }

//            if (controls.GetJumpDown())
//                Jump(true);
//        }

//        //Reset when fall
//        if (!IsDead() && transform.position.y < fall_pos_y - GetSize().y)
//        {
//            TakeDamage(fall_damage);
//            if (reset_when_fall)
//                //Teleport(last_ground_pos); //혹은 게임 오버
//        }
//    }



//    private void UpdateJump()
//    {
//        PlayerControls controls = PlayerControls.Get(player_id);

//        //Jump
//        jump_timer += Time.fixedDeltaTime;

//        //Jump end timer
//        if (is_jumping && !controls.GetJumpHold() && jump_timer > jump_time_min)
//            is_jumping = false;
//        if (is_jumping && jump_timer > jump_time_max)
//            is_jumping = false;


//        //Add jump velocity
//        if (!is_grounded)
//        {
//            //Falling
//            float gravity = !is_jumping ? jump_fall_gravity : jump_gravity; // 내려갈때 중력값상승
//            move.y = Mathf.MoveTowards(move.y, -move_max * 2f, gravity * Time.fixedDeltaTime);
//        }
//        else if (!is_jumping)
//        {
//            //Grounded
//            move.y = 0f;
//        }

//        if (!was_grounded && is_grounded)
//        {
//            if (onLand != null)
//                onLand.Invoke();
//        }
//    }

//    public void Jump(bool force_jump = false)
//    {
//        if (can_jump && force_jump)
//        {
//            if (is_grounded || force_jump || (is_double_jump && double_jump))
//            {
//                is_double_jump = !is_grounded;
//                move.y = jump_strength;
//                jump_timer = 0f;
//                is_jumping = true;
//                state = CharacterStates.MovementStates.Normal;
//                state_timer = 0f;
//                if (onJump != null)
//                    onJump.Invoke();
//            }
//        }
//    }

//    public void Climb()
//    {
//        state = CharacterStates.MovementStates.Climb;
//        state_timer = 0f;

//        if(onClimb != null)
//        {
//            onClimb.Invoke();
//        }
//    }

//    private void CheckForFloorTrigger()
//    {
//        //Platform
//        Vector3 center = GetCapsulePos(Vector3.down);
//        float radius = GetCapsuleRadius() + ground_raycast_dist;
//        GameObject platform = RaycastObstacle<PlatformMoving>(center, Vector3.down * radius);
//        if (platform && platform.GetComponent<PlatformMoving>())
//        {
//            PlatformMoving pmoving = platform.GetComponent<PlatformMoving>();
//            pmoving.OnCharacterStep();
//            rigid.position += new Vector2(pmoving.GetMove().x, pmoving.GetMove().y) * Time.fixedDeltaTime;
//        }

//        //Enemy
//        GameObject enemy_trigger = RaycastObstacle<Enemy>(center, Vector3.down * radius);
//        if (enemy_trigger && enemy_trigger.GetComponent<Enemy>())
//        {
//            Enemy etrigger = enemy_trigger.GetComponent<Enemy>();
//            TouchEnemy(etrigger);
//        }

//        //Floor trigger
//        GameObject floor_trigger = RaycastObstacle<FloorTrigger>(center, Vector3.down * radius);
//        if (floor_trigger && floor_trigger.GetComponent<FloorTrigger>())
//        {
//            FloorTrigger ftrigger = floor_trigger.GetComponent<FloorTrigger>();
//            ftrigger.Activate();
//        }

//    }

//    private bool IsFronted()
//    {

//        return false;
//    }


//    private bool DetectObstacle()
//    {
//        return true;
//    }






//    public bool RaycastObstacle(Vector2 pos, Vector2 dir)
//    {
//        RaycastHit2D[] hitBuffer = new RaycastHit2D[5];
//        Physics2D.Raycast(pos, dir.normalized, contact_filter, hitBuffer, dir.magnitude);
//        for (int j = 0; j < hitBuffer.Length; j++)
//        {
//            if (hitBuffer[j].collider != null && hitBuffer[j].collider != capsule_coll && !hitBuffer[j].collider.isTrigger)
//            {
//                return true;
//            }
//        }
//        return false;
//    }
//    public GameObject RaycastObstacle<T>(Vector2 pos, Vector2 dir)
//    {
//        RaycastHit2D[] hitBuffer = new RaycastHit2D[5];
//        Physics2D.Raycast(pos, dir.normalized, contact_filter, hitBuffer, dir.magnitude);
//        for (int j = 0; j < hitBuffer.Length; j++)
//        {
//            if (hitBuffer[j].collider != null && hitBuffer[j].collider != capsule_coll && !hitBuffer[j].collider.isTrigger)
//            {
//                if (hitBuffer[j].collider.GetComponent<T>() != null)
//                    return hitBuffer[j].collider.gameObject;
//            }
//        }
//        return null;
//    }






//    public Vector2 GetSize()
//    {
//        if (capsule_coll != null)
//            return new Vector2(Mathf.Abs(transform.localScale.x) * capsule_coll.size.x, Mathf.Abs(transform.localScale.y) * capsule_coll.size.y);
//        return new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
//    }

//    private Vector3 GetCapsulePos(Vector3 dir)
//    {
//        Vector2 orientation = dir.normalized;
//        Vector2 raycast_offset = capsule_col.offset + orientation * Mathf.Abs(capsule_col.size.y * 0.5f - capsule_col.size.x * 0.5f);
//        return rigid.position + raycast_offset * capsule_col.transform.lossyScale.y;
//    }
//    private float GetCapsuleRadius()
//    {
//        return GetSize().x * 0.5f;
//    }


//}
