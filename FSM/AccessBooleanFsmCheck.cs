using AccessRandomizer.Modules;
using HutongGames.PlayMaker;

namespace AccessRandomizer.Fsm
{
    internal class AccessBooleanFsmCheck : FsmStateAction
    {
        private string keyName;
        private string trueEvent;
        private string falseEvent;

        public AccessBooleanFsmCheck(string _keyName, string _trueEvent, string _falseEvent) 
        {
            keyName = _keyName;
            trueEvent = _trueEvent;
            falseEvent = _falseEvent;
        }
        public override void OnEnter()
        {
            if (AccessModule.Instance.GetVariable<bool>(keyName))
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