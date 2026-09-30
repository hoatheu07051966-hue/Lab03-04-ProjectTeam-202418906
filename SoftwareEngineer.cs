/****************/
/* Mã sinh viên: 202418906 */
/* Họ tên: Lê Gia Huân */
/****************/
using System;

namespace ProjectManagement
{
    /// <summary>
    /// Kỹ sư phần mềm: kế thừa công khai từ Employee,
    /// thêm ngôn ngữ lập trình chính và phụ cấp kỹ thuật.
    /// </summary>
    public class SoftwareEngineer : Employee
    {
        private string primaryLanguage;     // ngôn ngữ lập trình chính
        private double technicalAllowance;  // phụ cấp kỹ thuật

        /// <summary>Constructor 3 tham số: lương = 0, phụ cấp = 0.</summary>
        public SoftwareEngineer(string id, string fullName, string primaryLanguage)
            : base(id, fullName)
        {
            ready = false;   // chỉ coi là hợp lệ khi kiểm tra xong phần của lớp con
            this.primaryLanguage = RequireNonEmpty(primaryLanguage, "Ngôn ngữ chính");
            this.technicalAllowance = 0;
            ready = true;
        }

        /// <summary>Constructor đầy đủ.</summary>
        public SoftwareEngineer(string id, string fullName, double baseSalary,
                                string primaryLanguage, double technicalAllowance)
            : base(id, fullName, baseSalary)
        {
            ready = false;
            this.primaryLanguage = RequireNonEmpty(primaryLanguage, "Ngôn ngữ chính");
            this.technicalAllowance = RequireNonNegative(technicalAllowance, "Phụ cấp kỹ thuật");
            ready = true;
        }

        public string PrimaryLanguage { get { return primaryLanguage; } }
        public double TechnicalAllowance { get { return technicalAllowance; } }

        /// <summary>Ghi đè: lương cơ bản + phụ cấp kỹ thuật.</summary>
        public override double CalculateMonthlyCost()
        {
            return BaseSalary + technicalAllowance;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("  [SoftwareEngineer] ID=" + Id + " | Tên=" + FullName +
                " | Ngôn ngữ=" + primaryLanguage +
                " | Lương cơ bản=" + Money(BaseSalary) +
                " | Phụ cấp=" + Money(technicalAllowance) +
                " | Chi phí tháng=" + Money(CalculateMonthlyCost()));
        }

        protected override void Dispose(bool disposing)
        {
            Console.WriteLine("  [~SoftwareEngineer] Hủy kỹ sư: " + Id);
            base.Dispose(disposing);
        }
    }
}
