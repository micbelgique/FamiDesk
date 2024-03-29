﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FamiDesk.Mobile.App.Helpers;
using FamiDesk.Mobile.App.Messages;
using FamiDesk.Mobile.App.Models;
using FamiDesk.Mobile.App.Views;
using Xamarin.Forms;

namespace FamiDesk.Mobile.App.ViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        private string _personId;
        private string _firstName;
        private string _lastName;
        private string _address;
        private string _avatar;
        private string _comment;
        private string _familyInformation;
        private bool _isIn;
        private bool _isOut;

        public string PersonId
        {
            get => _personId;
            set => SetProperty(ref _personId, value);
        }
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        public string Avatar
        {
            get => _avatar;
            set => SetProperty(ref _avatar, value);
        }
        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }
        public string FamilyInformations
        {
            get => _familyInformation;
            set => SetProperty(ref _familyInformation, value);
        }
        public bool IsIn
        {
            get => _isIn;
            set => SetProperty(ref _isIn, value);
        }
        public bool IsOut
        {
            get => _isOut;
            set => SetProperty(ref _isOut, value);
        }

        public ICommand CheckInCommand { get; }
        public ICommand CheckOutCommand { get; }
        public INavigation Navigation { get; set; }

        public DetailsViewModel()
        {
            Title = "Details";
            CheckInCommand = new Command(async () => await ExecuteCheckInCommand());
            CheckOutCommand = new Command(async () => await ExecuteCheckOutCommand());
        }

        private async Task ExecuteCheckOutCommand()
        {
            await EventDataStore.AddItemAsync(new EventInfo
            {
                Id = Guid.NewGuid().ToString(),
                Comment = Comment,
                Date = DateTime.UtcNow,
                PersonId = PersonId,
                UserId = App.CurrentUserId,
                Type = "CheckOut"
            });

            await Navigation.PopToRootAsync();
            MessagingCenter.Send(this, "RefreshRootMessage", new RefreshRootMessage());
        }

        private async Task ExecuteCheckInCommand()
        {
            await EventDataStore.AddItemAsync(new EventInfo
            {
                Id = Guid.NewGuid().ToString(),
                Comment = Comment,
                Date = DateTime.UtcNow,
                PersonId = PersonId,
                UserId = App.CurrentUserId,
                Type = "CheckIn"
            });

            await Navigation.PopToRootAsync();
            MessagingCenter.Send(this, "RefreshRootMessage", new RefreshRootMessage());
        }

        public async void Load(Person person)
        {
            var eventInfos = await EventDataStore.GetItemsAsync();
            PersonId = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            Title = $"{FirstName} {LastName}";
            FamilyInformations = person.FamilyInformations;
            Address = person.Address;
            Avatar = person.Avatar;
            IsIn = eventInfos.Where(e => e.PersonId == PersonId && e.UserId == App.CurrentUserId)
                       .OrderByDescending(d => d.Date)
                       .Select(e => e.Type)
                       .FirstOrDefault() == "CheckIn";

            IsOut = !IsIn;
        }
    }
}
