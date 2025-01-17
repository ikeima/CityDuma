﻿using CityDuma.ViewModels;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace CityDuma.Views
{
    /// <summary>
    /// Логика взаимодействия для MeetingsView.xaml
    /// </summary>
    public partial class MeetingsView : Page
    {
        public MeetingsView()
        {
            InitializeComponent();

            DataContext = new MeetingsViewModel();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            
        }
    }
}