using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CapacitiveTouchScreenClicker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GpioPin pin;
        private GpioPinValue pinValue;
        private const int SIGNAL_PIN = 5;
        private DispatcherTimer timer;
        private double Hz = 35; 

        public MainPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1.0 / (Hz / 1000));
            timer.Tick += Timer_Tick;
            InitGPIO();
            if (pin != null)
            {
                timer.Start();
            }

            RateTextBlock.Text = Hz.ToString() + " Hz";
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pin = null;
                //GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            pin = gpio.OpenPin(SIGNAL_PIN);
            pinValue = GpioPinValue.High;
            pin.Write(pinValue);
            pin.SetDriveMode(GpioPinDriveMode.Output);

            //GpioStatus.Text = "GPIO pin initialized correctly.";

        }

        private void Timer_Tick(object sender, object e)
        {
            if (pinValue == GpioPinValue.High)
            {
                pinValue = GpioPinValue.Low;
                pin.Write(pinValue);
            }
            else
            {
                pinValue = GpioPinValue.High;
                pin.Write(pinValue);
            }
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if (Hz == 1)
                return;

            Hz--;
            timer.Interval = TimeSpan.FromMilliseconds(1.0 / (Hz/1000));
            RateTextBlock.Text = Hz.ToString() + " Hz";
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            Hz++;
            timer.Interval = TimeSpan.FromMilliseconds(1.0 / (Hz / 1000));
            RateTextBlock.Text = Hz.ToString() + " Hz";
        }
    }

}
