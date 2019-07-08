using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Principal;
using Newtonsoft.Json;

namespace EBook.Models
{
    
    
    public class Customer
    {
        
        [Key]
        public int CustomerId { get; set; }
        
        
        [MaxLength(20)]
        public string RealName { get; set; }
        
        
        [MaxLength(20)]
        public string NickName { get; set; }
        
        
        [MaxLength(1000)]
        public string Password { get; set; }
        
        
        public int DefaultAddressIndex { get; set; }
        
        
        [MaxLength(20)]
        public string IdCardNum { get; set; }


   

        [Index]
        [Index(IsUnique = true)]
        [Required]
        [MaxLength(20)]
        [EmailAddress]
        public string Email { get; set; }

        
 
        [Index(IsUnique = true)]
        [Required]
        [Phone]
        [MaxLength(11)]
        public string PhoneNum{ get; set; }
        
        [NotMapped]
        public string BirthDayDate
        {
            get
            {
                return DateOfBirth.Date.ToString("dd/MM/yyyy");
            }
        }
        
        [JsonIgnore]
        public DateTime DateOfBirth{ get; set; }
        
        
        
        
        public int Point { get; set; }
        
        public int IsValid { get; set; }
        
        
        [MaxLength(60)]
        public string AvatarPath { set; get; }

        
        
        [JsonIgnore]
        public ICollection<CustomerAddress> CustomerAddresses { get; set; }
        
                        
        
        [JsonIgnore]
        public ICollection<Answer> Answers { get; set; }

        
        [JsonIgnore]
        public ICollection<Question> Questions { get; set; }


        public Customer()
        {
            IsValid = 1;
        }
        
        
    }
}