using JustNotes.WPF.Models;
using JustNotes.WPF.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JustNotes.WPF.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
		private bool isShowingRegister = false;
		private User user;

		public User User
		{
			get { return user; }
			set { user = value; }
		}

		private Visibility loginVisibility;

		public Visibility LoginVisibility
		{
			get { return loginVisibility; }
			set 
			{ 
				loginVisibility = value;
				OnPropertyChanged("LoginVisibility");
			}
		}

        private Visibility registerVisibility;

        public Visibility RegisterVisibility
        {
            get { return registerVisibility; }
            set
            {
                registerVisibility = value;
                OnPropertyChanged("RegisterVisibility");
            }
        }


        public RegisterCommand RegisterCommand { get; set; }

		public LoginCommand LoginCommand { get; set; }

		public ShowRegisterCommand ShowRegisterCommand { get; set; }

		public LoginViewModel()
		{
			loginVisibility = Visibility.Visible;
			registerVisibility = Visibility.Collapsed;
			RegisterCommand = new RegisterCommand(this);
			LoginCommand = new LoginCommand(this);
			ShowRegisterCommand = new ShowRegisterCommand(this);
		}

		public void SwitchViews()
		{
			isShowingRegister = !isShowingRegister;

			if(isShowingRegister)
			{
				RegisterVisibility = Visibility.Visible;
				LoginVisibility	= Visibility.Collapsed;
			} else
			{
                RegisterVisibility = Visibility.Collapsed;
                LoginVisibility = Visibility.Visible;
            }
		}

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
