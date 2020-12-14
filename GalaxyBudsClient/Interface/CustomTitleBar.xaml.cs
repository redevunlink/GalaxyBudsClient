﻿using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace GalaxyBudsClient.Interface
{
    public class CustomTitleBar : UserControl
    {
        private Button minimizeButton;
        private Button closeButton;
        private Button optionsButton;

        private DockPanel titleBar;
        private Control titleBarBackground;
        private TextBlock systemChromeTitle;
        private NativeMenuBar seamlessMenuBar;
        private NativeMenuBar defaultMenuBar;

        public static readonly StyledProperty<bool> IsSeamlessProperty =
            AvaloniaProperty.Register<CustomTitleBar, bool>(nameof(IsSeamless));

        public bool IsSeamless
        {
            get { return GetValue(IsSeamlessProperty); }
            set
            {
                SetValue(IsSeamlessProperty, value);
                if (titleBarBackground != null && 
                    systemChromeTitle != null &&
                    seamlessMenuBar != null &&
                    defaultMenuBar != null)
                {
                    titleBarBackground.IsVisible = !IsSeamless;
                    systemChromeTitle.IsVisible = !IsSeamless;
                    seamlessMenuBar.IsVisible = IsSeamless;
                    defaultMenuBar.IsVisible = !IsSeamless;

                    if (IsSeamless == false)
                    {
                        titleBar.Resources["SystemControlForegroundBaseHighBrush"] = new SolidColorBrush { Color = new Color(255, 0, 0, 0) };
                    }
                }
            }
        }
        
        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<CustomTitleBar, string>(nameof(Title));
        
        public event EventHandler? OptionsPressed;

        public Button OptionsButton
        {
            set => optionsButton = value;
            get => optionsButton;
        }
        
        public string Title
        {
            get { return GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
            }
        }
        
        public CustomTitleBar()
        {
            this.InitializeComponent();

            minimizeButton = this.FindControl<Button>("MinimizeButton");
            closeButton = this.FindControl<Button>("CloseButton");
            optionsButton = this.FindControl<Button>("OptionsButton");
            
            minimizeButton.Click += MinimizeWindow;
            closeButton.Click += CloseWindow;
            optionsButton.Click += (sender, args) => OptionsPressed?.Invoke(sender, EventArgs.Empty);

            titleBar = this.FindControl<DockPanel>("TitleBar");
            titleBarBackground = this.FindControl<Control>("TitleBarBackground");
            systemChromeTitle = this.FindControl<TextBlock>("SystemChromeTitle");
            seamlessMenuBar = this.FindControl<NativeMenuBar>("SeamlessMenuBar");
            defaultMenuBar = this.FindControl<NativeMenuBar>("DefaultMenuBar");
            
            SubscribeToWindowState();
        }

        private void CloseWindow(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window hostWindow = (Window)this.VisualRoot;
            hostWindow.Close();
        }
        
        private void MinimizeWindow(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window hostWindow = (Window)this.VisualRoot;
            hostWindow.WindowState = WindowState.Minimized;
        }

        private async void SubscribeToWindowState()
        {
            Window? hostWindow = (Window?)this.VisualRoot;

            while (hostWindow == null)
            {
                hostWindow = (Window?)this.VisualRoot;
                await Task.Delay(50);
            }
            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
