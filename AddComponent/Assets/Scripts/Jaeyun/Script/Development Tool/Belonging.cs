// using System.Collections;
// using UnityEngine;
//
// namespace UnityTemplateProjects.Jaeyun.Script.Development_Tool
// {
//     public class Belonging : MonoBehaviour
//     {
//         private ComponentButton _componentButton;
//         
//         private Actor.Actor _actor;
//         private RectTransform _rectTransform;
//
//         public LayerMask actorMask;
//
//         private Button _button;
//
//         private Camera _cam;
//
//         public void InitializeDrag(ComponentButton componentButton, Sprite sprite)
//         {
//             _button = GetComponent<Button>();
//             
//             _rectTransform = GetComponent<RectTransform>();
//             
//             _componentButton = componentButton;
//             _button.image.sprite = sprite;
//             
//             _cam = Camera.main;
//             
//             BeginDrag();
//         }
//
//         public void BeginDrag()
//         {
//             _actor?.RemoveDrag(this);
//             
//             StartCoroutine(ChaseMouse());
//             StartCoroutine(CheckActor());
//         }
//
//         public void EndDrag()
//         {
//             if (!Attach())
//             {
//                 _componentButton.DestroyDrag();
//             }
//         }
//
//         public bool Attach()
//         {
//             _actor?.DrawNormal();
//
//             var allDrags = FindObjectsOfType<Drag>();
//             
//             if (_actor != null && !_actor.isLocked)
//             {
//                 _actor.AddDrag(this);
//                 SortUpperActorHead();
//                 StopAllCoroutines();
//                 return true;
//             }
//             else
//             {
//                 
//                 return false;
//             }
//
//             
//         }
//
//         public void SortUpperActorHead()
//         {
//             var targetPos = _actor.GetComponentPos(this);
//             _rectTransform.position = _cam.WorldToScreenPoint(targetPos);
//         }
//
//     }
// }