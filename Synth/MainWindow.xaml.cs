using System.Collections.Generic;
using System.Windows;
using NAudio.Wave;

namespace Synth
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IWavePlayer audioOut;

        private List<int> pressedKeys;
        private Dictionary<int, int> keyNote;
        private int octave;

        public MainWindow()
		{
			InitializeComponent();

            octave = 1;
            pressedKeys = new List<int>();
            keyNote = new Dictionary<int, int>()
            {
                {44, 0},
                {66, 1},
                {62, 2},
                {48, 3},
                {47, 4},
                {49, 5},
                {63, 6},
                {50, 7},
                {68, 8},
                {51, 9},
                {64, 10},
                {53, 11},
                {54, 12},
                {58, 13},
                {55, 14},
                {59, 15},
                {140, 16},
                {152, 17},
                {151, 18}
            };

			AudioOutInit();
		}

		private void AudioOutInit()
		{
            audioOut = new AsioOut();
            audioOut.Init(IoCContainer.Get<AudioEngine>());
            Constants.SamplesPerChannel = ((AsioOut)audioOut).FramesPerBuffer; //TODO: Clean up
            IoCContainer.Get<AudioEngine>().Init();
            audioOut.Play();
		}

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (!keyNote.ContainsKey((int)e.Key))
                return;

            if (pressedKeys.Find(k => k == (int)e.Key) == default(int))
            {
                pressedKeys.Add((int)e.Key);
                IoCContainer.Get<AudioEngine>().KeyDown(keyNote[(int)e.Key] + octave * 12);
            }
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!keyNote.ContainsKey((int)e.Key))
                return;

            pressedKeys.Remove((int)e.Key);
            IoCContainer.Get<AudioEngine>().KeyUp(keyNote[(int)e.Key] + octave * 12);
		}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            audioOut.Stop();
        }
    }
}
