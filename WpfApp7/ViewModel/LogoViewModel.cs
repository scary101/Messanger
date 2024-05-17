using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp7.Model;
using WpfApp7.ViewModel.Helpers;

namespace WpfApp7.ViewModel
{
    public class LogoViewModel : HelpCommand
    {
        private ObservableCollection<string> logoMes = new ObservableCollection<string>();
        private TestTcp server;
        public ObservableCollection<string> LogoMes
        {
            get { return logoMes; }
            set
            {
                logoMes = value;
                OnPropertyChanged(nameof(LogoMes));
            }
        }

        public LogoViewModel(ObservableCollection<string> logo)
        {
            LogoMes = logo;
        }
    }
}
