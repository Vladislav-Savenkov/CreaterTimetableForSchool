using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FuckingSchoolProject
{
    public class Room : INotifyPropertyChanged
    {
        private string name;
        public int RoomID { get; set; }
        public string RoomName 
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("RoomName");
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
