using Calendar.Services;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Calendar.ViewModels
{
    public class EventViewModel : INotifyPropertyChanged
    {
        private readonly GoogleCalendarService googleCalendarService;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICommand CreateEventCommand { get; }

        public EventViewModel()
        {
            googleCalendarService = new GoogleCalendarService();
            CreateEventCommand = new Command(CreateEvent);
        }

        private async void CreateEvent()
        {
            var newEvent = new Event
            {
                Summary = Title,
                Description = Description,
                Start = new EventDateTime { DateTime = StartTime },
                End = new EventDateTime { DateTime = EndTime }
            };

            try
            {
                var createdEvent = await googleCalendarService.CreateEventAsync(newEvent);
                // Realizar cualquier acción necesaria después de la creación del evento
                await Application.Current.MainPage.DisplayAlert("Success", "Event created successfully", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to create event: {ex.Message}", "OK");
            }
        }
    }
}
