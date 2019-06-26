using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

namespace EBook.Models
{
    
    
    public class Customer
    {
        
        [Key]
        public int CustomerId { get; set; }
        
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        [MaxLength(20)]
        public string NickName { get; set; }


        [NotMapped]
        [JsonIgnore] 
        public string Password { get; set; }

/*
        [JsonIgnore]
        [MaxLength(20)]
        public string PasswordHash
        {
            get { return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5").ToLower().Substring(8, 16);; }
            set { PasswordHash = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5").ToLower().Substring(8, 16);}

        }
        */     
        
        public int DefaultAddressIndex { get; set; }
        
        [MaxLength(20)]
        public string IdCardNum { get; set; }


   

        [Index]
        [Required]
        [MaxLength(20)]
        [EmailAddress]
        public string Email { get; set; }

        
 
        [Index]
        [Required]
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

        
        
        // public ICollection<CustomerAddress> CustomerAddresses { get; set; }
        
        // public ICollection<ShoppingCart> ShoppingCartRecords { get; set; }

        // public ICollection<Collect> Collection { get; set; }
        public Customer()
        {
            PhoneNum = "1233211234567";
            Email = "631101@qq.com";
        }


    }
}