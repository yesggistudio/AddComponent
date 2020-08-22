using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	private static EventManager instance;
	public static EventManager Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance)
		{
			DestroyImmediate(gameObject);
			return;
		}
		instance = this;
	}
	private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new Dictionary<EVENT_TYPE, List<IListener>>();

	public void AddListener(EVENT_TYPE eventType, IListener listener)
	{
		List<IListener> ListenList = null;
		if (Listeners.TryGetValue(eventType, out ListenList))
		{
			ListenList.Add(listener);
			return;
		}
		ListenList = new List<IListener>();
		ListenList.Add(listener);
		Listeners.Add(eventType, ListenList);
	}

	public void EventPost(EVENT_TYPE eventType, Component sender, Object param = null)
	{
		List<IListener> ListenList = null;
		if (!Listeners.TryGetValue(eventType, out ListenList))
			return;
		foreach (IListener listener in ListenList)
		{
			if (!listener.Equals(null))
				listener.OnEvent(eventType, sender, param);
		}
	}

	public void RemoveEvent(EVENT_TYPE eventType)
	{
		Listeners.Remove(eventType);
	}

	public void Initialize()
	{
		Dictionary<EVENT_TYPE, List<IListener>> tempListeners = new Dictionary<EVENT_TYPE, List<IListener>>();
		foreach (KeyValuePair<EVENT_TYPE, List<IListener>> Depot in Listeners)
		{
			for (int i = Depot.Value.Count - 1; i >= 0; i--)
			{
				if (Depot.Value[i].Equals(null))
					Depot.Value.RemoveAt(i);
			}
			if (Depot.Value.Count > 0)
				tempListeners.Add(Depot.Key, Depot.Value);
		}
		Listeners = tempListeners;
	}

}