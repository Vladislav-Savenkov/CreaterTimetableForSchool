using FuckingSchoolProject.TeacherFolder;
using FuckingSchoolProject.LessonsFolder;
using FuckingSchoolProject.RoomsFolder;
using FuckingSchoolProject.ClassFolder;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Data.SQLite;
using System.Threading;


namespace FuckingSchoolProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db;
        public MainWindow()
        {
            InitializeComponent();
            db = new ApplicationContext();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            teachersList.SelectionMode = SelectionMode.Single;
            if (TeachersTab.IsSelected)
            {
                db.Teachers.Load();
                this.DataContext = db.Teachers.Local.ToBindingList();
                if (teachersList.SelectedItem == null)
                    teacherInformatinGrid.Visibility = Visibility.Hidden;
            }
            if (LessonsTab.IsSelected)
            {
                db.Lessons.Load();
                this.DataContext = db.Lessons.Local.ToBindingList();
            }
            if (RoomsTab.IsSelected)
            {
                db.Rooms.Load();
                this.DataContext = db.Rooms.Local.ToBindingList();
            }
            if (ClassesTab.IsSelected)
            {
                db.Classes.Load();
                this.DataContext = db.Classes.Local.ToBindingList();
                if (classList.SelectedItem == null)
                    classInformatinGrid.Visibility = Visibility.Hidden;
            }

        }

        #region TeachersBlock
        private void Add_TeacherMainClick(object sender, RoutedEventArgs e)
        {
            
            AddTeacher addTeacher = new AddTeacher(new Teacher());
            if (addTeacher.ShowDialog() == true)
            {
                if (addTeacher.Teacher == null)
                    return;
                Teacher teacher = addTeacher.Teacher;
                db.Teachers.Add(teacher);
                db.SaveChanges();
            }
        }

        private void Edit_TeacherMainClick(object sender, RoutedEventArgs e)
        {
            if (teachersList.SelectedItem == null)
                return;

            Teacher teacher = teachersList.SelectedItem as Teacher;
            AddTeacher addTeacher = new AddTeacher(new Teacher
            {
                TeacherID = teacher.TeacherID,
                TeacherName = teacher.TeacherName,
                TeacherPhone = teacher.TeacherPhone,
                TeacherLessons = teacher.TeacherLessons,
                TeacherRoom = teacher.TeacherRoom
            }) ;

            if (addTeacher.ShowDialog() == true)
            {
                teacher = db.Teachers.Find(addTeacher.Teacher.TeacherID);
                if (teacher != null)
                {
                    teacher.TeacherName = addTeacher.Teacher.TeacherName;
                    teacher.TeacherPhone = addTeacher.Teacher.TeacherPhone;
                    teacher.TeacherRoom = addTeacher.Teacher.TeacherRoom;
                    teacher.TeacherLessons = addTeacher.Teacher.TeacherLessons;
                    db.Entry(teacher).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            teachersList.SelectedItem = null;
            teachersList.SelectedItem = teacher;
        }

        private void Delete_TeacherMainClick(object sender, RoutedEventArgs e)
        {
            if (teachersList.SelectedItem == null)
                return;
            Teacher teacher = teachersList.SelectedItem as Teacher;
            db.Teachers.Remove(teacher);
            db.SaveChanges();
        }
        private void teachersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (teachersList.SelectedItem == null)
                return;
            teacherInformatinGrid.Visibility = Visibility.Visible;
            teacherInformatinGrid.IsEnabled = true;
            Teacher selectedTeacher = teachersList.SelectedItem as Teacher;
            teacherInformationGridName.Text = selectedTeacher.TeacherName;
            teacherInformationGridPhone.Text = selectedTeacher.TeacherPhone;
            teacherInformationGridRoom.Text = selectedTeacher.TeacherRoom;
            var selectedTeacherLessonsList = selectedTeacher.TeacherLessons.Split('|');
            teacherInformationGridLessonsView.ItemsSource = selectedTeacherLessonsList;
        }
        #endregion

        #region LessonBlock
        private void Click_AddLessonMain(object sender, RoutedEventArgs e)
        {
            AddLesson addLesson = new AddLesson(new Lesson());
            if (addLesson.ShowDialog() == true)
            {
                Lesson lesson = addLesson.Lesson;
                db.Lessons.Add(lesson);
                db.SaveChanges();
            }
        }

        private void Click_DeleteLessonMain(object sender, RoutedEventArgs e)
        {
            if (lessonsList.SelectedItem == null)
                return;

            Lesson lesson = lessonsList.SelectedItem as Lesson;
            db.Lessons.Remove(lesson);
            db.SaveChanges();
        }
        #endregion

        #region RoomBlock
        private void Click_AddRoomMain(object sender, RoutedEventArgs e)
        {
            AddRoom addRoom = new AddRoom(new Room());
            if (addRoom.ShowDialog() == true)
            {
                Room room = addRoom.Room;
                db.Rooms.Add(room);
                db.SaveChanges();
            }
        }

        private void Click_DeleteRoomMain(object sender, RoutedEventArgs e)
        {
            if (roomsList.SelectedItem == null)
                return;

            Room room = roomsList.SelectedItem as Room;
            db.Rooms.Remove(room);
            db.SaveChanges();
        }

        #endregion

        #region ClassBlock
        private void classList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (classList.SelectedItem == null)
                return;
            classInformationGridLessonsView.Items.Clear();
            classInformatinGrid.Visibility = Visibility.Visible;
            classInformatinGrid.IsEnabled = true;
            Class selectedClass = classList.SelectedItem as Class;
            classInformationGridName.Text = selectedClass.ClassName;
            classInformationGridLeaderPhone.Text = selectedClass.ClassLeaderPhone;
            classInformationGridLeaderName.Text = selectedClass.ClassLeaderName;
            foreach(string lesson in (classList.SelectedItem as Class).ClassLessons.Split('|'))
            {
                if (lesson != "")
                {
                    classInformationGridLessonsView.Items.Add(new tmpLesson { ClassLessonName = lesson.Split('*')[0], ClassLessonTeacher = lesson.Split('*')[1], ClassLessonHours = lesson.Split('*')[2] });
                }
            }
        }

        private void Add_ClassMainClick(object sender, RoutedEventArgs e)
        {
            AddClass addClass = new AddClass(new Class());
            if (addClass.ShowDialog() == true)
            {
                Class MyClass = addClass.Class;
                db.Classes.Add(MyClass);
                db.SaveChanges();
            }
        }

        private void Edit_ClassMainClick(object sender, RoutedEventArgs e)
        {
            if (classList.SelectedItem == null)
                return;
            Class MyClass = classList.SelectedItem as Class;
            AddClass addClass = new AddClass(new Class
            {
                ClassID = MyClass.ClassID,
                ClassLeaderName = MyClass.ClassLeaderName,
                ClassLeaderPhone = MyClass.ClassLeaderPhone,
                ClassLessons = MyClass.ClassLessons,
                ClassName = MyClass.ClassName
            });
            if (addClass.ShowDialog() == true)
            {
                MyClass = db.Classes.Find(addClass.Class.ClassID);
                if (MyClass != null)
                {
                    MyClass.ClassName = addClass.Class.ClassName;
                    MyClass.ClassLessons = addClass.Class.ClassLessons;
                    MyClass.ClassLeaderName = addClass.Class.ClassLeaderName;
                    MyClass.ClassLeaderPhone = addClass.Class.ClassLeaderPhone;
                    db.Entry(MyClass).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            classList.SelectedItem = null;
            classList.SelectedItem = MyClass;
        }

        private void Delete_ClassMainClick(object sender, RoutedEventArgs e)
        {
            if (classList.SelectedItem == null)
                return;
            db.Classes.Remove(classList.SelectedItem as Class);
        }
    public class tmpLesson
    {
        public string ClassLessonName { get; set; }
        public string ClassLessonTeacher { get; set; }
        public string ClassLessonHours { get; set; }
    }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Открытие страницы авторизации...", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            
            Timer t = new Timer((object o) => { System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=dQw4w9WgXcQ"); },
                null, 2000, 0);
           
        }
    }
}
