namespace MyLaptopWebsite.Utils
{
    public static class OrderState
    {
        private static Dictionary<int, string> State = new()
        {
            { 0, "Đang kiểm duyệt" },
            { 1, "Đang giao hàng" },
            { 2, "Đã giao" }
        };
        public static string getStateLabel(int stateId)
        {
            try
            {
                return State[stateId];
            }
            catch
            {
                return "";
            }
        }
        public static Dictionary<int, string> getStates()
        {
            return State;
        }
    }
}
