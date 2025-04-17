using System;
using System.ComponentModel.DataAnnotations;

namespace flight_manager.Models
{
	public class StaffModelDto
	{
		public StaffModelDto(StaffMembers staffMember)
		{
            Id = staffMember.Id;
            Username = staffMember.Username;
            Email = staffMember.Email;
            FirstName = staffMember.FirstName;
            LastName = staffMember.LastName;
            EGN = staffMember.EGN;
            Address = staffMember.Address;
            PhoneNumber = staffMember.PhoneNumber;
            Rank = staffMember.Rank;
		}

        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string EGN { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Rank { get; set; } = null!;
    }
}

