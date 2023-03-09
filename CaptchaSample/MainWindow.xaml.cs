using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaptchaSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string errorMessage;

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public string Login { get; set; }
        public string CaptchaUserText { get; set; }

        public string ErrorMessage { 
            get => errorMessage;
            set
            {
                errorMessage = value;
                Signal();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }


        private void CheckAuth(string login, string pass)
        {
            ErrorMessage = null;
            var user = DB.Instance.Users.
                            Include(s => s.UserRoleNavigation).
                            FirstOrDefault(s => s.UserLogin == login &&
                                s.UserPassword == pass);
            if (user == null)
            { // неудачная авторизация
                ErrorMessage = "Ошибка авторизации";
                GenerateCaptcha();
            }
            else
            {// открыть следующую форму и тп 
             // лучше менять страницу на следующую

            }
        }

        Random random = new Random();
        string captchaText = null;
        private void GenerateCaptcha()
        {
            captchaText = GenerateText();
            captchCanvas.Children.Clear();
            captchaPanel.Visibility = Visibility.Visible;
            int x, y;
            x = y = 10;
            foreach (var s in captchaText)
            { 
                TextBlock textBlock = new TextBlock();
                textBlock.FontSize = 25;
                textBlock.Text = s.ToString();
                captchCanvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, x);
                Canvas.SetTop(textBlock, y);
                x += 8 + random.Next(-5, 10);
                y = 13 + random.Next(-5, 5);
            }
        }

        private bool CaptchaValid()
        {
            if (captchaText == null)
                return true;
            return captchaText == CaptchaUserText;
        }

        (int, int) digits = (48, 58);
        (int, int) charsUpper = (65, 91);
        (int, int) charsLower = (97, 123);

        private string GenerateText()
        {
            string result = null;
            for (int i = 0; i < 4; i++)
            {
                char s;
                switch (random.Next(3))
                { 
                    case 0: s = (char)random.Next(digits.Item1, digits.Item2); break;
                    case 1: s = (char)random.Next(charsUpper.Item1, charsUpper.Item2); break;
                    default: s = (char)random.Next(charsLower.Item1, charsLower.Item2); break;
                }
                result += s;
            }
            return result;
        }

        private void buttonEnter(object sender, RoutedEventArgs e)
        {
            if (CaptchaValid())
                CheckAuth(Login, passwordBox.Password);
            else
            {
                ErrorMessage = "Неверный код с картинки";
                GenerateCaptcha();
            }
        }

        
    }
}
