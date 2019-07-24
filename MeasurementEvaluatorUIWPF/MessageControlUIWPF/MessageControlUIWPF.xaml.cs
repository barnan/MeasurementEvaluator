using Frame.MessageHandler;
using Frame.PluginLoader;
using Interfaces.Misc;
using MeasurementEvaluatorUI.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace MeasurementEvaluatorUIWPF.MessageControlUI
{
    /// <summary>
    /// Interaction logic for MessageControlUIWPF.xaml
    /// </summary>
    public partial class MessageControlUIWPF : UserControlBase, IMessageControlUIWPF, INotifyPropertyChanged
    {
        private IUIMessageControl _messageControl;
        private readonly object obj = new object();
        private MessageControlParameters _parameters;


        private ObservableCollection<MessageToUI> _messages;
        public ObservableCollection<MessageToUI> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
            }
        }


        private MessageToUI _selectedMessage;
        public MessageToUI SelectedMessage
        {
            get { return _selectedMessage; }
            set
            {
                _selectedMessage = value;
                OnPropertyChanged();
            }
        }


        public MessageControlUIWPF(MessageControlParameters parameters)
        {
            _parameters = parameters;

            InitializeComponent();

            DataContext = this;

            _messages = new ObservableCollection<MessageToUI>();

            _messageControl = PluginLoader.MessageControll;
            _messageControl.MessageReceived += _messageControl_MessageReceived;

        }

        private void _messageControl_MessageReceived(object sender, EventArgs e)
        {
            if (!(e is MessageEventArg<string, MessageSeverityLevels> receivedData))
            {
                return;
            }

            Action act = delegate ()
            {
                Messages.Add(new MessageToUI { Time = DateTime.Now, MessageSeverityLevel = receivedData.Data2, MessageText = receivedData.Data1 });
                SelectedMessage = Messages[Messages.Count - 1];
            };
            _parameters.MainWindowDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, act);
        }



        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }


    public class MessageToUI
    {
        public DateTime Time { get; set; }

        public string MessageText { get; set; }

        public MessageSeverityLevels MessageSeverityLevel { get; set; }
    }

}
