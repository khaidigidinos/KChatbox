using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SignalRApi.Ultilities;

namespace SignalRApi.Entities.Room
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("messages")]
        public List<Message> Messages { get; set; }

        [BsonRepresentation(BsonType.Int64)]
        [BsonElement("updatedAt")]
        public long UpdatedAt { get; set; } = DateTimeUltility.UnixTimeStamp;

        [BsonRepresentation(BsonType.String)]
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("users")]
        public List<string> Users { get; set; }

        public class Message
        {
            [BsonElement("id")]
            public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
            [BsonElement("content")]
            public string Content { get; set; }
            [BsonElement("senderId")]
            public string SenderId { get; set; }
            [BsonElement("sentAt")]
            public long SentAt { get; set; }
            [BsonElement("createdAt")]
            public long CreatedAt { get; set; } = DateTimeUltility.UnixTimeStamp;
            [BsonElement("udpatedAt")]
            public long UpdatedAt { get; set; } = DateTimeUltility.UnixTimeStamp;
            [BsonElement("DeletedAt")]
            public long? DeletedAt { get; set; } = null;
        }

        public class MessageComparer : IComparer<Message>
        {
            public int Compare(Message x, Message y)
            {
                return x.SentAt < y.SentAt ? -1 : 1;
            }
        }
    }
}
