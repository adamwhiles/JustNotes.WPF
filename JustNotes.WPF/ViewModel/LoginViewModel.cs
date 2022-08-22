using JustNotes.WPF.Models;
using JustNotes.WPF.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustNotes.WPF.ViewModel
{
    public class LoginViewModel
    {
		private User user;

		public User User
		{
			get { return user; }
			set { user = value; }
		}

		public RegisterCommand RegisterCommand { get; set; }

		public LoginCommand LoginCommand { get; set; }

		public LoginViewModel()
		{
			RegisterCommand = new RegisterCommand(this);
			LoginCommand = new LoginCommand(this);
		}
	}
}
