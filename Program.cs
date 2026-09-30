/****************/
/* Mã sinh viên: 202418906 */
/* Họ tên: Lê Gia Huân */
/****************/
using System;
using System.Globalization;
using System.Text;

namespace ProjectManagement
{
    /// <summary>Chương trình kiểm thử theo 15 bước của đề bài + các trường hợp biên.</summary>
    public class Program
    {
        static void Step(string text)
        {
            Console.WriteLine();
            Console.WriteLine(">>> " + text);
        }

        static void Check(string label, bool actual, bool expected)
        {
            Console.WriteLine("  " + (actual == expected ? "[OK]  " : "[LỖI] ") +
                label + " -> " + actual);
        }

        static void ExpectException(string label, Action action)
        {
            try
            {
                action();
                Console.WriteLine("  [LỖI] " + label + " -> không ném ngoại lệ");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("  [OK]  " + label + " -> bị từ chối: " + ex.Message);
            }
        }

        public static void Main()
        {
            Console.OutputEncoding = new UTF8Encoding(false);
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine("########## CHƯƠNG TRÌNH KIỂM THỬ ##########");

            // 1. Hai Employee bằng hai constructor khác nhau
            Step("1. Tạo hai Employee bằng hai constructor khác nhau");
            Employee e1 = new Employee("E001", "Nguyễn Văn An");
            Employee e2 = new Employee("E002", "Trần Thị Bình", 10000000);

            // 2. Hai SoftwareEngineer bằng hai constructor khác nhau
            Step("2. Tạo hai SoftwareEngineer bằng hai constructor khác nhau");
            SoftwareEngineer se1 = new SoftwareEngineer("SE01", "Lê Văn Cường", "C#");
            SoftwareEngineer se2 = new SoftwareEngineer("SE02", "Phạm Thị Dung", 15000000, "Java", 3000000);
            e1.DisplayInfo(); e2.DisplayInfo(); se1.DisplayInfo(); se2.DisplayInfo();

            // 3. Tăng lương bằng số tiền cố định
            Step("3. Tăng lương E002 thêm 500.000 (cố định)");
            e2.IncreaseSalary(500000);
            Console.WriteLine("  Lương E002 = " + e2.BaseSalary.ToString("N0"));

            // 4. Tăng lương theo phần trăm
            Step("4. Tăng lương SE02 thêm 10% (theo phần trăm)");
            se2.IncreaseSalary(10, true);
            Console.WriteLine("  Lương SE02 = " + se2.BaseSalary.ToString("N0"));

            // 5. Nhóm không có trưởng nhóm
            Step("5. Tạo nhóm dự án không có trưởng nhóm");
            ProjectTeam team1 = new ProjectTeam("PRJ01", "Hệ thống quản lý nhân sự");
            team1.DisplayTeam();

            // 6. addMember(employee)
            Step("6. Thêm E002 bằng AddMember(employee)");
            Check("AddMember(E002)", team1.AddMember(e2), true);

            // 7. addMember(employee, true)
            Step("7. Thêm SE02 bằng AddMember(employee, true) -> trưởng nhóm");
            Check("AddMember(SE02, true)", team1.AddMember(se2, true), true);
            Console.WriteLine("  Trưởng nhóm hiện tại: " + team1.Leader.Id);

            // 8. Thêm lại thành viên đã tồn tại
            Step("8. Thử thêm lại thành viên đã tồn tại");
            Check("AddMember(E002) lần 2", team1.AddMember(e2), false);
            Check("AddMember(SE02) lần 2", team1.AddMember(se2), false);
            Console.WriteLine("  Số thành viên vẫn là: " + team1.MemberCount);

            // 9. Hiển thị bằng lời gọi đa hình
            Step("9. Hiển thị danh sách (đa hình)");
            team1.DisplayTeam();

            // 10. Tổng chi phí hằng tháng
            Step("10. Tổng chi phí nhân sự hằng tháng");
            Console.WriteLine("  Tổng = " + team1.CalculateTotalMonthlyCost().ToString("N0"));
            Console.WriteLine("  (E002: 10.500.000 + SE02: 16.500.000 + 3.000.000 = 30.000.000)");

            // 11. Xóa trưởng nhóm hiện tại
            Step("11. Thử xóa trưởng nhóm hiện tại");
            Check("RemoveMember(SE02) khi còn là trưởng nhóm", team1.RemoveMember("SE02"), false);
            Check("SE02 vẫn còn trong nhóm", team1.Contains("SE02"), true);

            // 12. Đổi trưởng nhóm rồi xóa người từng là trưởng nhóm
            Step("12. Đổi trưởng nhóm sang E002 rồi xóa SE02");
            Check("ChangeLeader(E002)", team1.ChangeLeader(e2), true);
            Console.WriteLine("  Trưởng nhóm mới: " + team1.Leader.Id);
            Check("RemoveMember(SE02)", team1.RemoveMember("SE02"), true);
            Check("Contains(SE02)", team1.Contains("SE02"), false);
            team1.DisplayTeam();

            // 13. Nhóm thứ hai với nhân sự đã có ở nhóm thứ nhất
            Step("13. Tạo nhóm thứ hai và thêm E002 (đã ở nhóm 1) -> kết tập nhiều nhóm");
            ProjectTeam team2 = new ProjectTeam("PRJ02", "Ứng dụng di động");
            Check("team2.AddMember(E002)", team2.AddMember(e2), true);
            Check("team1.Contains(E002)", team1.Contains("E002"), true);
            Check("team2.Contains(E002)", team2.Contains("E002"), true);
            team2.Dispose();

            // 14 + 15. Hủy nhóm bằng khối lệnh cục bộ, chứng minh nhân sự vẫn tồn tại
            Step("14. Hủy nhóm thứ hai bằng khối lệnh cục bộ (using)");
            {
                using (ProjectTeam local = new ProjectTeam("PRJ03", "Nhóm tạm", se1))
                {
                    local.AddMember(e1);
                    local.DisplayTeam();
                    Console.WriteLine("  ...kết thúc khối lệnh, nhóm PRJ03 sắp bị hủy...");
                }
            }
            Step("15. Nhân sự sau khi nhóm bị hủy vẫn tồn tại");
            se1.DisplayInfo();
            e1.DisplayInfo();
            e2.DisplayInfo();
            Console.WriteLine("  E002 vẫn thuộc team1: " + team1.Contains("E002"));

            // ===== Các trường hợp biên =====
            Console.WriteLine();
            Console.WriteLine("########## CÁC TRƯỜNG HỢP BIÊN ##########");
            ExpectException("Employee mã rỗng", () => new Employee("", "Tên"));
            ExpectException("Employee họ tên toàn khoảng trắng", () => new Employee("E9", "   "));
            ExpectException("Employee lương âm", () => new Employee("E9", "Tên", -1));
            ExpectException("SoftwareEngineer ngôn ngữ rỗng", () => new SoftwareEngineer("S9", "Tên", ""));
            ExpectException("SoftwareEngineer phụ cấp âm", () => new SoftwareEngineer("S9", "Tên", 1000, "C#", -5));
            ExpectException("IncreaseSalary(0)", () => e2.IncreaseSalary(0));
            ExpectException("IncreaseSalary(-100)", () => e2.IncreaseSalary(-100));
            ExpectException("IncreaseSalary(-5, true)", () => e2.IncreaseSalary(-5, true));
            ExpectException("IncreaseSalary(NaN)", () => e2.IncreaseSalary(double.NaN));
            ExpectException("ProjectTeam mã dự án rỗng", () => new ProjectTeam("", "X"));
            try
            {
                new ProjectTeam("PX", "X", null);
                Console.WriteLine("  [LỖI] leader null không bị từ chối");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("  [OK]  ProjectTeam leader null -> bị từ chối");
            }

            Employee defaultEmp = new Employee();
            Console.WriteLine("  Employee mặc định: Id=" + defaultEmp.Id + ", Tên=" + defaultEmp.FullName +
                ", Lương=" + defaultEmp.BaseSalary);

            Employee p = new Employee("P1", "Lương 1000", 1000);
            p.IncreaseSalary(50, false);
            Console.WriteLine("  IncreaseSalary(50, false) -> +50 cố định: " + p.BaseSalary);
            p.IncreaseSalary(100, true);
            Console.WriteLine("  IncreaseSalary(100, true) -> +100%: " + p.BaseSalary);

            ProjectTeam t = new ProjectTeam("PRJ04", "Nhóm biên");
            Check("Xóa thành viên không tồn tại", t.RemoveMember("NOPE"), false);
            Check("Xóa khi nhóm rỗng, chưa có trưởng nhóm", t.RemoveMember("E001"), false);
            Check("AddMember(null)", t.AddMember(null), false);
            Check("ChangeLeader(null)", t.ChangeLeader(null), false);
            Console.WriteLine("  Tổng chi phí nhóm rỗng = " + t.CalculateTotalMonthlyCost());
            Employee dup = new Employee("E001", "Trùng mã với E001");
            t.AddMember(e1);
            Check("Thêm nhân sự khác đối tượng nhưng trùng mã E001", t.AddMember(dup), false);
            Check("ChangeLeader(nhân sự trùng mã khác đối tượng)", t.ChangeLeader(dup), false);
            Check("AddMember(E001, true) khi E001 đã có -> đổi trưởng, không thêm trùng", t.AddMember(e1, true), true);
            Console.WriteLine("  Số thành viên: " + t.MemberCount + ", trưởng nhóm: " + t.Leader.Id);
            Check("Xóa trưởng nhóm duy nhất", t.RemoveMember("E001"), false);
            t.AddMember(e2);
            t.ChangeLeader(e2);
            Check("Xóa E001 sau khi đổi trưởng nhóm", t.RemoveMember("E001"), true);
            t.DisplayTeam();

            // Dọn dẹp: giải phóng tường minh các đối tượng
            Console.WriteLine();
            Console.WriteLine("########## GIẢI PHÓNG ĐỐI TƯỢNG ##########");
            t.Dispose();
            team1.Dispose();
            e1.Dispose(); e2.Dispose(); se1.Dispose(); se2.Dispose();
            defaultEmp.Dispose(); p.Dispose(); dup.Dispose();
            Console.WriteLine("Hoàn tất.");
        }
    }
}
