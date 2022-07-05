using System;
using System.ComponentModel.DataAnnotations;

namespace Calendar.DataModels
{
    public class EventRecord
    {
        public uint Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Дата начала события
        /// </summary>
        public DateTime Date { get; set; }

        public uint UserId { get; set; }
    }
}