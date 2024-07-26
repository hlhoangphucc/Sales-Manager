using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SHOPTV.Models
{
    public class Account
    {
        public int Id { get; set; }

        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} từ 6-20 kí tự")]
        public string Username { get; set; }=string.Empty;

        [DisplayName("Mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} từ 6-20 kí tự")]
        public string Password { get; set; } = string.Empty;

        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "{0} không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [DisplayName("SĐT")]
        [RegularExpression("0\\d{9}", ErrorMessage = "SĐT không hợp lệ")]
        public string Phone { get; set; } = string.Empty;

        [DisplayName("Địa chỉ")]
        public string Address { get; set; } = string.Empty;

        [DisplayName("Họ tên")]
        public string FullName { get; set; } = string.Empty;

        [DisplayName("Là admin")]
        [DefaultValue(true)]
        public bool IsAdmin { get; set; }=false;
        [DisplayName("Ảnh đại diện")]
        public string Avatar { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile AvatarFile { get; set; } = null!;
        // Collection reference property cho khóa ngoại từ Invoice
        public List<Invoice> Invoices { get; set; } = new List<Invoice>();

        // Collection reference property cho khóa ngoại từ Cart
        public List<Cart> Carts { get; set; } = new List<Cart>();
    }
}
