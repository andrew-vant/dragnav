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
		// We want to drag the navball's frame around. The frame is
		// the grandparent of the ball itself.
		GameObject ball = FindObjectOfType<NavBall>().gameObject;
		GameObject frame = ball.transform.parent.gameObject.transform.parent.gameObject;
		frame.AddComponent<NavBallDrag>();
	}
}
public class NavBallDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
	float ptrstart;
	float ballstart;
	/* This component makes the navball draggable. It works by recording
	 * the pointer and ball's position when a drag starts, then moving the
	 * ball however far the pointer moved along the X axis.
	 *
	 * Simply setting the ball to the pointer's position doesn't work; the
	 * pointer and ball have different coordinate origins. The pointer's
	 * X:0 is the left edge of the screen; The ball's X:0 is the center.
	 *
	 * Restricting movement to the X axis makes the ball frame slide along
	 * the bottom edge of the screen.
	 */

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
