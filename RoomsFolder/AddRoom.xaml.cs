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

namespace FuckingSchoolProject.RoomsFolder
{
    /// <summary>
    /// Логика взаимодействия для AddRoom.xaml
    /// </summary>
    public partial class AddRoom : Window
    {
        public Room Room { get; private set; }
        public AddRoom(Room r)
        {
            InitializeComponent();
            Room = r;
            this.DataContext = Room;
        }

        private void Click_AcceptRoom(object sender, RoutedEventArgs e)
        {
            if (TBoxRoomName.Text == "")
            {
                MessageBox.Show("Поле \"Название помещения\" не может быть пустым!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
                this.DialogResult = true;
        }
    }
}
