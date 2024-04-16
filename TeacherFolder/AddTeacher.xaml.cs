using FuckingSchoolProject.LessonsFolder;
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
using FuckingSchoolProject.TeacherFolder;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace FuckingSchoolProject.TeacherFolder
{
    /// <summary>
    /// Логика взаимодействия для AddTeacher.xaml
    /// </summary>
    public partial class AddTeacher : Window
    {
        private Dictionary<string, int> LessonsOfTeacherBefore = new Dictionary<string, int>();
        private Dictionary<string, int> Lessons = new Dictionary<string, int>();

        public Teacher Teacher { get; set; }
        ApplicationContext db;
        public AddTeacher(Teacher t)
        {
            InitializeComponent();
            TBoxTeacherName.Text = t.TeacherName;
            TBoxTeacherPhone.Text = t.TeacherPhone;
            CreateRoomContext(t);
            CreateLessonsContext(t);
            Teacher = t;
        }

        private void CreateRoomContext(Teacher t)
        {
            db = new ApplicationContext();
            db.Rooms.Load();

            foreach (var room in db.Rooms.Local)
            {
                ComboBoxItem tmp = new ComboBoxItem();
                tmp.Content = room.RoomName;

                if (room.RoomName == t.TeacherRoom)
                    tmp.IsSelected = true;
                CBoxTeacherRoom.Items.Add(tmp);
            }
        }
        private void CreateLessonsContext(Teacher t)
        {
            db = new ApplicationContext();
            db.Lessons.Load();

            db.Teachers.Load();
            this.DataContext = db.Teachers.Local.ToBindingList();
            List<StackPanel> LessonsContext = new List<StackPanel>();
            foreach (var lesson in db.Lessons.Local)
            {
                CheckBox checkBox = new CheckBox();
                TextBlock textBlock = new TextBlock();
                textBlock.Text = lesson.LessonName;
                if (t.TeacherLessons != null && t.TeacherLessons.Contains(lesson.LessonName) == true)
                {
                    checkBox.IsChecked = true;
                    LessonsOfTeacherBefore.Add(lesson.LessonName, lesson.LessonID);
                }
                else
                    checkBox.IsChecked = false;
                Lessons.Add(lesson.LessonName, lesson.LessonID);
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                checkBox.Margin = new Thickness(3);
                textBlock.Margin = new Thickness(3);
                stackPanel.Children.Add(checkBox);
                stackPanel.Children.Add(textBlock);
                LessonsContext.Add(stackPanel);
            }
            lessonsList.ItemsSource = LessonsContext;
        }

        private static (string, Dictionary<string, int>) CreateLessonsOfTeacher(ListBox lessonsList, Dictionary<string, int> LessonsSource)
        {
            string LessonsString = "";
            Dictionary<string, int> LessonsID = new Dictionary<string, int>();
            foreach (StackPanel i in lessonsList.ItemsSource)
            {
                CheckBox checkBox = i.Children[0] as CheckBox;
                bool LessonIsChecked = checkBox.IsChecked.Value;
                if (LessonIsChecked == true)
                {
                    TextBlock tmp = i.Children[1] as TextBlock;
                    LessonsString += tmp.Text + '|';
                    LessonsID.Add(tmp.Text, LessonsSource[tmp.Text]);
                }
            }
            return (LessonsString, LessonsID);
        }

        private void ChangeTeachersOfLessons(Dictionary<string, int> LessonsOfTeacherAfter, Teacher teacher)
        {
            db = new ApplicationContext();
            foreach (KeyValuePair<string, int> oldLesson in LessonsOfTeacherBefore)
            {
                if (!LessonsOfTeacherAfter.Contains(oldLesson))
                {
                    var lesson = db.Lessons.Find(oldLesson.Value);
                    lesson.TeachersNames.Replace(teacher.TeacherName + '|', "");
                    db.Entry(lesson).State = EntityState.Modified;
                }
            }
            foreach (KeyValuePair<string, int> newLesson in LessonsOfTeacherAfter)
            {
                if (!LessonsOfTeacherBefore.Contains(newLesson))
                {
                    var lesson = db.Lessons.Find(newLesson.Value);
                    lesson.TeachersNames += teacher.TeacherName + '|';
                    db.Entry(lesson).State = EntityState.Modified;
                }
            }
            db.SaveChanges();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AcceptTeacher(object sender, RoutedEventArgs e)
        {
            if (TBoxTeacherName.Text == "")
            {
                MessageBox.Show("Поле \"Имя педагога\" не может быть пустым!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int CountOfNumberinPhone = 0;
            foreach(char c in TBoxTeacherPhone.Text)
            {
                if (Int32.TryParse(c.ToString(), out int tmp))
                    CountOfNumberinPhone++;
            }
            if (CountOfNumberinPhone != 10)
            {
                MessageBox.Show("Указан некорректный номер телефона (необходимо 10 цифр)", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Teacher.TeacherName = TBoxTeacherName.Text;
            Teacher.TeacherPhone = TBoxTeacherPhone.Text;
            Teacher.TeacherRoom = CBoxTeacherRoom.SelectionBoxItem.ToString();
            var data_tmp = CreateLessonsOfTeacher(lessonsList, Lessons); // tuple(Lessons as string, Lessons with ID)
            Teacher.TeacherLessons = data_tmp.Item1;
            ChangeTeachersOfLessons(data_tmp.Item2, Teacher);
            this.DialogResult = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CBoxTeacherRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBoxTeacherRoom.SelectedValue = CBoxTeacherRoom.SelectedItem;
        }

        private void TBoxTeacherPhone_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (TBoxTeacherName.Text == null)
                return;
            int size = TBoxTeacherName.Text.Length;
            for (int i = 0; i < size; i++)
            {
                if (i > 1)
                {
                    TBoxTeacherName.Text.Insert(i, " (");
                    size++;
                }
            }
        }
       
    }
}
