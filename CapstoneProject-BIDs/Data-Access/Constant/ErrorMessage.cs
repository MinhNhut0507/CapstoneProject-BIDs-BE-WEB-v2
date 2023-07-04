namespace Data_Access.Constant
{
    public static class ErrorMessage
    {
        #region Common error message
        public static class CommonError
        {
            public readonly static string NAME_IS_NULL = "Tên trồng(vui lòng nhập tên)";
            public readonly static string ID_IS_NULL = "ID trống(Vui lòng nhập ID)";
            public readonly static string INVALID_REQUEST = "Yêu cầu không hợp lệ";
            public readonly static string ACCOUNT_NAME_IS_EXITED = "Tài khoản đã tồn tại";
            public readonly static string EMAIL_IS_EXITED = "Email Đã tồn tại";
            public readonly static string WRONG_EMAIL_FORMAT = "Email sai định dạng";
            public readonly static string CCCD_NUMBER_IS_EXITED = "Số CCCD đã tồn tại";
            public readonly static string WRONG_CCCD_NUMBER_FORMAT = "Số CCCD sai định dạng";
            public readonly static string PHONE_IS_EXITED = "Số điện thoại đã tồn tại";
            public readonly static string WRONG_PHONE_FORMAT = "Số điện thoại sai định dạng";
            public readonly static string EMAIL_IS_NULL = "Email trống(vui lòng nhập email)";
        }
        #endregion

        #region User error message
        public static class UserError
        {
            public readonly static string USER_NOT_FOUND = "Người dùng không tồn tại";
            public readonly static string USER_EXISTED = "Người dùng đã tồn tại";
            public readonly static string ACCOUNT_CREATE_NOT_FOUND = "Tài khoản khởi tạo không tồn tại";
        }
        #endregion

        #region Staff error message
        public static class StaffError
        {
            public readonly static string STAFF_NOT_FOUND = "Nhân viên không tồn tại";
            public readonly static string STAFF_EXISTED = "Nhân viên đã tồn tại";
        }
        #endregion

        #region Admin error message
        public static class AdminError
        {
            public readonly static string ADMIN_NOT_FOUND = "Quản trị viên không tồn tại";
            public readonly static string ADMIN_EXISTED = "Quản trị viên đã tồn tại";
        }
        #endregion

        #region Fee error message
        public static class FeeError
        {
            public readonly static string FEE_NOT_FOUND = "Bảng giá không tài tại";
            public readonly static string FEE_EXISTED = "Bảng giá đã tồn tại";
        }
        #endregion

        #region Login error message
        public static class LoginError
        {
            public readonly static string WRONG_ACCOUNT_NAME_OR_PASSWORD = "Wrong account name or password";
        }
        #endregion

        #region Category error message
        public static class CategoryError
        {
            public readonly static string CATEGORY_NOT_FOUND = "Loại sản phẩm không tồn tại";
            public readonly static string CATEGORY_EXISTED = "Loại sản phẩm đã tồn tài";
        }
        #endregion

        #region Description error message
        public static class DescriptionError
        {
            public readonly static string DESCRIPTION_NOT_FOUND = "Chi tiết cho sản phầm không tồn tại";
            public readonly static string DESCRIPTION_EXISTED = "Chi tiết cho sản phẩm đã tồn tài";
        }
        #endregion

        #region Session error message
        public static class SessionError
        {
            public readonly static string SESSION_NOT_FOUND = "Phiên đấu giá không tồn tại";
            public readonly static string SESSION_EXISTED = "Phiên đấu giá đã tồn tại";
            public readonly static string DATE_TIME_BEGIN_END_ERROR = "Sai thông tin ngày đấu giá(Ngày bắt đầu sau ngày kết thúc)";
            public readonly static string DATE_TIME_LATE_ERROR = "Sai thông tin ngày đấu giá(Ngày bắt đầu trong quá khứ)";
            public readonly static string DATE_TIME_BEGIN_ERROR = "Sai thông tin ngày đấu giá(Ngày bắt đầu phải cách ngày tạo ít nhất 1 ngày)";
            public readonly static string OUT_OF_DATE_BEGIN_ERROR = "Đã quá hạn đăng ký tham gia đấu giá";
        }
        #endregion

        #region Ban History error message
        public static class BanHistoryError
        {
            public readonly static string BAN_HISTORY_NOT_FOUND = "Lịch sử tài khoản khóa không tồn tại";
        }
        #endregion

        #region Item error message
        public static class ItemError
        {
            public readonly static string ITEM_NOT_FOUND = "Sản phẩm không tồn tại";
            public readonly static string ITEM_EXISTED = "Sản phẩm đã tồn tại";
            public readonly static string INVALID_STEP_PRICE = "Bước giá không hợp lệ(5-10% giá khởi điểm)";
        }
        #endregion

        #region Payment error message
        public static class PaymentError
        {
            public readonly static string PAYMENT_NOT_FOUND = "Đơn thanh toán không tồn tại";
            public readonly static string PAYMENT_EXISTED = "Đơn thanh toán đã tồn tại";
        }
        #endregion

        #region Auction History error message
        public static class AuctionHistoryError
        {
            public readonly static string AUCTION_HISTORY_NOT_FOUND = "Lịch sử đấu giá không tồn tại";
            public readonly static string AUCTION_HISTORY_EXISTED = "Lịch sử đấu giá đã tồn tại";
        }
        #endregion

        #region Booking Item error message
        public static class BookingItemError
        {
            public readonly static string BOOKING_ITEM_NOT_FOUND = "Đơn đăng ký sản phẩm không tồn tại";
            public readonly static string BOOKING_ITEM_EXISTED = "Đơn đăng ký sản phẩm đã tồn tại";
        }
        #endregion

        #region Item Description error message
        public static class ItemDescriptionError
        {
            public readonly static string ITEM_DESCRIPTION_NOT_FOUND = "Mô tả sản phẩm không tồn tại";
            public readonly static string ITEM_DESCRIPTION_EXISTED = "Mô tả sản phẩm đã tồn tại";
        }
        #endregion
    }
}
