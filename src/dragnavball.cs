using IOUtils = KSP.IO.IOUtils;
using NavBall = KSP.UI.Screens.Flight.NavBall;

using UnityEngine;
using UnityEngine.EventSystems;

[KSPAddon(KSPAddon.Startup.Flight, false)]
public class NavBallAttacher : MonoBehaviour
{
	void Start()
	{
		/* When we speak of dragging the navball, what we really want
		 * is to drag the entire SAS/navball/maneuver control panel.
		 * It has the type "IVAEVACollapseGroup" but I can't figure out
		 * where that's defined so I can't search for it. Taking the
		 * parent of the NavBall works just as well.
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
	private float NAVBALL_XCOORD;

	private Vector2 dragstart;
	private Vector3 ballstart;

	const string modname = "Draggable Navball";
	const string cfgfile = "DraggableNavball.cfg";

	private void place_navball()
	{
		Vector3 newpos = transform.position;
		newpos.x = NAVBALL_XCOORD;
		transform.position = newpos;
	}

	void Start()
	{
		string path = IOUtils.GetFilePathFor(this.GetType(), cfgfile);
		ConfigNode config = ConfigNode.Load(path);
		ConfigNode.LoadObjectFromConfig(this, config);
		place_navball();
	}

	public float xpos
	{
		get { NAVBALL_XCOORD = transform.position.x; return NAVBALL_XCOORD; }
		set { NAVBALL_XCOORD = value; place_navball(); }
	}

	public void OnBeginDrag(PointerEventData evtdata)
	{
		dragstart = evtdata.position;
		ballstart = transform.position;
	}
	public void OnDrag(PointerEventData evtdata)
	{
		Vector2 dragdist = evtdata.position - dragstart;
		xpos = ballstart.x + dragdist.x;
	}
	public void OnDestroy()
	{
		ConfigNode node = new ConfigNode(modname);
		ConfigNode.CreateConfigFromObject(this, node);
		string path = IOUtils.GetFilePathFor(this.GetType(), cfgfile);
		node.Save(path);
	}
}
