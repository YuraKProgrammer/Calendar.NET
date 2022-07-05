using System;

namespace Calendar.Models
{
    public class Event
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Дата начала события
        /// </summary>
        public DateTime Date { get; set; }
    }
}
