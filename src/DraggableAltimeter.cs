using IOUtils = KSP.IO.IOUtils;
using Altimeter = KSP.UI.Screens.Flight.AltitudeTumbler;

using UnityEngine;
using UnityEngine.EventSystems;

/* Two classes are needed for DraggableNavball. One is the component that
 * implements the drag behavior. I can't work out how to make it add itself
 * to the navball, so a second class (NavBallAttacher) is rigged to run at
 * flight startup solely to add the component.
 *
 * I feel like there should be a cleaner way to do this but haven't found it
 * yet.
 */

[KSPAddon(KSPAddon.Startup.Flight, false)]
public class AltimeterAttacher : MonoBehaviour
{
	void Start()
	{
		/* When we speak of dragging the navball, what we really want
		 * is to drag the entire SAS/navball/maneuver control cluster.
		 * It has the type "IVAEVACollapseGroup" but I can't figure out
		 * where that's defined so I can't search for it. Taking the
		 * parent of the NavBall works just as well.
		 *
		 * This doesn't drag the (invisible) frame around the navball's
		 * control cluster, but that doesn't seem to hurt anything.
		 */
		GameObject altimeter = GameObject.Find("Altimeter");
		GameObject slideframeparent = altimeter.transform.parent.gameObject;
		GameObject cluster = slideframeparent.transform.parent.gameObject;
		cluster.AddComponent<AltDrag>();
	}
}
public class AltDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
{
	/* This component makes the navball draggable. Movement is restricted
	 * to the x axis, so the ball slides along the bottom edge of the
	 * screen.
	 */

	/* I can't figure out how to make ConfigNode.Load do the right thing
	 * with C# properties, so NAVBALL_COORD gets mapped to a private
	 * variable that serves just as a place to keep save-load data.
	 */
	[Persistent]
	private float ALTIMETER_XCOORD;

	private Vector2 dragstart;
	private Vector3 altstart;

	const string cfgfile = "DraggableAltimeter.cfg";

	void Start()
	{
		string path = IOUtils.GetFilePathFor(this.GetType(), cfgfile);
		ConfigNode config = ConfigNode.Load(path);
		ConfigNode.LoadObjectFromConfig(this, config);
		xpos = ALTIMETER_XCOORD;
	}

	public float xpos
	{
		/* Assigning to xpos moves the navball. Don't do it any other
		 * way; stuff necessary for consistency happens here.
		 */
		get { return transform.position.x; }
		set
		{
			// Prevent the ball from ending up offscreen where we
			// can't drag it back.
			int right_edge = GameSettings.SCREEN_RESOLUTION_WIDTH / 2;
			int left_edge  = -right_edge;
			if (value > right_edge) value = right_edge;
			if (value < left_edge)  value = left_edge;

			// Move the ball.
			Vector3 newpos = transform.position;
			newpos.x = value;
			transform.position = newpos;
		}
	}

	public void OnBeginDrag(PointerEventData evtdata)
	{
		/* Record the pointer and ball positions when dragging starts.
		 * We'll need them in OnDrag. */
		dragstart = evtdata.position;
		altstart = transform.position;
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
		xpos = altstart.x + dragdist.x;
	}

	public void Update()
	{
		xpos = xpos;
	}

	public void OnDestroy()
	{
		// I'm not sure when you're "supposed" to save plugin data.
		// This was the best I could come up with.
		ALTIMETER_XCOORD = xpos;
		ConfigNode config = ConfigNode.CreateConfigFromObject(this);
		string path = IOUtils.GetFilePathFor(this.GetType(), cfgfile);
		config.Save(path);
	}
}
