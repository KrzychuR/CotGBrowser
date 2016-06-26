using GotGLib;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CotGBrowser.Views
{
    public class ScrollToBottomAction : TriggerAction<RichTextBox>
    {
        protected override void Invoke(object parameter)
        {
            AssociatedObject.ScrollToEnd();
        }
    }
    
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : MetroWindow
    {
        public ChatWindow()
        {
            InitializeComponent();
            this.AllowsTransparency = true;

            if(IoCHelper.IsInitialized)
            {
                var mv = new ChatWindowMV();
                this.DataContext = mv;
                rtb.Document = mv.ChatDoc;

                mv.ScrollDocumentToBottom += Mv_ScrollDocumentToBottom;
            }

            //Owner = Application.Current.MainWindow;
        }

        private void Mv_ScrollDocumentToBottom(object sender, EventArgs e)
        {
            ScrollViewer scroller = FindScroll(rtb as Visual);

            if (scroller != null)
                (scroller as ScrollViewer).ScrollToBottom();
        }

        private ChatWindowMV ModalView { get { return this.DataContext as ChatWindowMV; } }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ModalView.SendMessage();
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                ModalView.ShowPrevMsg();
            }
            else if (e.Key == Key.Down)
            {
                ModalView.ShowNextMsg();
            }

        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            ModalView.HyperLinkClicked(sender, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer scroller = FindScroll(rtb as Visual);

            if (scroller != null)
                (scroller as ScrollViewer).ScrollToBottom();
        }

        public static ScrollViewer FindScroll(Visual visual)
        {
            if (visual is ScrollViewer)
                return visual as ScrollViewer;

            ScrollViewer searchChiled = null;
            DependencyObject chiled;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                chiled = VisualTreeHelper.GetChild(visual, i);
                if (chiled is Visual)
                    searchChiled = FindScroll(chiled as Visual);
                if (searchChiled != null)
                    return searchChiled;
            }

            return null;
        }

    }
}
