using JustNotes.WPF.ViewModel;
using JustNotes.WPF.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JustNotes.WPF.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {

        NotesViewModel vm;
        public NotesWindow()
        {
            InitializeComponent();

            vm = Resources["vm"] as NotesViewModel;
            vm.SelectedNoteChanged += vm_SelectedNoteChanged;

            var fontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontFamilyComboBox.ItemsSource = fontFamilies;

            List<double> fontSizes = new List<double>() { 8,9,10,11,12,14,16,28,48,72};
            fontSizeComboBox.ItemsSource = fontSizes;
            
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (string.IsNullOrEmpty(App.UserId))
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Topmost = true;
                loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                loginWindow.ShowDialog();

                vm.GetNotes();
            }
        }

        private void vm_SelectedNoteChanged(object sender, EventArgs e)
        {
            if (vm.SelectedNote != null)
            {
                rtbNote.Document.Blocks.Clear();
                if (!string.IsNullOrEmpty(vm.SelectedNote.Content))
                {
                    string noteContent = vm.SelectedNote.Content;
                    byte[] byteArray = Encoding.ASCII.GetBytes(noteContent.ToString());
                    using (MemoryStream ms = new MemoryStream(byteArray))
                    {
                        var rtbContent = new TextRange(rtbNote.Document.ContentStart, rtbNote.Document.ContentEnd);
                        rtbContent.Load(ms, DataFormats.Rtf);
                        ms.Close();
                    }
                    

                }
                statusLastUpdated.Text = $"Last Updated {vm.SelectedNote.UpdatedDate}";
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void rtb_TextChanged(object sender, TextChangedEventArgs e)
        {
            int numberOfCharacters = (new TextRange(rtbNote.Document.ContentStart, rtbNote.Document.ContentEnd)).Text.Length;

            statusTextBlock.Text = $"{numberOfCharacters} characters";
        }

        private void boldToggle_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonChecked)
            {
                rtbNote.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            } else
            {
                rtbNote.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
            
        }

        private void rtbNote_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedWeight = rtbNote.Selection.GetPropertyValue(Inline.FontWeightProperty);
            var selectedDecoration = rtbNote.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            var selectedStyle = rtbNote.Selection.GetPropertyValue(Inline.FontStyleProperty);

            boldToggle.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && (selectedWeight.Equals(FontWeights.Bold));
            italicToggle.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) && (selectedStyle.Equals(FontStyles.Italic));
            underlineToggle.IsChecked = (selectedDecoration != DependencyProperty.UnsetValue) && (selectedDecoration.Equals(TextDecorations.Underline));

            fontFamilyComboBox.SelectedItem = rtbNote.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            fontSizeComboBox.Text = (rtbNote.Selection.GetPropertyValue(Inline.FontSizeProperty)).ToString();
        }

        private void underlineToggle_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonChecked)
            {
                rtbNote.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                TextDecorationCollection textDecorations;
                (rtbNote.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                rtbNote.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void italicToggle_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonChecked)
            {
                rtbNote.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                rtbNote.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(fontSizeComboBox.SelectedItem != null)
            {
                rtbNote.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }

        private void fontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbNote.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.Text);
        }

        private void saveNoteButton_Click(object sender, RoutedEventArgs e)
        {
            string rtbContent;
            var noteContent1 = new TextRange(rtbNote.Document.ContentStart, rtbNote.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                noteContent1.Save(ms, DataFormats.Rtf);
                rtbContent = Encoding.ASCII.GetString(ms.ToArray());
                vm.SelectedNote.Content = rtbContent;
                vm.SelectedNote.UpdatedDate = DateTime.Now;
                DBHelper.Update(vm.SelectedNote);
                statusLastUpdated.Text = $"Last Updated {vm.SelectedNote.UpdatedDate}";
                ms.Close();
            }
        }
    }
}
