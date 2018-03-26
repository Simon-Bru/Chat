using System;

namespace ChatApplication
{
    public class Message
    {
        private String     clientName;
        private String     message;
        private DateTime   sentAt;

        public string ClientName
        {
            get => clientName;
            set => clientName = value;
        }

        public string Message1
        {
            get => message;
            set => message = value;
        }

        public DateTime SentAt
        {
            get => sentAt;
            set => sentAt = value;
        }
    }
}