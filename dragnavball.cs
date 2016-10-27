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
public class NavBallDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
{
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

	Vector2 dragstart;
	Vector3 ballstart;

	public void OnBeginDrag(PointerEventData evtdata)
	{
		dragstart = evtdata.position;
		ballstart = transform.position;
	}
	public void OnDrag(PointerEventData evtdata)
	{
		Vector3 dragdist = evtdata.position - dragstart;
		dragdist.Scale(Vector3.right); // flatten the drag vector
		transform.position = ballstart + dragdist;
	}
}
