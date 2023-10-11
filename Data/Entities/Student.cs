using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECStudentManagerWebAPI.Data.Entities;

[Index(nameof(SocialSecurityNumber), IsUnique = true)]//sätter personumret till unikt
public class Student
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }

    [Column(TypeName = "nchar(13)")]
    public string SocialSecurityNumber { get; set; }

    [MaxLength(50)]
    public string Email { get; set; }
}