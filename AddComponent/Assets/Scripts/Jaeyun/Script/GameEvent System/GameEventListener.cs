using UnityEngine;
using UnityEngine.Events;

namespace Jaeyun.Script.GameEvent_System
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        {

            if (Event == null)
            {
			
#if UNITY_EDITOR
                Debug.Log(this.name + "'s Event is not exist'");
#endif
                return;
            }

            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (Event == null)
            {
#if UNITY_EDITOR
                Debug.Log(this.name + "'s Event is not exist'");
#endif
                return;
            }
		
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
		
// #if UNITY_EDITOR
// 		Debug.Log("Evnet Listener : " + this.name + "\nResponse : " + Response.ToString());
// #endif
		
            Response.Invoke();
        }
	
        public void OnEventRaised<T>(T param1)
        {
		
//#if UNITY_EDITOR
//		Debug.Log("Evnet Listener : " + this.name + "\nResponse : " + Response.ToString());
//#endif
            for(int i = 0 ; i < Response.GetPersistentEventCount(); i++ ){    
                ((MonoBehaviour)Response.GetPersistentTarget(i)).SendMessage(Response.GetPersistentMethodName(i),param1);
            }
		
        }
    }
}