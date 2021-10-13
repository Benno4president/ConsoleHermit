using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleHermit.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class ActivitiesUser
    {
        [JsonProperty("picked")]
        public int Picked;

        [JsonProperty("joined_status")]
        public int JoinedStatus;

        [JsonProperty("name")]
        public string Name;
    }

    public class Action
    {
        [JsonProperty("activities_user")]
        public ActivitiesUser ActivitiesUser;
    }

    public class ActivitiesUser2
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("status")]
        public string Status;

        [JsonProperty("status_code")]
        public int StatusCode;

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt;

        [JsonProperty("user_id")]
        public int UserId;
    }

    public class Comment
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("created_at")]
        public DateTime CreatedAt;

        [JsonProperty("user_id")]
        public int UserId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("comment")]
        public string comment;
    }

    public class NoRsvp
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public string Name;
    }

    public class HoldsportActivities
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("club")]
        public string Club;

        [JsonProperty("department")]
        public bool Department;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("starttime")]
        public DateTime Starttime;

        [JsonProperty("endtime")]
        public DateTime Endtime;

        [JsonProperty("comment")]
        public string Comment;

        [JsonProperty("place")]
        public string Place;

        [JsonProperty("pickup_place")]
        public string PickupPlace;

        [JsonProperty("pickup_time")]
        public string PickupTime;

        [JsonProperty("status")]
        public int Status;

        [JsonProperty("registration_type")]
        public int RegistrationType;

        [JsonProperty("actions")]
        public List<Action> Actions;

        [JsonProperty("action_path")]
        public string ActionPath;

        [JsonProperty("action_method")]
        public string ActionMethod;

        [JsonProperty("activities_users")]
        public List<ActivitiesUser> ActivitiesUsers;

        [JsonProperty("comments")]
        public List<Comment> Comments;

        [JsonProperty("max_attendees")]
        public int MaxAttendees;

        [JsonProperty("no_rsvp_count")]
        public int NoRsvpCount;

        [JsonProperty("no_rsvp")]
        public List<NoRsvp> NoRsvp;

        [JsonProperty("ride")]
        public bool Ride;

        [JsonProperty("ride_comment")]
        public string RideComment;

        [JsonProperty("rides")]
        public List<object> Rides;

        [JsonProperty("show_ride_button")]
        public bool ShowRideButton;

        [JsonProperty("event_type")]
        public string EventType;

        [JsonProperty("event_type_id")]
        public int EventTypeId;
    }


}
