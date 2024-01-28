using BikEvent.Domain.Models;
using BikEvent.Domain.Utility.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BikEvent.App.ViewModels
{
    public class VisualizerViewModel : INotifyPropertyChanged
    {
        private Event _event;

        public VisualizerViewModel(Event initialEvent)
        {
            _event = initialEvent;
        }

        public string EventTitle
        {
            get { return _event.EventTitle; }
            set
            {
                if (_event.EventTitle != value)
                {
                    _event.EventTitle = value;
                    OnPropertyChanged(nameof(EventTitle));
                }
            }
        }

        public string CityState
        {
            get { return _event.CityState; }
            set
            {
                if (_event.CityState != value)
                {
                    _event.CityState = value;
                    OnPropertyChanged(nameof(CityState));
                }
            }
        }

        public DateTime EventDate
        {
            get { return _event.EventDate; }
            set
            {
                if (_event.EventDate != value)
                {
                    _event.EventDate = value;
                    OnPropertyChanged(nameof(EventDate));
                }
            }
        }

        public string Tag
        {
            get { return _event.Tag; }
            set
            {
                if (_event.Tag != value)
                {
                    _event.Tag = value;
                    OnPropertyChanged(nameof(Tag));
                }
            }
        }

        public string SocialMedia
        {
            get { return _event.SocialMedia; }
            set
            {
                if (_event.SocialMedia != value)
                {
                    _event.SocialMedia = value;
                    OnPropertyChanged(nameof(SocialMedia));
                }
            }
        }

        public string Description
        {
            get { return _event.Description; }
            set
            {
                if (_event.Description != value)
                {
                    _event.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string Benefits
        {
            get { return _event.Benefits; }
            set
            {
                if (_event.Benefits != value)
                {
                    _event.Benefits = value;
                    OnPropertyChanged(nameof(Benefits));
                }
            }
        }

        public string PhoneNumber
        {
            get { return _event.PhoneNumber; }
            set
            {
                if (_event.PhoneNumber != value)
                {
                    _event.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string EventType
        {
            get { return _event.EventType; }
            set
            {
                if (_event.EventType != value)
                {
                    _event.EventType = value;
                    OnPropertyChanged(nameof(EventType));
                }
            }
        }

        public string Difficulty
        {
            get { return _event.Difficulty; }
            set
            {
                if (_event.Difficulty != value)
                {
                    _event.Difficulty = value;
                    OnPropertyChanged(nameof(Difficulty));
                }
            }
        }

        public RepeatInterval RepeatInterval
        {
            get { return _event.RepeatInterval; }
            set
            {
                if (_event.RepeatInterval != value)
                {
                    _event.RepeatInterval = value;
                    OnPropertyChanged(nameof(RepeatInterval));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
