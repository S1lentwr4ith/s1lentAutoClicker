using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using WpfApp1.Click;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using WpfApp1.Enums;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        DispatcherTimer _timer1 = new DispatcherTimer();
        POINT pnt;
        bool _stop = true;
        
        public MainWindow()
        {
            InitializeComponent();
            _timer1.Tick += new EventHandler(TickMethod);
            ClicksPerSecSliderLmb.ValueChanged += Slider_ValueChanged;
            
        }
        private void Button_Click(object? sender = null, RoutedEventArgs? e = null)
        {
            _stop = (_stop) ? false : true;
            
            _timer1.Interval = new TimeSpan(0,0, 0, 0,  1000 / (int) ClicksPerSecSliderLmb.Value);

            if (!_stop )
            {
                _timer1.Start();
            }
            else { _timer1.Stop(); }
            
            
        }
        

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT pPoint);

        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        

        private void TickMethod(object sender, EventArgs e)

        {
            POINT pnt;
            GetCursorPos(out pnt);
            int coordX = pnt.X;
            int coordY = pnt.Y;
            ClickMethods.leftclick(coordX, coordY);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            leftMB.Content = $"{(int)ClicksPerSecSliderLmb.Value} CPS";
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;

        private IntPtr _windowHandle;
        private HwndSource _source;
        private uint NONE = 0x0000;
        private uint VK_F6 = 0x75;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e); 

            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            RegisterHotKey(_windowHandle, HOTKEY_ID, NONE, VK_F6); //F6
        } 


        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) //Geen idee
        {
            const int WM_HOTKEY = 0x0312;
            if (msg.Equals(WM_HOTKEY) && wParam.ToInt32() == HOTKEY_ID)
            {
                int vkey = (((int)lParam >> 16) & 0xFFFF);
                if (vkey == VK_F6)
                {
                    Button_Click();// wel idee
                }
                handled = true;
            }
            return IntPtr.Zero; // .....
        }

        protected override void OnClosed(EventArgs e) // Hooks afsluiten
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }
    }
}

