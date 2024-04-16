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
using System.Data.Entity;

namespace FuckingSchoolProject.ClassFolder
{
    /// <summary>
    /// Логика взаимодействия для AddLessonOfClass.xaml
    /// </summary>
    public partial class AddLessonOfClass : Window
    {
        public LocalLesson LocalLesson { get; set; }
        ApplicationContext db;
        Dictionary<string, int> LocalLessonsContext = new Dictionary<string, int>();
        public AddLessonOfClass(LocalLesson ll)
        {
            InitializeComponent();
            CreateLessonsContext(ll);
            if (CBoxLessonName.SelectedItem != null)
                CreateTeachersContext((CBoxLessonName.SelectedItem as ComboBoxItem).Content.ToString(), ll);
            LocalLesson = ll;
            TBoxLessonHours.Text = ll.LocalLessonHours;
        }
        private void CreateLessonsContext(LocalLesson ll)
        {
            db = new ApplicationContext();
            db.Lessons.Load();
            foreach(var lesson in db.Lessons.Local)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = lesson.LessonName;
                LocalLessonsContext.Add(lesson.LessonName, lesson.LessonID);
                if (lesson.LessonName == ll.LocalLessonName)
                    comboBoxItem.IsSelected = true;
                CBoxLessonName.Items.Add(comboBoxItem);
                
            }
        }
        private void CreateTeachersContext(string lesson, LocalLesson ll)
        {
            if (lesson == null)
                return;
            CBoxTeacherName.Items.Clear();
            db = new ApplicationContext();
            db.Lessons.Load();
            foreach(var teacher in db.Lessons.Local[LocalLessonsContext[lesson]].TeachersNames.Split('|'))
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = teacher;
                if (teacher == ll.LocalLessonTeacher)
                    comboBoxItem.IsSelected = true;
                CBoxTeacherName.Items.Add(comboBoxItem);
            }
        }

        private void CBoxLessonName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBoxLessonName.SelectedItem != null)
                CreateTeachersContext((CBoxLessonName.SelectedItem as ComboBoxItem).Content.ToString(), new LocalLesson());
        }

        private void AcceptLessonOfClass_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxLessonName.SelectedItem == null)
            {
                MessageBox.Show("Поле \"Предмет\" не может быть пустым!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (CBoxTeacherName.SelectedItem == null)
            {
                MessageBox.Show("Поле \"Преподаватель\" не может быть пустым!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (TBoxLessonHours.Text == "")
            {
                MessageBox.Show("Поле \"Количество часов\" не может быть пустым!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            LocalLesson.LocalLessonName = (CBoxLessonName.SelectedItem as ComboBoxItem).Content.ToString();
            LocalLesson.LocalLessonTeacher = (CBoxTeacherName.SelectedItem as ComboBoxItem).Content.ToString();
            LocalLesson.LocalLessonHours = TBoxLessonHours.Text;
            this.DialogResult = true;
        }
    }
}
