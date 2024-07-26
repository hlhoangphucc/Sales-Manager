using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SHOPTV.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [DisplayName("Mã HĐ")]
        public string Code { get; set; }=string.Empty;

        public int AccountId { get; set; }

        // Navigation reference property cho khóa ngoại đến Account 
        [DisplayName("Khách hàng")]
        public Account? Account { get; set; }

        [DisplayName("Ngày lập")]
        [DataType(DataType.DateTime)]
        public DateTime IssuedDate { get; set; } = DateTime.Now;

        [DisplayName("Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; } = string.Empty;

        [DisplayName("SĐT giao hàng")]
        public string ShippingPhone { get; set; }= string.Empty;

        [DisplayName("Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        [DefaultValue(0)]
        public int Total { get; set; } = 0;

        [DisplayName("Còn hiệu lực")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        // Collection reference property cho khóa ngoại từ InvoiceDetail
        public List<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }
}
