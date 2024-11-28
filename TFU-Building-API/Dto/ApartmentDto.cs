namespace TFU_Building_API.Dto
{
    public class ApartmentResponseDto
    {
        public int STT { get; set; } // Serial Number
        public Guid ApartmentId { get; set; }
        public string OwnerName { get; set; }
        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        public int NumberOfMembers { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BuildingName { get; set; } // Add Building Name
    }


    public class ApartmentMemberDetailDto
    {
        public Guid Id { get; set; }
        public int STT { get; set; }
        public string MemberName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class AddApartmentMemberDto
    {
        public string MemberName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid ApartmentId { get; set; }
    }

    public class AddApartmentMemberResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ApartmentDto
    {
        public Guid Id { get; set; }
        public int RoomNumber { get; set; }
    }

}
