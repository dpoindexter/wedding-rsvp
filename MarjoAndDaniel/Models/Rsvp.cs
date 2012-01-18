using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Norm;
using MarjoAndDaniel.Data;

namespace MarjoAndDaniel.Models
{
    public class Rsvp : IMongoIdentified
    {
        public Rsvp()
        {
            Primary = new Guest();
            PlusOne = new Guest();
        }

        public Rsvp(RsvpViewModel vm)
        {
            this.Primary = new Guest(vm.Primary);
            this.PlusOne = new Guest(vm.PlusOne);
            this.IsAllowedPlusOne = vm.IsAllowedPlusOne;
            this.IsGuestPrefilled = vm.IsGuestPrefilled;
        }

        public Rsvp Create()
        {
            this.HasResponded = false;
            this.RespondedOn = null;
            this.UpdatedOn = null;

            new mongo().SaveItem<Rsvp>(this);
            return this;
        }

        public Rsvp Save()
        {
            if (!this.HasResponded)
            {
                this.HasResponded = true;
                this.RespondedOn = DateTime.Now;
            }
            else
            {
                this.UpdatedOn = DateTime.Now;
            }

            new mongo().SaveItem<Rsvp>(this);
            return this;
        }

        public string PlusOneCss()
        {
            if (!this.IsAllowedPlusOne || this.PlusOne.IsAttending == null)
                return "";
            return (this.PlusOne.IsAttending ?? false)
                ? "accept"
                : "decline";
        }

        public static Rsvp Retrieve(RsvpViewModel vm)
        {
            var item = new mongo().FindOne<Rsvp>(i => i.Id == vm.Id);
            item.Primary.Name = vm.Primary.Name;
            item.Primary.IsAttending = new Guest(vm.Primary).IsAttending;
            item.PlusOne.Name = vm.PlusOne.Name;
            item.PlusOne.IsAttending = new Guest(vm.PlusOne).IsAttending;
            item.Message = vm.Message;
            return item;
        }

        public int? Id { get; set; }

        public Guest Primary { get; set; }
        public Guest PlusOne { get; set; }
        public string Message { get; set; }

        public bool IsAllowedPlusOne { get; set; }
        public bool IsGuestPrefilled { get; set; }

        public bool HasResponded { get; set; }
        public DateTime? RespondedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public List<string> SearchTerms
        {
            get
            {
                return new List<string> { this.Primary.Name, this.PlusOne.Name }
                    .Aggregate((memo, next) => memo + " " + next)
                    .Split(' ', '&')
                    .OrderBy(str => str)
                    .ToList();
            }
        }
    }

    public class Guest
    {
        public Guest() {}

        public Guest(GuestViewModel vm)
        {
            this.Name = vm.Name;
            if (String.IsNullOrEmpty(vm.IsAttending))
                this.IsAttending = null;
            else
                this.IsAttending = (vm.IsAttending == "Accept") ? true : false;
            this.IsEditable = vm.IsEditable;
        }

        public string Name { get; set; }
        public bool? IsAttending { get; set; }
        public bool IsEditable {get; set; }
    }

    public class RsvpViewModel
    {
        public RsvpViewModel()
        {
            this.Primary = new GuestViewModel();
            this.PlusOne = new GuestViewModel();
        }

        public RsvpViewModel(Rsvp rsvp)
        {
            this.Id = rsvp.Id;
            this.Primary = new GuestViewModel(rsvp.Primary);
            this.PlusOne = new GuestViewModel(rsvp.PlusOne);
            this.IsAllowedPlusOne = rsvp.IsAllowedPlusOne;
            this.IsGuestPrefilled = rsvp.IsGuestPrefilled;
            this.Message = rsvp.Message;
        }

        public string ToLabel()
        {
            return (!String.IsNullOrEmpty(this.PlusOne.Name))
                ? this.Primary.Name + " & " + this.PlusOne.Name
                : this.Primary.Name;
        }

        public int? Id { get; set; }

        public GuestViewModel Primary { get; set; }
        public GuestViewModel PlusOne { get; set; }

        public string Message { get; set; }
        public bool IsAllowedPlusOne { get; set; }
        public bool IsGuestPrefilled { get; set; }

    }

    public class GuestViewModel
    {
        public GuestViewModel() { }

        public GuestViewModel(Guest guest)
        {
            Name = guest.Name;
            this.IsAttending = (guest.IsAttending != null)
                ? (guest.IsAttending == true) ? "Accept" : "Decline"
                : "";
            this.IsEditable = guest.IsEditable;
        }

        public string Name { get; set; }
        public string IsAttending { get; set; }
        public bool IsEditable { get; set; }
    }

    public class RsvpJsonModel
    {
        public RsvpJsonModel()
        {
            this.value = new RsvpViewModel();
        }

        public RsvpJsonModel(Rsvp rsvp)
            : this(new RsvpViewModel(rsvp)) {}

        public RsvpJsonModel(RsvpViewModel vm)
        {
            this.id = vm.Id;
            this.label = vm.ToLabel();
            this.value = vm;          
        }

        public int? id { get; set; }
        public string label { get; set; }
        public RsvpViewModel value { get; set; }
    }

    public interface IMongoIdentified
    {
        int? Id { get; set; }
    }

    public class TermComparer : IEqualityComparer<string>
    {        
        public bool Equals(string x, string y)
        {
            return y.ToLowerInvariant().StartsWith(x.ToLowerInvariant());
        }

        public int GetHashCode(string term)
        {
            return base.GetHashCode();
        }
    }
}