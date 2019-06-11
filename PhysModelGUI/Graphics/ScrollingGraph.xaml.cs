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

        public int Interval { get; set; } = 10;
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
        DateTime bufferDate = DateTime.Now;

        private bool _showXLabels = true;
        public bool ShowXLabels
        {
            get => _showXLabels;
            set { _showXLabels = value; OnPropertyChanged();}
        }

        private bool firstRunFlag = true;
        private bool bufferFlag = true;

        public ScrollingGraph()
        {
            InitializeComponent();

            DataContext = this;

            MinDate = DateTime.Now;
            MaxDate = MinDate.AddSeconds(Interval);
            startDate = DateTime.Now;
            bufferDate = DateTime.Now;
            firstRunFlag = true;
            bufferFlag = true;
            dataSeries1.ItemsSource = AsyncData1;
        }

        public void UpdateData(double data1, double data2, double data3 = 0, double data4 = 0, double data5 = 0)
        {
            if (firstRunFlag)
            {
                firstRunFlag = false;
                MinDate = DateTime.Now;
                MaxDate = MinDate.AddSeconds(Interval);
                startDate = DateTime.Now;
            }


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

            if ((DateTime.Now - startDate).TotalSeconds > Interval)
            {
                MaxDate = DateTime.Now;
                MinDate = MaxDate.AddSeconds(-Interval);

            }

            if ((DateTime.Now - bufferDate).TotalSeconds > Interval * 2)
            {
                bufferFlag = false;
            }

            if (!bufferFlag)
            {
               AsyncData1.RemoveAt(0);
            }
        }

        public class ChartDataClass
        {
            public DateTime TimeValue { get; set; }
            public double YValue { get; set; }
            public bool Enabled { get; set; } = true;
        }

    }
}
