using AccessRandomizer.Manager;
using HutongGames.PlayMaker;

namespace AccessRandomizer.Fsm
{
    internal class KeyBooleanFsmCheck : FsmStateAction
    {
        private string keyName;
        private string trueEvent;
        private string falseEvent;

        public KeyBooleanFsmCheck(string _keyName, string _trueEvent, string _falseEvent) 
        {
            keyName = _keyName;
            trueEvent = _trueEvent;
            falseEvent = _falseEvent;
        }
        public override void OnEnter()
        {
            if (AccessManager.SaveSettings.GetVariable<bool>(keyName))
            {
                Fsm.Event(trueEvent);
            }
            else
            {
                Fsm.Event(falseEvent);
            }
            Finish();
        }
    }
}