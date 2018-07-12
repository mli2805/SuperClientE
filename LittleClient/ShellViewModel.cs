using Caliburn.Micro;

namespace LittleClient 
{
    public class ShellViewModel : Screen, IShell
    {
        private string _textLine;
        public string TextLine
        {
            get { return _textLine;}
            set
            {
                if (value == _textLine) return;
                _textLine = value;
                NotifyOfPropertyChange();
            }
        }

        public ShellViewModel()
        {
            TextLine = "initial";
        }

    }
}