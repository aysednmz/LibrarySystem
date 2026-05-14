
 using Microsoft.AspNetCore.Identity;
 using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
 using Microsoft.EntityFrameworkCore;

 namespace LibrarySystem.Data;

 public class LibraryIdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
 {
     public LibraryIdentityDbContext(DbContextOptions<LibraryIdentityDbContext> options) : base(options)
     {
     }
 }
