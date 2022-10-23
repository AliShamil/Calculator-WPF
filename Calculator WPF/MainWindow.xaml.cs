using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator_WPF
{
    public partial class MainWindow : Window
    {
        private double _result;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Calculate()
        {
            string formattedCalculation = txt.Text.Replace("×", "*").ToString().Replace("÷", "/").ToString().Replace(",", ".");
            try
            {
                _result = double.Parse(new DataTable().Compute(formattedCalculation, null).ToString()!);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                _result = 0;
            }
        }

        private Button? findBtn(UIElementCollection children, string equal)
        {
            foreach (var child in children)
            {
                if (child is Button btn && btn.Content.ToString() == equal)
                    return btn;
            }
            return null;
        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key.ToString().StartsWith("D"))
                ButtonNum_Click(findBtn(grid_num.Children, e.Key.ToString().Substring(1, 1))!, e);




            else if (e.Key.ToString().StartsWith("NumPad"))
                ButtonNum_Click(findBtn(grid_num.Children, e.Key.ToString().Substring(6, 1))!, e);


            switch (e.Key)
            {
                case Key.Back:
                    ButtonDel_Click(sender, e);
                    break;
                case Key.OemPeriod:
                    ButtonCalcSymb_Click(findBtn(grid_num.Children, ".")!, e);
                    break;
                case Key.OemMinus:
                    ButtonCalcSymb_Click(findBtn(grid_num.Children, "-")!, e);
                    break;
                case Key.OemPlus:
                    ButtonCalcSymb_Click(findBtn(grid_num.Children, "+")!, e);
                    break;
                case Key.Divide:
                    ButtonCalcSymb_Click(findBtn(grid_num.Children, "÷")!, e);
                    break;
                case Key.Multiply:
                    ButtonCalcSymb_Click(findBtn(grid_num.Children, "×")!, e);
                    break;
                case Key.Enter:
                    ButtonEqual_Click(sender, e);
                    break;
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            txt.Text = txt.Text.Remove(txt.Text.Length - 1);

            if (string.IsNullOrEmpty(txt.Text))
            {
                txt.Text = "0";
                return;
            }
        }

        private void ButtonNum_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (txt.Text == "0")
                    txt.Text = string.Empty;

                txt.Text += btn.Content.ToString();
            }
        }

        private void ButtonCalcSymb_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt.Text))
                return;

            if (sender is Button btn)
            {
                if (btn.Content.ToString() == "CE" || btn.Content.ToString() == "C")
                {
                    txt.Text = "0";
                    _result = 0;
                    return;
                }

                if (char.IsDigit(txt.Text[txt.Text.Length - 1]) || txt.Text[txt.Text.Length - 1] == '.' && btn.Content.ToString() != "." || txt.Text[txt.Text.Length - 1] == ',' && btn.Content.ToString() != ".")
                    txt.Text += btn.Content.ToString();
            }
        }

        private void ButtonEqual_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt.Text))
                return;

            if (!char.IsDigit(txt.Text[txt.Text.Length - 1]) && txt.Text[txt.Text.Length - 1] != '.')
                return;

            Calculate();
            txt.Text = _result.ToString();
        }

        private void ButtonComplex_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt.Text))
                return;

            if (sender is Button btn)
            {
                if (txt.Text.Contains('+') || txt.Text.Contains('-') || txt.Text.Contains('×') || txt.Text.Contains('÷'))
                    Calculate();

                if (_result == 0)
                    if (!double.TryParse(txt.Text, out _result))
                        return;

                switch (btn.Content.ToString())
                {
                    case "%":
                        _result /= 100;
                        break;
                    case "x":
                        _result = Math.Pow(_result, 2);
                        break;
                    case "√x":
                        _result =  Math.Sqrt(_result);
                        break;
                    case "1/x":
                        _result =  1 / _result;
                        break;
                    case "±":
                        _result*= -1;

                        break;
                }


                if (_result.ToString() == "∞")
                {
                    MessageBox.Show("Cannot Divide by zero !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txt.Text = "0";
                    return;
                }

                txt.Text = _result.ToString();
            }
        }


    }
}
