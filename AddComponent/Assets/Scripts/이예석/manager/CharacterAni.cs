using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Actor
{
    [RequireComponent(typeof(Actor))]
    [RequireComponent(typeof(Animator))]

    public class CharacterAni : MonoBehaviour
    {
        private Actor character;
        private SpriteRenderer render;
        private Animator animator;
        private float flash_fx_timer;

        void Awake()
        {
            character = GetComponent<Actor>();
            render = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            character.onJump += OnJump;
            character.onDeath += OnDeath;
        }


        void Update()
        {

            //Anims
            animator.SetBool("Jumping", character.IsJumping());
            animator.SetBool("InAir", !character.IsGrounded());
            animator.SetFloat("Speed", character.GetMove().magnitude);

            //Hit flashing
            render.color = new Color(render.color.r, render.color.g, render.color.b, 1f);
            if (flash_fx_timer > 0f)
            {
                flash_fx_timer -= Time.deltaTime;
                float alpha = Mathf.Abs(Mathf.Sin(flash_fx_timer * Mathf.PI * 4f));
                render.color = new Color(render.color.r, render.color.g, render.color.b, alpha);
            }
        }



        void OnJump()
        {
            animator.SetTrigger("Jump");
            animator.SetBool("Jumping", true);
        }


        void OnDeath()
        {
            animator.SetTrigger("Death");
        }
    }

}