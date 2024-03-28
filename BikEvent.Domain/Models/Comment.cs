using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BikEvent.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        [MaxLength(400)]
        public string CommentText { get; set; }

        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}
