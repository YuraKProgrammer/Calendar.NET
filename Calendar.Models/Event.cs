﻿namespace Calendar.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Дата начала события
        /// </summary>
        public DateTime Date { get; set; }
    }
}
