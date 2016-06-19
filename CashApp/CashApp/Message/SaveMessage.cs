using MvvmCross.Plugins.Messenger;

namespace CashApp.Message
{
    public class SaveMessage : MvxMessage
    {
        public SaveMessage(object sender, bool saved)
        : base(sender)
        {
            Saved = saved;
        }

        public bool Saved { get; private set; }
    }

}
