using System;

using System.Windows.Forms;

namespace Stereo_Mono_ManagedBass
{
    public partial class Form1 : Form
    {

        //public int _stream; // Original stereo stream
        //public int _mixer;  // Mixer stream to downmix to mono
        public string _filePath;
        myStereoToMono _myStereoToMono;

        public Form1()
        {
            InitializeComponent();            
            _myStereoToMono = new myStereoToMono();
        }


        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;
            openFile.Title = "Open";
            openFile.Filter = "Musicas |*.mp3;*.wma";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                _filePath= openFile.FileName;                
            }
        }


        private void buttonPlay_Click(object sender, EventArgs e)
        {

            checkBoxStereo.Checked = true;
            _myStereoToMono.CreateStream(_filePath);
            _myStereoToMono.PlayMono(false, true);
            

        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            _myStereoToMono.StopStream();
        }
        //==============================================================
        private void checkBoxLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLeft.Checked)
            {
                // Play left channel
                //_myStereoToMono.PlayMono(_filePath, true); 
                _myStereoToMono.PlayMono(true);
                checkBoxRight.Checked = false;
                checkBoxStereo.Checked = false;

            }
        }

        private void checkBoxRight_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxRight.Checked)
            {
                //_myStereoToMono.PlayMono(_filePath, false);
                // Play Right Channel
                _myStereoToMono.PlayMono(false);
                checkBoxLeft.Checked = false;
                checkBoxStereo.Checked = false;
            }
        }

        private void checkBoxStereo_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStereo.Checked)
            {            

                checkBoxRight.Checked=false;
                checkBoxLeft.Checked=false;
                checkBoxStereo.Checked = true;
                // Play Stereo File
                //_myStereoToMono.PlayMono(_filePath,false,true);
                _myStereoToMono.PlayMono(false,true);

            }
        }

    }
}
