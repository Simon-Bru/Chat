using System;

namespace ChatApplication
{
    public class Message
    {
        private String     clientName;
        private String     content;
        private DateTime   sentAt;

        public string ClientName
        {
            get => clientName;
            set => clientName = value;
        }

        public string Content
        {
            get => content;
            set => content = value;
        }

        public DateTime SentAt
        {
            get => sentAt;
            set => sentAt = value;
        }
    }
}