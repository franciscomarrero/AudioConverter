using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Threading;





namespace AudioConverter
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


       
        List<string> aux;
        int bitrate = 0;
        int freq = 0;
        //private readonly BackgroundWorker worker = new BackgroundWorker();

        private readonly BackgroundWorker conworker = new BackgroundWorker();
        private readonly BackgroundWorker conworker2 = new BackgroundWorker();

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();



        DateTime inici, inici2;
        Datos d, d2;

        string output;
        int cont;
        string tam, tam2;

        //Nullable<bool> siguiente = true;

        delegate void dvoid();
        Microsoft.Win32.OpenFileDialog abrir;
        

        public MainWindow()
        {
            InitializeComponent();

            /* worker.DoWork += worker_DoWork;
             worker.ProgressChanged += worker_ProgressChanged;
             worker.RunWorkerCompleted += worker_RunWorkerCompleted;
             worker.WorkerReportsProgress = true;*/

            conworker.DoWork += conworker_DoWork;
             conworker.RunWorkerCompleted += conworker_RunWorkerCompleted;
            conworker.ProgressChanged += conworker_ProgressChanged;
             conworker.WorkerReportsProgress = true;
          

             conworker2.DoWork += conworker2_DoWork;
             conworker2.RunWorkerCompleted += conworker2_RunWorkerCompleted;
             conworker2.ProgressChanged += new ProgressChangedEventHandler(conworker2_ProgressChanged);
             conworker2.WorkerReportsProgress = true;
        

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("Column1");
            dataGrid1.Columns.Add(col1);
            col1.Header = "Nombre";

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("Column2");
            dataGrid1.Columns.Add(col2);
            col2.Header = "Tamaño";

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("Column3");
            dataGrid1.Columns.Add(col3);
            col3.Header = "Formato";

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("Column4");
            dataGrid1.Columns.Add(col4);
            col4.Header = "Tiempo";

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("Column5");
            dataGrid1.Columns.Add(col5);
            col5.Header = "Propiedades";

          


            timer.Tick += new EventHandler(timer_Tick);
            timer2.Tick += new EventHandler(timer2_Tick);
        
        }

        void timer2_Tick(object sender, EventArgs e)
        {
            double mida = double.Parse(tam2);
            double percent = 0.0;
            TimeSpan t;

            try
            {

                if (timer2.IsEnabled == true)
                {

                    t = DateTime.UtcNow - inici2;
                    percent = (t.Seconds * 200.0) / (mida * 1024.0);
                    percent = percent * 100.0;

                }

                if (percent < 90) conworker2.ReportProgress((int)percent);
                else
                {
                    timer2.Stop();
                }
            }
            catch (Exception) { }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            double mida = double.Parse(tam);
            double percent = 0.0;
            TimeSpan t;
            try
            {
                if (timer.IsEnabled == true)
                {

                    t = DateTime.UtcNow - inici;
                    percent = (t.Seconds * 200.0) / (mida * 1024.0);
                    percent = percent * 100.0;

                }

                if (percent < 90) conworker.ReportProgress((int)percent);
                else
                {
                    timer.Stop();
                }
            }
            catch (Exception) { }
      
        }

     
        

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //props.Clear();
            abrir = new OpenFileDialog();
            abrir.Filter = "Ficheros Audio|*.mp3;*.wav;*.aac;*.ogg;*.flac";
            abrir.Multiselect = true;
            Nullable<bool> result = abrir.ShowDialog();
            if (result == false) return;
            //textBox1.Text = abrir.FileName;
           

            plenarDatagrid();
            
       
            
           

        }


        public List<string> extraerInfo(string ruta, string filename)
        {
            List<string> arrHeaders = new List<string>();
            List<string> props = new List<string>();

            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder objFolder;

            try
            {

                objFolder = shell.NameSpace(ruta);

                for (int i = 0; i < short.MaxValue; i++)
                {
                    string header = objFolder.GetDetailsOf(null, i);
                    if (String.IsNullOrEmpty(header))
                        break;
                    arrHeaders.Add(header);
                }


                //StringBuilder sb = new StringBuilder();
                //StreamWriter outfile = new StreamWriter(ruta + @"\AllTxtFiles.txt");
                foreach (Shell32.FolderItem2 item in objFolder.Items())
                {
                    
                    if (item.Path.Equals(filename))
                    {
                        
                        
                        props.Add(filename);
                        for (int i = 0; i < arrHeaders.Count; i++)
                        {
                            if (i == 1 || i == 2 || i == 27 || i == 28)
                            {
                                props.Add(objFolder.GetDetailsOf(item, i).ToString());
                                //sb.AppendLine((i.ToString() + " " + arrHeaders[i].ToString() + " " + objFolder.GetDetailsOf(item, i).ToString()));
                            }
                            
                        }
                        
                        //outfile.Write(sb.ToString());
                    }
                    
                    
                }
            }
            catch (Exception o)
            {
                MessageBox.Show(o.ToString());
            }

            return props;

        }

       

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            
            if (abrir == null || abrir.FileName == null) return;

            cont = 0;

            
            //string rutap = AppDomain.CurrentDomain.BaseDirectory.ToString();

            //MessageBox.Show(rutap);

            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if(result != System.Windows.Forms.DialogResult.OK) return;
            

            foreach (Datos col in dataGrid1.Items)
            {



                try
                {



                    //MessageBox.Show(aux.ToString());
                    if (aux == null) return;

                    bitrate = int.Parse(aux[1].ToString());
                    freq = int.Parse(aux[0].ToString());
                    string Params = null;

                    if (bitrate == 0) { return; }


                    string extension = aux[2].ToString();
                    string viejaex = System.IO.Path.GetExtension(col.Ruta.ToString());



                  
                   

                    string[] p = col.Column2.Split();
                    List<object> objects = new List<object>();
                    objects.Add(col);
                    objects.Add(p[0]);

                 
                    string archivo = col.Column1.Replace(viejaex, extension);
                    string guardar = System.IO.Path.Combine(dialog.SelectedPath, archivo);


                    /* DirectoryInfo info = new DirectoryInfo(rutap);
                     string info2 = info.Parent.Parent.FullName.ToString();
                     string rutaf = System.IO.Path.Combine(info2, "ffmpeg.exe");         
                     Converter c = new Converter(rutaf);*/

                    //string output = null;



                    //EL switch/case es el nostre amic :) pot facilitar el codic prou ^^
                    switch (extension)
                    {
                        case ".wav":
                        case ".mp3":
                        case ".aac":
                        case ".ogg":
                        case ".flac":
                            //  /C el que fa es mantindre la consola per a que pugues veure el que pasa. Li pots ficar en el argument "PAUSE"!
                            if (freq == 0)
                            {
                                Params = "-i \"" + col.Ruta + "\" -ab " + bitrate + "k \"" + guardar + "\"";
                            }
                            else
                            {
                                Params = "-i \"" + col.Ruta + "\" -ar " + freq + " -ab " + bitrate + "k \"" + guardar + "\"";
                            }//string Params = string.Format("-i {0} -ar {1} -ab {2}k \"{3}\"", abrir.FileName, freq, bitrate, ruta);


                            objects.Add(Params);
                            
                            if (conworker.IsBusy) conworker2.RunWorkerAsync(objects);
                            else conworker.RunWorkerAsync(objects);
                            //output = c.RunProcess(Params);

                            break;
                        default:
                            MessageBox.Show("Extensión no suportada");
                            this.Close();
                            break;


                    }

                }
                catch (Exception o)
                {
                    MessageBox.Show(o.ToString());
                }

            }
        
        }

       

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            Window1 win1 = new Window1();
            win1.formato(button5.Content.ToString());
            win1.ShowDialog();
            //MessageBox.Show("S'ha tancat el form2");
            aux = win1.devuelveDatos();
      
            if (aux.Capacity == 0) { return; }
            
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            Window1 win1 = new Window1();
            win1.formato(button6.Content.ToString());
            win1.ShowDialog();
            //MessageBox.Show("S'ha tancat el form2");
            aux = win1.devuelveDatos();
          
            if (aux.Capacity == 0) { return; }
          
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            Window1 win1 = new Window1();
            win1.formato(button7.Content.ToString());
            win1.ShowDialog();
            
            //MessageBox.Show("S'ha tancat el form2");
            aux = win1.devuelveDatos();
           
            if (aux.Capacity == 0) { return; }
           
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            Window1 win1 = new Window1();
            win1.formato(button8.Content.ToString());
            win1.ShowDialog();
  
            //MessageBox.Show("S'ha tancat el form2");
            aux = win1.devuelveDatos();
        
            if (aux.Capacity == 0) { return; }
           
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Window1 win1 = new Window1();
            win1.formato(button4.Content.ToString());
            win1.ShowDialog();

            //MessageBox.Show("S'ha tancat el form2");
            aux = win1.devuelveDatos();
           
            if (aux.Capacity == 0) { return; }
          
        }


        public void plenarDatagrid()
        {
            List<string> datos = null;

            if (abrir.FileNames != null)
            {
                foreach (String file in abrir.FileNames)
                {
                    try
                    {
                        datos=extraerInfo(System.IO.Path.GetDirectoryName(file.ToString()), file.ToString());
                        string props = propiedades(file);
                        if (datos[3] == "")
                        {
                            string rutap = AppDomain.CurrentDomain.BaseDirectory.ToString();
                            DirectoryInfo info = new DirectoryInfo(rutap);
                            string info2 = info.Parent.Parent.FullName.ToString();
                            string rutaf = System.IO.Path.Combine(info2, "ffmpeg.exe");
                            Converter c = new Converter(rutaf);
                            datos[3] = c.tiempo(file);
                            double aux = c.bitrate(file);
                            props += " " + aux.ToString() + "kb/s";
                        }
                        dataGrid1.Items.Add(new Datos() { Column1 = System.IO.Path.GetFileName(datos[0]), Column2 = datos[1], Column3 = datos[2], Column4 = datos[3], Column5 = props, Ruta=datos[0]});
                    }
                    catch (Exception o)
                    {
                        MessageBox.Show(o.ToString());
                    }


                }
            }
        
        }
        private void dataGrid1_dragEnter(Object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }


        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);



            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);


            foreach (string file in files)
            {

                List<string> datos = extraerInfo(System.IO.Path.GetDirectoryName(file.ToString()), file.ToString());
                string props = propiedades(file);
                if (datos[3] == "")
                {
                    string rutap = AppDomain.CurrentDomain.BaseDirectory.ToString();
                    DirectoryInfo info = new DirectoryInfo(rutap);
                    string info2 = info.Parent.Parent.FullName.ToString();
                    string rutaf = System.IO.Path.Combine(info2, "ffmpeg.exe");
                    Converter c = new Converter(rutaf);
                    datos[3] = c.tiempo(file);
                    double aux = c.bitrate(file);
                    props += " "+aux.ToString() + "kb/s";

                }
                dataGrid1.Items.Add(new Datos() { Column1 = System.IO.Path.GetFileName(datos[0]), Column2 = datos[1], Column3 = datos[2], Column4 = datos[3], Column5 = props, Ruta = datos[0] });
            }


        }

      


        public string propiedades(string filename)
        {
            string rutap = AppDomain.CurrentDomain.BaseDirectory.ToString();
            DirectoryInfo info = new DirectoryInfo(rutap);
            string info2 = info.Parent.Parent.FullName.ToString();
            string rutaf = System.IO.Path.Combine(info2, "ffmpeg.exe");
            Converter c = new Converter(rutaf);
            string aux = c.infoAudio(filename);
            string[]llista = aux.Split();

            switch(llista.Length)
            {
                case 7:
                    aux = llista[2] + llista[3] + " " + llista[4];
                    break;

                case 9:
                    aux = llista[2] + llista[3] + " " + llista[4] + " " + llista[6] + llista[7];
                    break;

                case 12:
                    aux = llista[5] + llista[6] + " " + llista[7] + " " + llista[9] + llista[10];
                    break;
            }
            //aux = llista[2] + llista[3] + " " + llista[4] + " " + llista[6] + llista[7];
            return aux;
        }

    

        private void conworker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> aux = e.Argument as List<object>;
            string Params = aux[2].ToString();
            d = (Datos)aux[0];
            tam = aux[1].ToString();
            string rutap = AppDomain.CurrentDomain.BaseDirectory.ToString();
            DirectoryInfo info = new DirectoryInfo(rutap);
            string info2 = info.Parent.Parent.FullName.ToString();
            string rutaf = System.IO.Path.Combine(info2, "ffmpeg.exe");
            Converter c = new Converter(rutaf);

            inici = DateTime.UtcNow;
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();

            output = c.RunProcess(Params);
            
        }

        private void conworker2_DoWork(object sender, DoWorkEventArgs e)
        {

            List<object> aux = e.Argument as List<object>;
            string Params = aux[2].ToString();
            d2 = (Datos)aux[0];
            tam2 = aux[1].ToString();
            string Params2 = e.Argument.ToString();
            string rutap = AppDomain.CurrentDomain.BaseDirectory.ToString();
            DirectoryInfo info = new DirectoryInfo(rutap);
            string info2 = info.Parent.Parent.FullName.ToString();
            string rutaf = System.IO.Path.Combine(info2, "ffmpeg.exe");
            Converter c = new Converter(rutaf);

            inici2 = DateTime.UtcNow;
            timer2.Interval = new TimeSpan(0, 0, 2);
            timer2.Start();

            output = c.RunProcess(Params2);

         

        }

        private void conworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // run all background tasks here
            d.ProgressValue = e.ProgressPercentage;
            dataGrid1.Items.Refresh();

        }

        void conworker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            d2.ProgressValue = e.ProgressPercentage;
            dataGrid1.Items.Refresh();
        }


        private void conworker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
            if (output != null)
            {
             
                    d.ProgressValue = 100;
                    dataGrid1.Items.Refresh();
                
               
            }
            else MessageBox.Show("Error: no se ha podido convertir el archivo");

            if (cont < dataGrid1.Items.Count-1) cont++;
            else
            {
                MessageBox.Show("Conversión Finalizada");
                dataGrid1.Items.Clear();
            }

            conworker.Dispose();
            
        }

        private void conworker2_RunWorkerCompleted(object sender,
                                          RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
            if (output != null)
            {
                for (int i = 1; i <= 100; i++)
                {
                    d2.ProgressValue = 100;
                    dataGrid1.Items.Refresh();
                }

            }
            else MessageBox.Show("Error: no se ha podido convertir el archivo");

            if (cont < dataGrid1.Items.Count-1) cont++;
            else
            {
                MessageBox.Show("Conversión Finalizada");
                dataGrid1.Items.Clear();
                
            }

          
            conworker2.Dispose();
            
        }

        

        
       
    }
}
