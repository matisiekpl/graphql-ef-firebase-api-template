using System.ComponentModel.DataAnnotations;

namespace MyProject.Entities;

public class User
{
    [Key] public Guid UserId { get; set; }
    public string FirebaseUserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    
    public User(string firebaseUserId, string email)
    {
        FirebaseUserId = firebaseUserId;
        Email = email;
    }
}