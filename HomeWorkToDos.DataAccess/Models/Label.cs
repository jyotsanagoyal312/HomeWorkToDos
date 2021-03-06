﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeWorkToDos.DataAccess.Models
{
    public partial class Label
    {
        public Label()
        {
            ToDoItem = new HashSet<ToDoItem>();
            ToDoList = new HashSet<ToDoList>();
        }

        [Key]
        public int LabelId { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Label")]
        public virtual User User { get; set; }
        [InverseProperty("Label")]
        public virtual ICollection<ToDoItem> ToDoItem { get; set; }
        [InverseProperty("Label")]
        public virtual ICollection<ToDoList> ToDoList { get; set; }
    }
}