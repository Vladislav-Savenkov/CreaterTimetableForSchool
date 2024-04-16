using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FuckingSchoolProject
{
    public class Class : INotifyPropertyChanged
    {
        private string name;
        private string leaderName;
        private string leaderPhone;
        private string lessons;
        public int ClassID { get; set; }
        public string ClassName
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("ClassName");
            }
        }
        public string ClassLeaderName
        {
            get { return leaderName; }
            set
            {
                leaderName = value;
                OnPropertyChanged("ClassLeaderName");
            }
        }
        public string ClassLeaderPhone
        {
            get { return leaderPhone; }
            set
            {
                leaderPhone = value;
                OnPropertyChanged("ClassLeaderPhone");
            }
        }
        public string ClassLessons
        {
            get { return lessons; }
            set
            {
                lessons = value;
                OnPropertyChanged("ClassLessons");
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
