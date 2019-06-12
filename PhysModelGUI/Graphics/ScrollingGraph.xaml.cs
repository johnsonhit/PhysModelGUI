using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Telerik.Windows.Controls.Legend;

namespace PhysModelGUI.Graphics
{
    /// <summary>
    /// Interaction logic for ScrollingGraph.xaml
    /// </summary>
    public partial class ScrollingGraph : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

  
        public ObservableCollection<ChartDataClass> AsyncData1 { get; set; } = new ObservableCollection<ChartDataClass>();
        public ObservableCollection<ChartDataClass> AsyncData2 { get; set; } = new ObservableCollection<ChartDataClass>();
        public ObservableCollection<ChartDataClass> AsyncData3 { get; set; } = new ObservableCollection<ChartDataClass>();
        public ObservableCollection<ChartDataClass> AsyncData4 { get; set; } = new ObservableCollection<ChartDataClass>();
        public ObservableCollection<ChartDataClass> AsyncData5 { get; set; } = new ObservableCollection<ChartDataClass>();

        public ChartDataClass currentData1 = new ChartDataClass();
        public ChartDataClass currentData2 = new ChartDataClass();
        public ChartDataClass currentData3 = new ChartDataClass();
        public ChartDataClass currentData4 = new ChartDataClass();
        public ChartDataClass currentData5 = new ChartDataClass();

       
        public int MaxItemsCount { get; set; } = 250;

        private double _maxY = 100;
        public double MaxY
        {
            get => _maxY;
            set { _maxY = value; OnPropertyChanged(); }
        }

        private double _minY = 0;
        public double MinY
        {
            get => _minY;
            set { _minY = value; OnPropertyChanged(); }
        }

        private double _gridStep = 2;
        public double GridStep
        {
            get => _gridStep;
            set { _gridStep = value; OnPropertyChanged(); }
        }


        private DateTime _minDate = DateTime.Now;
        public DateTime MinDate
        {
            get => _minDate;
            set { _minDate = value; OnPropertyChanged(); }
        }

        private DateTime _maxDate = DateTime.Now;
        public DateTime MaxDate
        {
            get => _maxDate;
            set { _maxDate = value; OnPropertyChanged(); }
        }

        DateTime startDate = DateTime.Now;
        public int GraphDuration { get; set; } = 10;            // how many seconds are displayed in seconds
        public int BufferSize { get; set; } = 20;           // determine the buffer size in seconds

        private bool _showXLabels = true;
        public bool ShowXLabels
        {
            get => _showXLabels;
            set { _showXLabels = value; OnPropertyChanged();}
        }

        private bool firstRunFlag = true;
        private double runDuration = 0;

        public ScrollingGraph()
        {
            InitializeComponent();

            DataContext = this;

            InitGraph(5, 10);
            
        }

        public void InitGraph(int duration, int buffer)
        {
            GraphDuration = duration;
            BufferSize = buffer;

            startDate = DateTime.Now;
            MinDate = DateTime.Now;
            MaxDate = MinDate.AddSeconds(GraphDuration);
        
            runDuration = 0;
            firstRunFlag = true;

            // reset the datasources
            AsyncData1 = new ObservableCollection<ChartDataClass>();
            
            // attach to the graph
            dataSeries1.ItemsSource = AsyncData1;

            // reset the datasources
            AsyncData2 = new ObservableCollection<ChartDataClass>();

            // attach to the graph
            dataSeries2.ItemsSource = AsyncData2;

            // reset the datasources
            AsyncData3 = new ObservableCollection<ChartDataClass>();

            // attach to the graph
            dataSeries3.ItemsSource = AsyncData3;

        }

        public void UpdateData(double data1, double data2, double data3 = 0, double data4 = 0, double data5 = 0)
        {
            // when the graph starts we need to set the start conditions
            if (firstRunFlag)
            {
                firstRunFlag = false;
                runDuration = 0;
                startDate = DateTime.Now;
                MinDate = DateTime.Now;
                MaxDate = MinDate.AddSeconds(GraphDuration);
                
            }

            // calculatet the duration of the graph run
            runDuration = (DateTime.Now- startDate).TotalSeconds;

            // update the data
            if (currentData1.Enabled)
            {
                currentData1.TimeValue = DateTime.Now;
                currentData1.YValue = data1;
                lock (((ICollection)AsyncData1).SyncRoot)
                {
                    AsyncData1.Add(currentData1);
                }
               
            }

            // update the data
            if (currentData2.Enabled)
            {
                currentData2.TimeValue = DateTime.Now;
                currentData2.YValue = data2;
                lock (((ICollection)AsyncData2).SyncRoot)
                {
                    AsyncData2.Add(currentData2);
                }

            }

            // update the data
            if (currentData3.Enabled)
            {
                currentData3.TimeValue = DateTime.Now;
                currentData3.YValue = data3;
                lock (((ICollection)AsyncData3).SyncRoot)
                {
                    AsyncData3.Add(currentData3);
                }

            }



            // if the graph duration is longer than the interval we need to shift the range which is displayed
            if (runDuration > GraphDuration)
            {
                MaxDate = DateTime.Now;
                MinDate = MaxDate.AddSeconds(-GraphDuration);

            }

            // if the buffer is exceed start removing entries so safe memory
            if (runDuration > BufferSize)
            {
                if (AsyncData1.Count > 0) AsyncData1.RemoveAt(0);
                if (AsyncData2.Count > 0) AsyncData2.RemoveAt(0);
                if (AsyncData3.Count > 0) AsyncData3.RemoveAt(0);
            }

        }

        public class ChartDataClass
        {
            public DateTime TimeValue { get; set; }
            public double XValue { get; set; }
            public double YValue { get; set; }
            public bool Enabled { get; set; } = true;
        }

    }
}
