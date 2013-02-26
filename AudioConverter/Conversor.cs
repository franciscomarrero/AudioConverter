using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace AudioConverter
{

    public class Converter
    {
        
        private string _ffExe;
        //private string output;

        public string ffExe
        {
            get
            {
                return _ffExe;
            }
            set
            {
                _ffExe = value;
            }
        }

        private string _WorkingPath;

        public string WorkingPath
        {
            get
            {
                return _WorkingPath;
            }
            set
            {
                _WorkingPath = value;
            }
        }

    

     
        public Converter()
        {
            Initialize();
        }

        public Converter(string ffmpegExePath)
        {
            _ffExe = ffmpegExePath;
            Initialize();

        }
       

        
        private void Initialize()
        {
            //worker.DoWork += worker_DoWork;
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
           
            //Now see if ffmpeg.exe exists
            string workingpath = GetWorkingFile();
            if (string.IsNullOrEmpty(workingpath))
            {
                //ffmpeg doesn't exist at the location stated.
                throw new Exception("No se encuentra la copia de ffmpeg.exe");
            }
            _ffExe = workingpath;

        }

        private string GetWorkingFile()
        {
            //try the stated directory
            if (File.Exists(_ffExe))
            {
                return _ffExe;
            }

            //oops, that didn't work, try the base directory
            if (File.Exists(Path.GetFileName(_ffExe)))
            {
                return Path.GetFileName(_ffExe);
            }

            //well, now we are really unlucky, let's just return null
            return null;
        }
       

        public string RunProcess(string Parameters)
        {
            
            //string Params = string.Format("-i {0} {1} -vcodec mjpeg -ss {2} -vframes 1 -an -f rawvideo", input.Path, finalpath, secs);
            int result = 0;
            //create a process info
            ProcessStartInfo oInfo = new ProcessStartInfo(this._ffExe, Parameters);
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            //oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

            //Create the output and streamreader to get the output
            string output = null; StreamReader srOutput = null;

            //try the process
            try
            {


                //run the process
                Process proc = System.Diagnostics.Process.Start(oInfo);

                //get the output
                srOutput = proc.StandardError;

                //now put it in a string
                output = srOutput.ReadToEnd();
                result = proc.ExitCode;

                proc.WaitForExit();

                
                proc.Close();
            }
            catch (Exception)
            {
                output = string.Empty;
            }
            finally
            {
                //now, if we succeded, close out the streamreader
                if (srOutput != null)
                {
                    srOutput.Close();
                    srOutput.Dispose();
                }
            }

            return output;
        }

      public string infoAudio(string filename)
        {
            //set up the parameters for video info
            //string Params = string.Format("-i {0}", filename);
            string Params = "-i \"" + filename + "\"";
            string output = RunProcess(Params); 

            //get the audio format
            Regex re = new Regex("[A|a]udio:.*");
            Match m = re.Match(output);
            string fm = null; ;
            if (m.Success)
            {
                fm = m.Value;
            }
            

            return fm;
        }

      public string tiempo(string filename)
      {
          //set up the parameters for video info
          //string Params = string.Format("-i {0}", filename);
          string Params = "-i \"" + filename + "\"";
          string output = RunProcess(Params);
          string tiempo = null;

          //get duration
          Regex re = new Regex("[D|d]uration:.((\\d|:|\\.)*)");
          Match m = re.Match(output);

          if (m.Success)
          {
              string duration = m.Groups[1].Value;
              string[] timepieces = duration.Split(new char[] { ':', '.' });
              
              if (timepieces.Length == 4)
              {
                  TimeSpan tm = new TimeSpan(0, Convert.ToInt16(timepieces[0]), Convert.ToInt16(timepieces[1]), Convert.ToInt16(timepieces[2]));
                  tiempo = tm.ToString();
              }
              
          }

          return tiempo;
      }

      public double bitrate(string filename)
      {
          //set up the parameters for video info
          //string Params = string.Format("-i {0}", filename);
          string Params = "-i \"" + filename + "\"";
          string output = RunProcess(Params);
          double kb;

          //get audio bit rate
            Regex re = new Regex("[B|b]itrate:.((\\d|:)*)");
            Match m = re.Match(output);
            kb = 0.0;
            if (m.Success)
            {
                Double.TryParse(m.Groups[1].Value, out kb);
            }

            return kb;
      }

    
         
       
    }


   
}

