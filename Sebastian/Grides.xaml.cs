
using System.Windows.Controls;
using System.Windows;
using Microsoft.Expression.Encoder.Devices;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System;

namespace Sebastian
{
    /// <summary>
    /// Lógica de interacción para Grid.xaml
    /// </summary>

    public partial class Grides : UserControl
    {
        public Grides()
        {
            InitializeComponent();

            this.DataContext = this;

            VideoDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
            AudioDevices = EncoderDevices.FindDevices(EncoderDeviceType.Audio);
        }
        public Collection<EncoderDevice> VideoDevices { get; set; }
        public Collection<EncoderDevice> AudioDevices { get; set; }
        private void StartCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Display webcam video
                WebcamViewer.StartPreview();
            }
            catch (Microsoft.Expression.Encoder.SystemErrorException ex)
            {
                MessageBox.Show("Device is in use by another application");
            }
        }

        private void StopCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop the display of webcam video.
            WebcamViewer.StopPreview();
        }

        private void StartRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            // Start recording of webcam video to harddisk.
            WebcamViewer.StartRecording();
        }

        private void StopRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop recording of webcam video to harddisk.
            WebcamViewer.StopRecording();
        }

        private void TakeSnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            // Take snapshot of webcam video.
            WebcamViewer.SnapshotFormat = ImageFormat.Bmp;
            WebcamViewer.TakeSnapshot("SnapshotP");
            PreviewBMP.Children.Clear();
            PreviewBMP.Children.Add(new Preview());
        }
        private void Mostrar_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}