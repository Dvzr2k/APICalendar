using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Calendar.Services
{
    public class GoogleCalendarService
    {
        private readonly CalendarService calendarService;

        public GoogleCalendarService()
        {
            var scopes = new List<string>
            {
                CalendarService.Scope.Calendar,
                CalendarService.Scope.CalendarEvents,
                CalendarService.Scope.CalendarEventsReadonly
            };

            var credentials = GoogleCredential.FromFile(@"D:\Users\diegu\Source\\Repos\Calendar\Calendar\Calendar\ViewModels\credentials.json")
    .CreateScoped(scopes);


            calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "Calendar Sample"
            });
        }

        public async Task<Event> CreateEventAsync(Event newEvent)
        {
            var request = calendarService.Events.Insert(newEvent, "primary");
            return await request.ExecuteAsync();
        }

        public async Task<IList<Event>> GetUpcomingEventsAsync()
        {
            var request = calendarService.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            var events = await request.ExecuteAsync();
            return events.Items;
        }
    }
}
