using IOUtils = KSP.IO.IOUtils;
using NavBall = KSP.UI.Screens.Flight.NavBall;

using UnityEngine;
using UnityEngine.EventSystems;


namespace DraggableControls
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class Attacher : MonoBehaviour
	{
		void Start()
		{
			/* Rather than only dragging the altimeter, we really want to
			 * drag the entire control cluster it's part of.  So the
			 * dragger gets attached to that. It's the grandparent of the
			 * altimeter object. (Found this using DebugStuff for the
			 * curious)
			 */

			GameObject altimeter = GameObject.Find("Altimeter");
			GameObject slideframeparent = altimeter.transform.parent.gameObject;
			GameObject cluster = slideframeparent.transform.parent.gameObject;
			cluster.AddComponent<AltimeterDragger>();

			/* When we speak of dragging the navball, what we really want
			 * is to drag the entire SAS/navball/maneuver control cluster.
			 * It has the type "IVAEVACollapseGroup" but I can't figure out
			 * where that's defined so I can't search for it. Taking the
			 * parent of the NavBall works just as well.
			 *
			 * This doesn't drag the (invisible) frame around the navball's
			 * control cluster, but that doesn't seem to hurt anything.
			 */
			GameObject navball = FindObjectOfType<NavBall>().gameObject;
			cluster = navball.transform.parent.gameObject;
			cluster.AddComponent<NavBallDrag>();

			Destroy(gameObject);
		}
	}

}
