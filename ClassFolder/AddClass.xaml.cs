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
    /// Логика взаимодействия для AddClass.xaml
    /// </summary>
    public partial class AddClass : Window
    {

        public Class Class { get; set; }
        ApplicationContext db;
        Dictionary<string, int> Teachers = new Dictionary<string, int>(); //Name, ID
        
        public AddClass(Class c)
        {
            InitializeComponent();
            TBoxClassName.Text = c.ClassName;
            TBlockClassLeaderPhone.Text = c.ClassLeaderPhone;
            CreateLeadersContext(c);
            CreateLessonsOfClass(c);
            Class = c;
        }

        private void CreateLessonsOfClass(Class c)
        {
            if (c.ClassLessons != null)
            foreach (string lesson in c.ClassLessons.Split('|'))
            {
                if (lesson != "")
                {
                    LocalLesson localLesson = new LocalLesson();
                    localLesson.LocalLessonName = lesson.Split('*')[0];
                    localLesson.LocalLessonTeacher = lesson.Split('*')[1];
                    localLesson.LocalLessonHours = lesson.Split('*')[2];
                    lessonsLocalList.Items.Add(localLesson);
                }
            }
        }
        private void CreateLeadersContext(Class c)
        {
            db = new ApplicationContext();
            db.Teachers.Load();
            foreach (Teacher teacher in db.Teachers.Local)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = teacher.TeacherName;
                Teachers.Add(teacher.TeacherName, teacher.TeacherID);
                if (teacher.TeacherName == c.ClassLeaderName)
                {
                    item.IsSelected = true;
                    TBlockClassLeaderPhone.Text = teacher.TeacherPhone;
                }
                CBoxClassLeader.Items.Add(item);
                
            }
        }

        private void CBoxClassLeader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            db = new ApplicationContext();
            TBlockClassLeaderPhone.Text = db.Teachers.Find(Teachers[(CBoxClassLeader.SelectedItem as ComboBoxItem).Content.ToString()]).TeacherPhone;
        }

        private void AddLessonForClass_Click(object sender, RoutedEventArgs e)
        {
            AddLessonOfClass addLessonOfClass = new AddLessonOfClass(new LocalLesson());
            if (addLessonOfClass.ShowDialog() == true)
            {
                if (addLessonOfClass.LocalLesson == null)
                    return;
                LocalLesson localLesson = addLessonOfClass.LocalLesson;
                lessonsLocalList.Items.Add(localLesson);
            }
        }

        private void EditLessonOfClass_Click(object sender, RoutedEventArgs e)
        {
            if (lessonsLocalList.SelectedItem == null)
                return;
            LocalLesson localLesson = lessonsLocalList.SelectedItem as LocalLesson;
            AddLessonOfClass addLessonOfClass = new AddLessonOfClass(localLesson);
            if (addLessonOfClass.ShowDialog() == true)
            {
                lessonsLocalList.SelectedItem = addLessonOfClass.LocalLesson;
            }
        }
        private void AcceptClass_Click(object sender, RoutedEventArgs e)
        {
            if (TBoxClassName.Text == "")
            {
                MessageBox.Show("Поле \"Наименование класса\" не может быть пустым!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Class.ClassName = TBoxClassName.Text;
            Class.ClassLeaderName = (CBoxClassLeader.SelectedItem as ComboBoxItem).Content.ToString();
            Class.ClassLeaderPhone = TBlockClassLeaderPhone.Text;
            Class.ClassLessons = "";
            Class.ClassLessons = CreateOutputLessonsOfClass(ref lessonsLocalList);
            this.DialogResult = true;
        }
        private string CreateOutputLessonsOfClass(ref ListView listView)
        {
            string result = "";
            foreach(var lessonListViewItem in listView.Items)
            {
                LocalLesson localLesson = lessonListViewItem as LocalLesson;
                result += localLesson.LocalLessonName + '*' + localLesson.LocalLessonTeacher + '*' + localLesson.LocalLessonHours + '|';
            }
            return result;
        }

        private void DeleteLessonOfClass_Click(object sender, RoutedEventArgs e)
        {
            if (lessonsLocalList.SelectedItem == null)
                return;
            lessonsLocalList.Items.Remove(lessonsLocalList.SelectedItem);
        }
    }
    public class LocalLesson
    {
        public string LocalLessonName { get; set; }
        public string LocalLessonTeacher { get; set; }
        public string LocalLessonHours { get; set; }
    }
}

