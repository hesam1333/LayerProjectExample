using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class EventListForSearchDto
    {
        public int Id { get; set; }
        public DateTime EntryDatetime { get; set; }
        public string EventTitle { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }


    }
}
