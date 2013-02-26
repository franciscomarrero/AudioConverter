using System;
using System.Collections.Generic;
using System.Text;

namespace AudioConverter
{
    class Llistes
    {
        List<string> bitrate = new List<string>();
        List<string> freq = new List<string>();

        public Llistes() { 
        bitrate.Add("192");
        bitrate.Add("160");
        bitrate.Add("128");
        bitrate.Add("127");
        bitrate.Add("96");
        bitrate.Add("95");
        bitrate.Add("64");
        bitrate.Add("63");

        freq.Add("Original");
        freq.Add("48000");
        freq.Add("44100");
        freq.Add("32000");
        freq.Add("22050");
        freq.Add("8000");

        
        }

        public List<string> devuelveLista1()
        {
            return bitrate;
        }

        public List<string> devuelveLista2()
        {
            return freq;
        }
    }
}
