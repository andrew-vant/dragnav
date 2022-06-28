using IOUtils = KSP.IO.IOUtils;
using Altimeter = KSP.UI.Screens.Flight.AltitudeTumbler;

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/* How this works: There is one class, the AltDrag component, that implements
 * drag behavior for the altimeter. The second class, the Attacher, spawns when
 * flight starts and adds the component to the altimeter. I can't work out how
 * to make the dragger attach itself. I feel like there should be a cleaner way
 * to do this but I haven't found it yet.
 */

namespace DraggableControls
{
	public class AltimeterDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		/* This makes the altimeter draggable. Movement is restricted to the x
		 * axis, so it slides along the edge of the screen.
		 */

		private Vector2 dragstart;
		private Vector3 altstart;

		void Start()
		{
			Config.altimeterTransform = transform;
		}

		// The altimeter Xpos needs to be constantly reset, KSP seems to always reset it to 0
		// The navball Xpos doesn't need that, probably due to it now being movable in the game config
		public void Update()
		{
			Config.altimeterXpos = Config.ALTIMETER_XCOORD;
			//Config.navBallXpos = Config.NAVBALL_XCOORD;
		}

		public void OnBeginDrag(PointerEventData evtdata)
		{
			/* Record the pointer and altimeter positions when dragging
			 * starts.  We'll need them in OnDrag. */
			dragstart = evtdata.position;
			altstart = transform.position;
		}

		public void OnDrag(PointerEventData evtdata)
		{
			/* See how far the pointer has dragged and offset the altimeter
			 * from its original position by that much.
			 *
			 * This might be cleaner with a frame-by-frame delta but I
			 * worry about small errors adding up.
			 */
			Vector2 dragdist = evtdata.position - dragstart;
			Config.altimeterXpos = altstart.x + dragdist.x;
		}

		public void OnEndDrag(PointerEventData evtdata)
		{
			Config.ALTIMETER_XCOORD = Config.altimeterXpos;
			Config.Save();
		}
	}
}