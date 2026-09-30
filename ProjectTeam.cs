/****************/
/* Mã sinh viên: 202418906 */
/* Họ tên: Lê Gia Huân */
/****************/
using System;
using System.Collections.Generic;

namespace ProjectManagement
{
    /// <summary>
    /// Nhóm dự án. Quan hệ KẾT TẬP (aggregation) với Employee:
    /// nhóm chỉ giữ tham chiếu, KHÔNG sở hữu nhân sự; một nhân sự có thể
    /// thuộc nhiều nhóm và vẫn tồn tại khi nhóm bị hủy.
    ///
    /// Bất biến:
    ///  (1) Không có hai thành viên cùng mã trong một nhóm.
    ///  (2) Nếu đã có trưởng nhóm thì trưởng nhóm phải nằm trong danh sách thành viên.
    ///  (3) Một nhân sự không xuất hiện hai lần trong cùng nhóm.
    ///  (4) Mã dự án và tên dự án không rỗng.
    /// </summary>
    public class ProjectTeam : IDisposable
    {
        private string projectCode;
        private string projectName;
        private Employee leader;                          // liên kết không sở hữu
        private List<Employee> members = new List<Employee>(); // liên kết không sở hữu

        /// <summary>Tạo nhóm chưa có trưởng nhóm.</summary>
        public ProjectTeam(string projectCode, string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
                throw new ArgumentException("Mã dự án không được rỗng.");
            if (string.IsNullOrWhiteSpace(projectName))
                throw new ArgumentException("Tên dự án không được rỗng.");
            this.projectCode = projectCode.Trim();
            this.projectName = projectName.Trim();
            Console.WriteLine("  [ProjectTeam] Tạo nhóm: " + this.projectCode);
        }

        /// <summary>Tạo nhóm có trưởng nhóm; trưởng nhóm tự động vào danh sách thành viên.</summary>
        public ProjectTeam(string projectCode, string projectName, Employee leader)
            : this(projectCode, projectName)
        {
            if (leader == null)
                throw new ArgumentNullException("leader", "Trưởng nhóm không được null.");
            members.Add(leader);
            this.leader = leader;
        }

        public string ProjectCode { get { return projectCode; } }
        public string ProjectName { get { return projectName; } }
        public Employee Leader { get { return leader; } }
        public int MemberCount { get { return members.Count; } }

        /// <summary>Kiểm tra nhân sự có trong nhóm theo mã.</summary>
        public bool Contains(string employeeId)
        {
            return Find(employeeId) != null;
        }

        private Employee Find(string employeeId)
        {
            if (employeeId == null) return null;
            foreach (Employee e in members)
                if (e.Id == employeeId.Trim()) return e;
            return null;
        }

        /// <summary>Thêm thành viên. Trả về false nếu null hoặc trùng mã.</summary>
        public bool AddMember(Employee employee)
        {
            if (employee == null) return false;
            if (Contains(employee.Id)) return false;   // bất biến (1), (3)
            members.Add(employee);
            return true;
        }

        /// <summary>
        /// Thêm thành viên và (nếu makeLeader) đặt làm trưởng nhóm.
        /// - Nếu chưa có trong nhóm: thêm rồi đặt làm trưởng nhóm.
        /// - Nếu đã có trong nhóm và makeLeader = true: không thêm lần hai,
        ///   chỉ đổi trưởng nhóm (trả về true).
        /// - Nếu đã có và makeLeader = false: trả về false (trùng).
        /// Trưởng nhóm cũ vẫn là thành viên.
        /// </summary>
        public bool AddMember(Employee employee, bool makeLeader)
        {
            if (employee == null) return false;
            if (!makeLeader) return AddMember(employee);
            return ChangeLeader(employee);
        }

        /// <summary>
        /// Xóa thành viên theo mã. Từ chối (false) nếu không tồn tại
        /// hoặc là trưởng nhóm hiện tại (phải đổi trưởng nhóm trước).
        /// </summary>
        public bool RemoveMember(string employeeId)
        {
            Employee e = Find(employeeId);
            if (e == null) return false;
            if (leader != null && ReferenceEquals(e, leader)) return false;
            members.Remove(e);   // chỉ gỡ tham chiếu, KHÔNG hủy Employee
            return true;
        }

        /// <summary>
        /// Đổi trưởng nhóm. Nếu nhân sự chưa thuộc nhóm thì thêm vào.
        /// Từ chối nếu null, hoặc trùng mã với một nhân sự KHÁC đã có trong nhóm.
        /// </summary>
        public bool ChangeLeader(Employee employee)
        {
            if (employee == null) return false;
            Employee existing = Find(employee.Id);
            if (existing == null)
                members.Add(employee);
            else if (!ReferenceEquals(existing, employee))
                return false;   // cùng mã nhưng là đối tượng khác -> vi phạm bất biến
            leader = employee;
            return true;
        }

        /// <summary>Tổng chi phí hằng tháng (gọi đa hình CalculateMonthlyCost).</summary>
        public double CalculateTotalMonthlyCost()
        {
            double total = 0;
            foreach (Employee e in members)
                total += e.CalculateMonthlyCost();
            return total;
        }

        public void DisplayTeam()
        {
            Console.WriteLine("  === Dự án " + projectCode + " - " + projectName + " ===");
            Console.WriteLine("  Trưởng nhóm: " + (leader == null ? "(chưa có)" : leader.Id + " - " + leader.FullName));
            Console.WriteLine("  Thành viên (" + members.Count + "):");
            foreach (Employee e in members)
                e.DisplayInfo();   // gọi đa hình
        }

        /// <summary>
        /// Chỉ hủy cấu trúc danh sách nội bộ, KHÔNG hủy các Employee.
        /// </summary>
        public void Dispose()
        {
            members.Clear();
            leader = null;
            Console.WriteLine("  [~ProjectTeam] Hủy nhóm: " + projectCode + " (không hủy Employee)");
        }
    }
}
