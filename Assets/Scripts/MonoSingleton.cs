using System.Collections.Generic;
using UnityEngine;


public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	private static T _instance;

	public static bool HasInstance => _instance != null;

	public static T Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = (T)Object.FindFirstObjectByType(typeof(T));
				if(_instance == null)
				{
					_instance = new GameObject(typeof(MonoSingleton<T>).ToString(), typeof(T)).GetComponent<T>();
				}
			}

			return _instance;
		}
	}

	public virtual void Awake()
	{
		if (_instance != null)
		{
			Destroy(this.gameObject);
		}
	}
}

public class RegisteredBehaviour<T> : MonoBehaviour where T : RegisteredBehaviour<T>
{
	private static readonly List<T> _instances = new();

	public static IReadOnlyList<T> instances => _instances;

	protected virtual void Start()
	{
		var instance = (T)this;
		if (!_instances.Contains(instance))
			_instances.Add(instance);
	}

	protected virtual void OnDestroy()
	{
		_ = _instances.Remove((T)this);
	}
}
