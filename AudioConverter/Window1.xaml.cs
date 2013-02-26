using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AudioConverter
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        List<string> aux;

        public Window1()
        {
            InitializeComponent();
            aux = new List<string>();
            Llistes l = new Llistes();
            comboBox2.ItemsSource = l.devuelveLista2();
            comboBox3.ItemsSource = l.devuelveLista1();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Debes seleccionar las opciones de conversión");
                return;
            }
           
            if (comboBox2.SelectedItem.ToString().Equals("Original"))
            {
                aux.Add("0");
            }
            else
            {
                aux.Add(comboBox2.SelectedItem.ToString());
            }
            aux.Add(comboBox3.SelectedItem.ToString());
            aux.Add(label4.Content.ToString());
              
            Close();
        }


        public List<string> devuelveDatos()
        {
            return aux;
        }

        public void formato(string formato) 
        {
            label4.Content = "."+formato;
        }
    }
}
