using JustNotes.WPF.Models;
using JustNotes.WPF.ViewModel.Commands;
using JustNotes.WPF.ViewModel.Helpers;
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
			set 
			{ 
				user = value;
				OnPropertyChanged("User");
			}
		}

		private string password;

		public string Password
		{
			get { return password; }
			set 
			{ 
				password = value;
                User = new User
                {
                    Password = password,
					Email = this.Email,
					FirstName = this.FirstName,
                    LastName = this.LastName,
                };
                OnPropertyChanged("Password");
            }
		}

        private string email;

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                User = new User
                {
                    Password = this.Password,
					Email = email,
					FirstName = this.FirstName,
                    LastName = this.LastName,
                };
                OnPropertyChanged("Email");
            }
        }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                User = new User
                {
                    Password = this.Password,
                    Email = this.Email,
					FirstName = firstName,
                    LastName = this.LastName,
                };
                OnPropertyChanged("FirstName");
            }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                User = new User
                {
                    Password = this.Password,
                    Email = this.Email,
                    FirstName = this.FirstName,
                    LastName = lastName
                };
                OnPropertyChanged("LastName");
            }
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

			User = new User();
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

		public async void Login()
		{
            bool result = await AuthHepler.Login(User);
            if (result)
            {
                Authenticated?.Invoke(this, new EventArgs());
            }
		}

		public async void Register()
		{
            bool result = await AuthHepler.Register(User);
            if (result)
            {
                Authenticated?.Invoke(this, new EventArgs());
            }
		}

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Authenticated;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
