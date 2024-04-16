using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FuckingSchoolProject.LessonsFolder
{
    /// <summary>
    /// Логика взаимодействия для AddLesson.xaml
    /// </summary>
    public partial class AddLesson : Window
    {
        public Lesson Lesson { get; private set; }
        public AddLesson(Lesson l)
        {
            InitializeComponent();
            Lesson = l;
            this.DataContext = Lesson;
        }

        private void Click_AcceptLesson(object sender, RoutedEventArgs e)
        {
            if (TBoxLessonName.Text == "")
            {
                MessageBox.Show("Поле \"Название предмета\" не может быть пустым!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
                this.DialogResult = true;
        }
    }
}
