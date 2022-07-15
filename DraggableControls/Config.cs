using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using IOUtils = KSP.IO.IOUtils;

namespace DraggableControls
{
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	class Config : MonoBehaviour
	{
		internal static float NAVBALL_XCOORD;
		internal static float NAVBALL_YCOORD;
		internal static float ALTIMETER_XCOORD;

		static public Transform navBallTransform { get; set; }

		static public Transform altimeterTransform { get; set; }


		const string PATH = "/GameData/DraggableControls/PluginData/";
		const string cfgfile = "DraggableControls.cfg";

		static string fileName;

		public void Start()
		{
			fileName = KSPUtil.ApplicationRootPath + PATH + cfgfile;
			ConfigNode config = ConfigNode.Load(fileName);

			NAVBALL_XCOORD = float.Parse(config.GetValue("NAVBALL_XCOORD"));
			NAVBALL_YCOORD = float.Parse(config.GetValue("NAVBALL_YCOORD"));
			ALTIMETER_XCOORD = float.Parse(config.GetValue("ALTIMETER_XCOORD"));
		}

		public static void Save()
		{
			ConfigNode config = new ConfigNode();
			config.AddValue("NAVBALL_XCOORD", NAVBALL_XCOORD);
			config.AddValue("NAVBALL_YCOORD", NAVBALL_YCOORD);
			config.AddValue("ALTIMETER_XCOORD", ALTIMETER_XCOORD);
			config.Save(fileName);
		}

		public static float navBallXpos
		{
			/* Assigning to xpos moves the navball. Don't do it any other
			 * way; stuff necessary for consistency happens here.
			 */
			get { return navBallTransform.position.x; }
			set
			{
				// Prevent the ball from ending up offscreen where we
				// can't drag it back.
				int right_edge = GameSettings.SCREEN_RESOLUTION_WIDTH / 2;
				int left_edge = -right_edge;
				if (value > right_edge) value = right_edge;
				if (value < left_edge) value = left_edge;

				// Move the ball.
				Vector3 newpos = navBallTransform.position;
				newpos.x = value;
				NAVBALL_XCOORD = value;
				navBallTransform.position = newpos;

				/* Sometimes outside forces set the navball's
				 * position. They seem to use GameSettings.UI_POS_NAVBALL
				 * to get it, so let's override that. UI_POS_NAVBALL
				 * uses a -1:1 range, while we have a pixel offset
				 * from center, so a conversion is necessary.
				 */
				GameSettings.UI_POS_NAVBALL = value / right_edge;
			}
		}

		public const float VERTICAL_ADJUST_BOTTOM = 14f;
		public const float VERT_ADJUST_TOP = 340;
		public static float navBallYpos
		{
			/* Assigning to ypos moves the navball. Don't do it any other
			 * way; stuff necessary for consistency happens here.
			 */
			get { return navBallTransform.position.y; }
			set
			{
				// Prevent the ball from ending up offscreen where we
				// can't drag it back.
				int top_edge = GameSettings.SCREEN_RESOLUTION_HEIGHT / 2;
				int bottom_edge = -top_edge;
				if (value > top_edge-VERT_ADJUST_TOP * GameSettings.UI_SCALE_NAVBALL) value = top_edge - VERT_ADJUST_TOP * GameSettings.UI_SCALE_NAVBALL;
				if (value < bottom_edge - VERTICAL_ADJUST_BOTTOM* GameSettings.UI_SCALE_NAVBALL) value = bottom_edge - VERTICAL_ADJUST_BOTTOM* GameSettings.UI_SCALE_NAVBALL;

				// Move the ball.
				Vector3 newpos = navBallTransform.position;
				newpos.y = value;
				NAVBALL_YCOORD = value;
				navBallTransform.position = newpos;

				/* Sometimes outside forces set the navball's
				 * position. They seem to use GameSettings.UI_POS_NAVBALL
				 * to get it, so let's override that. UI_POS_NAVBALL
				 * uses a -1:1 range, while we have a pixel offset
				 * from center, so a conversion is necessary.
				 */
				GameSettings.UI_POS_NAVBALL = value / top_edge;
			}
		}

		public static float altimeterXpos
		{
			/* Assigning to xpos moves the altimeter. Don't do it any other
			 * way; stuff necessary for consistency happens here.
			 */
			get { return altimeterTransform.position.x; }
			set
			{
				// Prevent the altimeter from ending up offscreen where
				// we can't drag it back.
				int right_edge = GameSettings.SCREEN_RESOLUTION_WIDTH / 2;
				int left_edge = -right_edge;
				if (value > right_edge) value = right_edge;
				if (value < left_edge) value = left_edge;

				// Move the altimeter.
				Vector3 newpos = altimeterTransform.position;
				newpos.x = value;
				ALTIMETER_XCOORD = value;
				altimeterTransform.position = newpos;
			}
		}
	}
}
