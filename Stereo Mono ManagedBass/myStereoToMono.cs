
using System.Windows.Forms;
using ManagedBass;
using ManagedBass.Fx;
using ManagedBass.Mix;

namespace Stereo_Mono_ManagedBass
{
    internal class myStereoToMono
    {
        public int _stream; // Original stereo stream
        public int _mixer;  // Mixer stream to downmix to mono        

        public myStereoToMono()
        {
            // Inicialize Bass library
            Bass.Init();
        }
            
        public void CreateStream(string filePath)
        {
            //_stream = Bass.CreateStream(filePath, 0, 0, BassFlags.Default);
            _stream = Bass.CreateStream(filePath, 0, 0, BassFlags.Decode);            
        }

        public void PlayStream()
        {
            Bass.ChannelPlay(_stream);
        }

        public void StopStream() { 
            //Bass.ChannelStop(_stream);
            Bass.ChannelStop(_mixer);
        }

        public void PlayMono(bool useLeftChannel, bool stereo = false)
        {
            // Ensure the stream is already created
            if (_stream == 0)
            {
                MessageBox.Show("Load the music file first.");
                return;
            }

            // Free any previous mixer
            if (_mixer != 0)
            {
                Bass.StreamFree(_mixer);
            }

            // Create a mono mixer to handle channel output
            _mixer = BassMix.CreateMixerStream(44100, 2, BassFlags.Default); // Create a stereo mixer

            if (_mixer == 0)
            {
                // Handle error if mixer creation fails
                MessageBox.Show("Error: " + Bass.LastError);
                return;
            }

            if (stereo)
            {
                // Add stereo stream to mixer (default behavior)
                BassMix.MixerAddChannel(_mixer, _stream, BassFlags.Default);
            }
            else
            {
                // Split the required channel (left or right)
                int[] splitterChannel = useLeftChannel ? new int[] { 0, -1 } : new int[] { 1, -1 }; // Left: 0, Right: 1
                var splitter = BassMix.CreateSplitStream(_stream, BassFlags.Decode, splitterChannel);

                if (splitter == 0)
                {
                    // Handle error if splitter creation fails
                    MessageBox.Show("Error: " + Bass.LastError);
                    return;
                }

                // Add the selected channel to both sides of the mixer
                BassMix.MixerAddChannel(_mixer, splitter, BassFlags.Mono | BassFlags.SpeakerLeft);
                BassMix.MixerAddChannel(_mixer, splitter, BassFlags.Mono | BassFlags.SpeakerRight);
            }

            // Play the mixer stream
            Bass.ChannelPlay(_mixer);
        }


    }
}
