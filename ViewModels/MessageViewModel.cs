using System;
namespace SignalRApi.ViewModels
{
    public class MessageViewModel
    {
        public string Content { get; set; }
        public long SentAt { get; set; }
        public string SenderId { get; set; }
    }
}
