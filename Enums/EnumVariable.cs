namespace fake_tool.Helpers
{
    public class EnumVariable
    {
        public enum StorageStatus { Waiting, Received, Sent, Recall, Refund };
        public enum Status { Waiting, Trading, Complete, Cancel };
        public enum StatusRequest { Waiting, Approved, Denied, Cancel };
        public enum MessageE
        {
            DELETE,
            BLOCK,
            LIST_EMPTY,
            NOT_EXIST,
            ROLE_NOT_EXIST,
            EXIST,
            ID_NOT_EXIST,
            SUCCESS,
            FAILD
        }

        public enum RoleS { Admin, Staff, Customer }
    }
}
