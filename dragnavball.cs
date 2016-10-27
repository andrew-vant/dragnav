using KSP.UI.Screens.Flight;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[KSPAddon(KSPAddon.Startup.Flight, false)]
public class NavBallAttacher : MonoBehaviour
{
	void Start()
	{
		print("Starting draggable navball");
		GameObject ball = FindObjectOfType<NavBall>().gameObject;
		GameObject frame = ball.transform.parent.gameObject.transform.parent.gameObject;

		// Not sure which of these is actually doing the work.
		// ball.AddComponent<NavBallDrag>();
		frame.AddComponent<NavBallDrag>();

		// Show the object tree while I work out which object actually
		// needs to be dragged...
		string path = "/";
		for (GameObject obj = ball.gameObject; obj != null; obj = obj.transform.parent.gameObject)
		{
			print(obj.GetType().Name + ":" + obj.name);
			path = "/" + obj.GetType().Name + ":" + obj.name + path;
		}
		print("Done");
	}
}
public class NavBallDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
	float ptrstart;
	float ballstart;

	public void OnBeginDrag(PointerEventData evtdata)
	{
		print("Navball drag start");
		print(transform.position);
		ptrstart = evtdata.position.x;
		ballstart = transform.position.x;
	}
	public void OnDrag(PointerEventData evtdata)
	{
		float x = ballstart + (evtdata.position.x - ptrstart);
		float y = transform.position.y;
		float z = transform.position.z;
		transform.position = new Vector3(x, y, z);
		print(transform.position);
	}
	public void OnEndDrag(PointerEventData evtdata)
	{
		print("Drag finished");
		print(transform.position);
	}
	public void OnDrop(PointerEventData evtdata)
	{
		print("Navball dropped");
		print(transform.position);
	}
}
