/****************/
/* Mã sinh viên: 202418906 */
/* Họ tên: Lê Gia Huân */
/****************/
using System;
using System.Globalization;

namespace ProjectManagement
{
    /// <summary>
    /// Lớp cơ sở: nhân sự thông thường.
    /// Bất biến: Id không rỗng, FullName không rỗng, BaseSalary >= 0.
    /// </summary>
    public class Employee : IDisposable
    {
        private string id;          // mã nhân sự
        private string fullName;    // họ tên
        private double baseSalary;  // lương cơ bản
        private bool disposed;
        // true khi đối tượng đã khởi tạo hợp lệ xong (tránh in thông báo hủy cho đối tượng tạo lỗi)
        protected bool ready;

        // ---------- Constructor (nạp chồng) ----------

        /// <summary>Constructor mặc định: UNKNOWN / Unnamed employee / 0.</summary>
        public Employee() : this("UNKNOWN", "Unnamed employee", 0)
        {
        }

        /// <summary>Constructor 2 tham số: lương cơ bản = 0.</summary>
        public Employee(string id, string fullName) : this(id, fullName, 0)
        {
        }

        /// <summary>Constructor đầy đủ, kiểm tra toàn bộ ràng buộc.</summary>
        public Employee(string id, string fullName, double baseSalary)
        {
            this.id = RequireNonEmpty(id, "Mã nhân sự");
            this.fullName = RequireNonEmpty(fullName, "Họ tên");
            this.baseSalary = RequireNonNegative(baseSalary, "Lương cơ bản");
            ready = true;
            Console.WriteLine("  [Employee] Tạo: " + this.id);
        }

        // ---------- Getter ----------
        public string Id { get { return id; } }
        public string FullName { get { return fullName; } }
        public double BaseSalary { get { return baseSalary; } }

        // ---------- Nạp chồng phương thức tăng lương ----------

        /// <summary>Tăng lương một số tiền cố định (amount phải dương).</summary>
        public void IncreaseSalary(double amount)
        {
            if (double.IsNaN(amount) || double.IsInfinity(amount) || amount <= 0)
                throw new ArgumentException("Giá trị tăng lương phải dương.");
            baseSalary += amount;
        }

        /// <summary>
        /// byPercentage = true: tăng theo phần trăm lương hiện tại;
        /// byPercentage = false: tăng số tiền cố định. value phải dương.
        /// </summary>
        public void IncreaseSalary(double value, bool byPercentage)
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || value <= 0)
                throw new ArgumentException("Giá trị tăng lương phải dương.");
            if (byPercentage)
                baseSalary += baseSalary * value / 100.0;
            else
                baseSalary += value;
        }

        // ---------- Đa hình ----------

        /// <summary>Chi phí hằng tháng: mặc định bằng lương cơ bản.</summary>
        public virtual double CalculateMonthlyCost()
        {
            return baseSalary;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine("  [Employee] ID=" + id + " | Tên=" + fullName +
                " | Lương cơ bản=" + Money(baseSalary) +
                " | Chi phí tháng=" + Money(CalculateMonthlyCost()));
        }

        // ---------- Vòng đời ----------

        /// <summary>Giải phóng tường minh (tương đương destructor xác định của C++).</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Virtual: lớp con ghi đè để in thông báo của riêng mình rồi gọi base.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            disposed = true;
            Console.WriteLine("  [~Employee] Hủy: " + id);
        }

        // Finalizer: dự phòng nếu quên gọi Dispose
        ~Employee()
        {
            if (ready) Dispose(false);
        }

        // ---------- Tiện ích dùng chung ----------

        protected static string RequireNonEmpty(string value, string field)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(field + " không được rỗng.");
            return value.Trim();
        }

        protected static double RequireNonNegative(double value, string field)
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || value < 0)
                throw new ArgumentException(field + " không được âm.");
            return value;
        }

        protected static string Money(double v)
        {
            return v.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}
