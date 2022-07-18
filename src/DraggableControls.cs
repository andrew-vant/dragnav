using System;

using IOUtils = KSP.IO.IOUtils;
using NavBall = KSP.UI.Screens.Flight.NavBall;
using UnityEngine;
using UnityEngine.EventSystems;

/* How this works: There are "Dragger" subclasses for each UI element we
 * want to drag. A separate "Attacher" class is responsible for
 * connecting them to the UI elements at flight start. I would prefer
 * for the components to attach themselves, but haven't figured out how
 * to do it.
 *
 * Note that when we speak of dragging the navball or altimeter or
 * whatever, we're really referring to the control clusters containing
 * them, not the elements themselves. For the curious, I found the
 * appropriate objects using DebugStuff. Accessing them is a bit of a
 * pain.
 */


[KSPAddon(KSPAddon.Startup.Flight, false)]
public class DraggerAttacher : MonoBehaviour
{
	void Start()
	{
		/* TODO: I'm unhappy with this. I *want* to make a component
		 * draggable by simply subclassing Dragger and providing an
		 * appropriate targeting method. I know how to do that in
		 * Python (using classmethods and __init_subclass__), but I
		 * can't figure out the equivalent in C#.
		 *
		 * If I *did* figure it out, third party modders could make
		 * things draggable without requiring changes to dragctrl
		 * itself.
		 */
		NavballDragger.target().AddComponent<NavballDragger>();
		AltimeterDragger.target().AddComponent<AltimeterDragger>();
		Destroy(gameObject);
	}
}


class Screen
{
	/* Helper class for concisely referencing the screen dimensions */
	public static int width;
	public static int height;
	public static int right;
	public static int left;
	public static int top;
	public static int bottom;

	static Screen()
	{
		width = GameSettings.SCREEN_RESOLUTION_WIDTH;
		height = GameSettings.SCREEN_RESOLUTION_HEIGHT;
		right = width / 2;
		left = width / -2;
		top = height / 2;
		bottom = height / -2;
	}
}


abstract class Dragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	const string CONFIG_FILE = "DraggableControls.cfg";
	const string CONFIG_DIR = "GameData/DraggableControls/PluginData";
	private static string CONFIG_PATH;
	protected static ConfigNode config;

	private Vector2 drag_start;
	private Vector2 pos_start;
	private Vector2 pos_last;
	private bool lock_vertical = true;


	/* DRAGGER SUBCLASSES MUST PROVIDE THE FOLLOWING:
	 *
	 * CONFIG_PREFIX specifies the config file key used to save the
	 * position of the target object.
	 *
	 * target() locates and returns the game object the dragger should
	 * be attached to.
	 */
	public abstract string CONFIG_PREFIX { get; }
	public static GameObject target() { throw new NotImplementedException(); }

	static Dragger()
	{
		/* I don't know of a hook to run a static method on KSP startup
		 * without trying to create an instance (which it can't for an
		 * abstract); a static constructor should do the job though. */
		CONFIG_PATH = IOUtils.GetFilePathFor(typeof(Dragger), CONFIG_FILE);
		config = ConfigNode.Load(CONFIG_PATH);
	}

	public void Start()
	{
		lock_vertical = bool.Parse(config.GetValue($"{CONFIG_PREFIX}_VLOCK"));
		var pos = new Vector2(
			float.Parse(config.GetValue($"{CONFIG_PREFIX}_XPOS")),
			float.Parse(config.GetValue($"{CONFIG_PREFIX}_YPOS"))
			);
		reposition(pos);
	}

	protected virtual void reposition(Vector2 pos)
	{
		Vector3 newpos = transform.position;
		newpos.x = Mathf.Clamp(pos.x, Screen.left, Screen.right);
		if (!lock_vertical)
			newpos.y = Mathf.Clamp(pos.y, Screen.top, Screen.bottom);
		pos_last = newpos;
		transform.position = newpos;
	}

	public void OnBeginDrag(PointerEventData evt)
	{
		/* Record the event and the object position when dragging
		 * starts. We'll need them in OnDrag. */
		drag_start = evt.position;
		pos_start = transform.position;
	}

	public void OnDrag(PointerEventData now)
	{
		/* See how far the pointer has dragged and offset the object
		 * from its original position by that much.
		 *
		 * This might be cleaner with a frame-by-frame delta but I
		 * worry about small float errors adding up.
		 */
		var delta = now.position - drag_start;
		reposition(pos_start + delta);
	}

	public void OnEndDrag(PointerEventData evtdata)
	{
		config.SetValue($"{CONFIG_PREFIX}_XPOS", transform.position.x);
		config.SetValue($"{CONFIG_PREFIX}_YPOS", transform.position.y);
		config.SetValue($"{CONFIG_PREFIX}_VLOCK", lock_vertical);
		config.Save(CONFIG_PATH);
	}

	public void Update()
	{
		/* NOTE: I do not like repositioning on every update, but
		 * haven't found a better method. Switching to the map or to IVA
		 * view tends to reset the position of the navball and
		 * altimeter. Forcing a reposition every update puts them back
		 * where we want them, at the cost of doing so far more often
		 * than we actually need to.
		 *
		 * A better solution would replace or disable whatever hooks
		 * reposition these elements on view changes, but I don't know
		 * how to do that yet.
		 */
		reposition(pos_last);
	}
}


class NavballDragger : Dragger
{
	public override string CONFIG_PREFIX { get; } = "NAVBALL";
	public static new GameObject target()
	{
		/* The SAS/navball/maneuver control cluster has type
		 * "IVAEVACollapseGroup", but I can't figure out where that's
		 * defined so I can't search for it. Taking the parent of the
		 * Navball works just as well.
		 *
		 * This doesn't include the (invisible) frame around the navball's
		 * control cluster, but that doesn't seem to hurt anything.
		 */
		GameObject navball = FindObjectOfType<NavBall>().gameObject;
		GameObject cluster = navball.transform.parent.gameObject;
		return cluster;
	}

	protected override void reposition(Vector2 pos)
	{
		/* Sometimes outside forces set the navball's
		 * position. They seem to use GameSettings.UI_POS_NAVBALL
		 * to get it, so let's override that. UI_POS_NAVBALL
		 * uses a -1:1 range, while we have a pixel offset
		 * from center, so a conversion is necessary.
		 */
		base.reposition(pos);
		GameSettings.UI_POS_NAVBALL = transform.position.x / Screen.right;
	}
}


class AltimeterDragger : Dragger
{
	public override string CONFIG_PREFIX { get; } = "ALTIMETER";
	public static new GameObject target()
	{
		/* The altimeter's control cluster is the grandparent of the
		 * altimeter object. Not sure of a more concise way to get a
		 * reference to it.
		 */
		GameObject altimeter = GameObject.Find("Altimeter");
		GameObject slideframeparent = altimeter.transform.parent.gameObject;
		GameObject cluster = slideframeparent.transform.parent.gameObject;
		return cluster;
	}
}
