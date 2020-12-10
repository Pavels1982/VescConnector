using System.Windows;
using System.Windows.Controls;

namespace VescConnector.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для MaterialNumericUpDown.xaml
    /// </summary>
    public partial class MaterialNumericUpDown : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
          "Value",
          typeof(double),
          typeof(MaterialNumericUpDown),
          new FrameworkPropertyMetadata(
              1.0,
              FrameworkPropertyMetadataOptions.AffectsMeasure |
              FrameworkPropertyMetadataOptions.AffectsRender,
              new PropertyChangedCallback(OnValueChanged),
              new CoerceValueCallback(CoerceValue)));
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
                "Minimum",
                typeof(double),
                typeof(MaterialNumericUpDown),
                new FrameworkPropertyMetadata(
                    double.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(OnMinimumChanged),
                    new CoerceValueCallback(CoerceMinimum)));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
               "Title",
               typeof(string),
               typeof(MaterialNumericUpDown),
               new FrameworkPropertyMetadata(
               string.Empty));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
                "Maximum",
                typeof(double),
                typeof(MaterialNumericUpDown),
                new FrameworkPropertyMetadata(
                    double.MaxValue,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(OnMaximumChanged),
                    new CoerceValueCallback(CoerceMaximum)));

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
                "Increment",
                typeof(double),
                typeof(MaterialNumericUpDown),
                new FrameworkPropertyMetadata(
                    1.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(OnIncrementChanged),
                    new CoerceValueCallback(CoerceIncrement)));


        public MaterialNumericUpDown()
        {
            InitializeComponent();
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        private static object CoerceValue(DependencyObject d, object value)
        {
            MaterialNumericUpDown numeric = (MaterialNumericUpDown)d;
            if (value == null)
            {
                return numeric.Value;
            }
            if ((double)value > numeric.Maximum)
            {
                return numeric.Maximum;
            }
            if ((double)value < numeric.Minimum)
            {
                return numeric.Minimum;
            }
            return value;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialNumericUpDown numeric = (MaterialNumericUpDown)d;
            numeric.Value = (double)e.NewValue;
        }

        private static object CoerceMinimum(DependencyObject d, object value)
        {
            MaterialNumericUpDown numeric = (MaterialNumericUpDown)d;
            if (value == null)
            {
                return numeric.Minimum;
            }
            if ((double)value > numeric.Maximum)
            {
                numeric.Maximum = double.MaxValue;
            }
            return value;
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialNumericUpDown numeric = (MaterialNumericUpDown)d;
            numeric.Minimum = (double)e.NewValue;
        }

        private static object CoerceMaximum(DependencyObject d, object value)
        {
            MaterialNumericUpDown numeric = (MaterialNumericUpDown)d;
            if (value == null)
            {
                return numeric.Maximum;
            }
            if ((double)value < numeric.Minimum)
            {
                numeric.Minimum = double.MinValue;
            }
            return value;
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialNumericUpDown numeric = (MaterialNumericUpDown)d;
            numeric.Maximum = (double)e.NewValue;
        }

        private static object CoerceIncrement(DependencyObject d, object value)
        {
            MaterialNumericUpDown numeric = (MaterialNumericUpDown)d;
            if (value == null)
            {
                return numeric.Increment;
            }
            return value;
        }

        private static void OnIncrementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialNumericUpDown numeric = (MaterialNumericUpDown)d;
            numeric.Increment = (double)e.NewValue;

        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Value - this.Increment >= this.Minimum)
            {
                this.Value -= this.Increment;
            }
            else
            {
                this.Value = this.Minimum;
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Value + this.Increment <= this.Maximum)
            {
                this.Value += this.Increment;
            }
            else
            {
                this.Value = this.Maximum;
            }
        }


    }
}