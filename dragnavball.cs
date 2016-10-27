using KSP.UI.Screens.Flight;
using KSP;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[KSPAddon(KSPAddon.Startup.Flight, false)]
public class NavBallAttacher : MonoBehaviour
{
	void Start()
	{
		/* When we speak of dragging the navball, what we really want
		 * is to drag the entire SAS/navball/maneuver control panel.
		 * It has the type "IVAEVACollapseGroup" but that's not
		 * available and I'm not sure which assembly provides it.
		 * Taking the parent of the NavBall works just as well.
		 *
		 * This doesn't drag the (invisible) frame around the control
		 * cluster, but that doesn't seem to hurt anything.
		 */
		GameObject navball = FindObjectOfType<NavBall>().gameObject;
		GameObject cluster = navball.transform.parent.gameObject;
		cluster.AddComponent<NavBallDrag>();
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

	[Persistent]
	float ballpos;

	Vector2 dragstart;
	Vector3 ballstart;

	void Start()
	{
		// FIXME: Load from persistant file here or do nothing.
		// FIXME: When do we save? On destroy?
		ballpos = GameSettings.UI_POS_NAVBALL * GameSettings.SCREEN_RESOLUTION_WIDTH / 2f;
		place(ballpos);
	}

	void place(float x)
	{
		Vector3 newpos = transform.position;
		newpos.x = x;
		transform.position = newpos;
		ballpos = transform.position.x;
		GameSettings.UI_POS_NAVBALL = ballpos / (GameSettings.SCREEN_RESOLUTION_WIDTH / 2f);
	}

	public void OnBeginDrag(PointerEventData evtdata)
	{
		dragstart = evtdata.position;
		ballstart = transform.position;
	}
	public void OnDrag(PointerEventData evtdata)
	{
		Vector2 dragdist = evtdata.position - dragstart;
		place(ballstart.x + dragdist.x);
	}
}
