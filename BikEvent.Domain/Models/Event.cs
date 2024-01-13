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

        [Display(Name = "InitialSalary", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public double InitialSalary { get; set; }

        [Display(Name = "FinalSalary", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public double FinalSalary { get; set; }

        [Display(Name = "EventType", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string EventType { get; set; }

        [Display(Name = "TecnologyTools", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string TecnologyTools { get; set; }

        [Display(Name = "CompanyDescription", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        public string CompanyDescription { get; set; }

        [Display(Name = "JobDescription", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string JobDescription { get; set; }

        [Display(Name = "Benefits", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        public string Benefits { get; set; }

        [Display(Name = "InterestedSendEmailTo", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        [EmailAddress(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E002")]
        public string InterestedSendEmailTo { get; set; }

        [Display(Name = "PublicationDate", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        public DateTime PublicationDate { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
