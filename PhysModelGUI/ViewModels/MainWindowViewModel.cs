using PhysModelLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PhysModelGUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        List<string> _modelLog = new List<string>();
        List<string> ModelLog { get { return _modelLog; } set { _modelLog = value; OnPropertyChanged(); } }

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => MyAction(), true));
                //return _clickCommand ?? (_clickCommand = new RoutedCommand(() => MyAction(), null));
            }
        }
        public void MyAction()
        {

        }

        public MainWindowViewModel()
        {
            
            // get the physiological model initialized and setup a routine to get the data from it
            PhysModelMain.Initialize();
            PhysModelMain.modelInterface.PropertyChanged += ModelInterface_PropertyChanged; ;
            PhysModelMain.Start();

        }



        private void ModelInterface_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ModelUpdated":
                    //UpdatePVLoops();
                    //UpdatePressureGraph();
                    //UpdateFlowGraph();
                    //UpdateMonitorGraph();

                    break;
                case "StatusMessage":
                    // add the status message to the model log
                    ModelLog.Add(PhysModelMain.modelInterface.StatusMessage);
                    break;
            }
        }

        public void ClearModelLog()
        {
            ModelLog.Clear();
        }

        public class CommandHandler : ICommand
        {
            private Action _action;
            private bool _canExecute;
            public CommandHandler(Action action, bool canExecute)
            {
                _action = action;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _action();
            }
        }

    }
}
