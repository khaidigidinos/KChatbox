using System;
using System.Collections.Generic;

namespace SignalRApi.ViewModels
{
    public class RoomViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> users { get; set; }
    }
}
