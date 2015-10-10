public class TestButton : Button
{
	protected override void OnEnterTrigger(KinematicsTracker tracker)
	{
		UnityEngine.Debug.Log("Entered trigger");
	}
	protected override void OnExitTrigger(KinematicsTracker tracker)
	{
		UnityEngine.Debug.Log("Exited trigger");
	}
}