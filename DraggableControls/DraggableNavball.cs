using IOUtils = KSP.IO.IOUtils;
using NavBall = KSP.UI.Screens.Flight.NavBall;

using UnityEngine;
using UnityEngine.EventSystems;

namespace DraggableControls
{
	public class NavBallDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		/* This component makes the navball draggable. Movement is restricted
		 * to the x axis, so the ball slides along the bottom edge of the
		 * screen.
		 */

		private Vector2 dragstart;
		private Vector3 ballstart;

		void Start()
		{
			Config.navBallTransform = transform;
			Config.navBallXpos = Config.NAVBALL_XCOORD;
		}

		public void OnBeginDrag(PointerEventData evtdata)
		{
			/* Record the pointer and ball positions when dragging starts.
			 * We'll need them in OnDrag. */
			dragstart = evtdata.position;
			ballstart = transform.position;
		}

		public void OnDrag(PointerEventData evtdata)
		{
			/* See how far the pointer has dragged and offset the navball
			 * from its original position by that much.
			 *
			 * This might be cleaner with a frame-by-frame delta but I
			 * worry about small errors adding up.
			 */
			Vector2 dragdist = evtdata.position - dragstart;
			Config.navBallXpos = ballstart.x + dragdist.x;
		}

		public void OnEndDrag(PointerEventData evtdata)
		{
			Config.NAVBALL_XCOORD = Config.navBallXpos;
			Config.Save();
		}
	}
}