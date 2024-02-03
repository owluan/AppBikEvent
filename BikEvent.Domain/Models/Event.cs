using BikEvent.Domain.Utility.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BikEvent.Domain.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Company", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string Company { get; set; }

        [Display(Name = "EventTitle", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string EventTitle { get; set; }

        [Display(Name = "CityState", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string CityState { get; set; }

        [Display(Name = "EventDate", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public DateTime EventDate { get; set; }

        [Display(Name = "NextEventDate", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        public DateTime NextEventDate { get; set; }

        [Display(Name = "RepeatInterval", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public RepeatInterval RepeatInterval { get; set; }

        [Display(Name = "EventType", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string EventType { get; set; }

        [Display(Name = "Tag", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string Tag { get; set; }

        [Display(Name = "SocialMedia", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        public string SocialMedia { get; set; }

        [Display(Name = "Description", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string Description { get; set; }

        [Display(Name = "Difficulty", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string Difficulty { get; set; }

        [Display(Name = "Benefits", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        public string Benefits { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        [Phone(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E002")]
        public string PhoneNumber { get; set; }

        [NotMapped]
        public List<string> ImageUrl { get; set; }

        [Display(Name = "PublicationDate", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        public DateTime PublicationDate { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}
