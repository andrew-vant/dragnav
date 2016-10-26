using KSP.UI.Screens.Flight;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[KSPAddon(KSPAddon.Startup.Flight, false)]
public class NavBallAttacher : MonoBehaviour
{
	void Start()
	{
		Debug.Log("Starting draggable navball");
		NavBall ball = GameObject.FindObjectOfType<NavBall>();
		ball.gameObject.AddComponent<NavBallDrag>();
		Debug.Log("Done");
	}
}
public class NavBallDrag : MonoBehaviour, IBeginDragHandler
{
	public void OnBeginDrag(PointerEventData evtdata)
	{
		Debug.Log("Navball got begindrag");
		//this.ptrstart = evtdata.position.x
		//this.nbstart = this.ball.transform.position.x
	}
	public void OnMouseDrag()
	{
		Debug.Log("Navball was dragged");
		//this.ball.transform.
	}
}
