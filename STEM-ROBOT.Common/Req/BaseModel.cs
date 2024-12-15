using System;

namespace STEM_ROBOT.Common.Req
{
    public enum StatusEnum
    {
        Active,
        Inactive,
        Pending,
        Archived
        // Thêm các trạng thái khác nếu cần
    }

    public class BaseModel
    {
        public int Id { get; set; }
        public StatusEnum Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public BaseModel(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID phải là số nguyên dương.", nameof(id));

            Id = id;
            CreatedOn = DateTime.Now;
            Status = StatusEnum.Pending; // Trạng thái mặc định hoặc tùy chỉnh nếu cần
        }

        // Phương thức xác minh tổng thể
        public void Verify()
        {
            VerifyId();
            VerifyStatus();
            VerifyTimestamps();
            VerifyCreatedBy();
            VerifyModifiedBy();
        }

        // Xác minh ID
        private void VerifyId()
        {
            if (Id <= 0)
                throw new ArgumentException("ID phải là số nguyên dương.");
        }

        // Xác minh Status
        private void VerifyStatus()
        {
            if (!Enum.IsDefined(typeof(StatusEnum), Status))
                throw new ArgumentException("Trạng thái không hợp lệ.");
        }

        // Xác minh CreatedOn và ModifiedOn
        private void VerifyTimestamps()
        {
            if (CreatedOn.HasValue && CreatedOn > DateTime.Now)
                throw new ArgumentException("CreatedOn không thể là ngày trong tương lai.");

            if (ModifiedOn.HasValue && CreatedOn.HasValue && ModifiedOn < CreatedOn)
                throw new ArgumentException("ModifiedOn không thể sớm hơn CreatedOn.");
        }

        // Xác minh CreatedBy
        private void VerifyCreatedBy()
        {
            if (CreatedBy.HasValue && CreatedBy <= 0)
                throw new ArgumentException("CreatedBy phải là số nguyên dương hoặc null.");
        }

        // Xác minh ModifiedBy
        private void VerifyModifiedBy()
        {
            if (ModifiedBy.HasValue && ModifiedBy <= 0)
                throw new ArgumentException("ModifiedBy phải là số nguyên dương hoặc null.");
        }
    }
}
