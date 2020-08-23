using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Actor;

public class Hand : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Bomb")
        {
            collision.gameObject.GetComponent<Actor>().DBombFx();
            BossAttack.Instance.BossHpMinus();
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player:hit");

            collision.gameObject.GetComponent<Actor>().gameEvent.Raise();
        }

    }
}
