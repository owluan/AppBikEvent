﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BikEvent.Domain.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Name", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(10, ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E003")]
        public string Name { get; set; }

        [Display(Name = "Email", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        [EmailAddress(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E002")]
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(6, ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E003")]
        public string Password { get; set; }

        [Display(Name = "CityState", ResourceType = typeof(BikEvent.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BikEvent.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string CityState { get; set; }

        public string ProfilePhoto {  get; set; }

        public string CoverPhoto {  get; set; }

        public string Description { get; set; }

        public string Instagram { get; set; }

        public string Strava { get; set; }

        public string Skills { get; set; }
    }
}
