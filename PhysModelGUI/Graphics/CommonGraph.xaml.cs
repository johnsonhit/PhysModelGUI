using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Controls.Legend;

namespace PhysModelGUI.Graphics
{
    /// <summary>
    /// Interaction logic for CommonGraph.xaml
    /// </summary>
    public partial class CommonGraph : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public double[] dataSource;

        public List<ChartDataClass> data = new List<ChartDataClass>();

        public List<ChartDataClass> AsyncData1 { get; set; } = new List<ChartDataClass>();

        private ChartDataClass currentData1 = new ChartDataClass();


        private SolidColorBrush series1Color = new SolidColorBrush(Colors.DarkGreen);
        public SolidColorBrush Series1Color
        {
            get { return series1Color; }
            set { series1Color = value; OnPropertyChanged(); }
        }

     

        private string series1Legend = "heartrate";
        public string Series1Legend
        {
            get { return series1Legend; }
            set { series1Legend = value; OnPropertyChanged(); }
        }

      

        private int series1StrokeThickness = 1;
        public int Series1StrokeThickness
        {
            get { return series1StrokeThickness; }
            set { series1StrokeThickness = value; OnPropertyChanged(); }
        }

        private double _maxY = 250;
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

        private double _gridYStep = 10;
        public double GridYStep
        {
            get => _gridYStep;
            set { _gridYStep = value; OnPropertyChanged(); }
        }

        private bool _showYLabels = true;
        public bool ShowYLabels
        {
            get => _showYLabels;
            set { _showYLabels = value; OnPropertyChanged(); }
        }


        private double _minX = 0;
        public double MinX
        {
            get => _minX;
            set { _minX = value; OnPropertyChanged(); }
        }

        private double _maxX = 300;
        public double MaxX
        {
            get => _maxX;
            set { _maxX = value; OnPropertyChanged(); }
        }
        private double _gridXStep = 10;
        public double GridXStep
        {
            get => _gridXStep;
            set { _gridXStep = value; OnPropertyChanged(); }
        }
        private bool _showXLabels = false;
        public bool ShowXLabels
        {
            get => _showXLabels;
            set { _showXLabels = value; OnPropertyChanged(); }
        }

        public int BufferSize { get; set; } = 200;           // determine the buffer size off 200 datapoints

        public CommonGraph()
        {
            InitializeComponent();

            DataContext = this;

            dataSeries1.XValueBinding = new PropertyNameDataPointBinding() { PropertyName = "XValue" };
            dataSeries1.YValueBinding = new PropertyNameDataPointBinding() { PropertyName = "YValue" };
            dataSeries1.ItemsSource = AsyncData1;

        }

        public void InitGraph(double max_x, int buffer)
        {
            MaxX = max_x;
            BufferSize = buffer;
        }
        public void DrawGraph()
        {
            AsyncData1 = new List<ChartDataClass>(data);
            dataSeries1.ItemsSource = AsyncData1;

        }
        public void UpdateData(double data1, double data2)
        {
            ChartDataClass newC = new ChartDataClass()
            {
                XValue = data1,
                YValue = data2,
            };

            data.Add(newC);
            if (data.Count > BufferSize)
            {
                data.RemoveAt(0);
            }

        }


        public class ChartDataClass
        {
            public DateTime TimeValue { get; set; }
            public double XValue { get; set; }
            public double YValue { get; set; }
            public bool Enabled { get; set; } = true;
        }

        private void LinearAxis_AccessKeyPressed(object sender, System.Windows.Input.AccessKeyPressedEventArgs e)
        {

        }
    }
}
