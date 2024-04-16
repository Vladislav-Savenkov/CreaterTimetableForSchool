using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FuckingSchoolProject
{
    public class Teacher : INotifyPropertyChanged
    {
        private string name;
        private string lessons;
        private string phone;
        private string room;
        public int TeacherID { get; set; }
        public string TeacherName
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("TeacherName");
            }
        }
        public string TeacherLessons
        {
            get { return lessons; }
            set
            {
                lessons = value;
                OnPropertyChanged("TeacherLessons");
            }
        }
        public string TeacherPhone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged("TeacherPhone");
            }
        }
        public string TeacherRoom
        {
            get { return room; }
            set
            {
                room = value;
                OnPropertyChanged("TeacherRoom");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
