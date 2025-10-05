using UnityEngine;

public class AlwaysLastChild : MonoBehaviour
{
	void Update() => transform.SetAsLastSibling();
}
