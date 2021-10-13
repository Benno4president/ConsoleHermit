// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using Newtonsoft.Json;
using System.Collections.Generic;

public class Address
{
    [JsonProperty("street")]
    public string Street;

    [JsonProperty("city")]
    public string City;

    [JsonProperty("postcode")]
    public string Postcode;

    [JsonProperty("telephone")]
    public string Telephone;

    [JsonProperty("mobile")]
    public string Mobile;

    [JsonProperty("email")]
    public string Email;

    [JsonProperty("email_ex")]
    public object EmailEx;

    [JsonProperty("parents_name")]
    public object ParentsName;
}

public class ClubFields
{
}

public class HoldssportMemberModel
{
    [JsonProperty("id")]
    public int Id;

    [JsonProperty("firstname")]
    public string Firstname;

    [JsonProperty("lastname")]
    public string Lastname;

    [JsonProperty("role")]
    public int Role;

    [JsonProperty("member_number")]
    public string MemberNumber;

    [JsonProperty("birthday")]
    public string Birthday;

    [JsonProperty("addresses")]
    public List<Address> Addresses;

    [JsonProperty("profile_picture_path")]
    public string ProfilePicturePath;

    [JsonProperty("club_fields")]
    public ClubFields ClubFields;
}

