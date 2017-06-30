using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JobBoardMVC.Models
{
	public class CompanyFile
	{
		[Key]
		public int FileId { get; set; }
        //public string LogoId { get; set; }

		[StringLength(255)]
		public string FileName { get; set; }

		[StringLength(100)]
		public string ContentType { get; set; }
		public byte[] Content { get; set; }
		public FileType FileType { get; set; }
		public string CompanyName { get; set; }

		//nav property
		public virtual Company Company { get; set; }
	}
}