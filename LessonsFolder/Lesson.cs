using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FuckingSchoolProject
{
    public class Lesson :INotifyPropertyChanged
    {
        private string name;
        private string teachersNames;
        public int LessonID { get; set; }
        public string LessonName
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("LessonName");
            }
        }
        public string TeachersNames
        {
            get { return teachersNames; }
            set
            {
                teachersNames = value;
                OnPropertyChanged("TeachersNames");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
