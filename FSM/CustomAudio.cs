using HutongGames.PlayMaker;
using ItemChanger.Internal;

namespace AccessRandomizer.Fsm
{
    internal class CustomAudio : FsmStateAction
    {
        private string audioName;

        public CustomAudio(string _audioName) 
        {
            audioName = _audioName;
        }
        public override void OnEnter()
        {
            SoundManager soundManager = new(typeof(AccessRandomizer).Assembly, "AccessRandomizer.Resources.Sounds.");
            soundManager.PlayClipAtPoint(audioName, HeroController.instance.transform.localPosition);
            Finish();
        }
    }
}