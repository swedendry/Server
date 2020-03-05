namespace Server.Hubs.Models
{
    public class HubBody
    {
        public string msg { get; set; }                 // 결과 메세지
        public int code { get; set; }                   // 결과 코드
    }

    /// <summary>
    /// 채팅 정보
    /// </summary>
    public class CS_Chat
    {
        public string Message { get; set; }
    }

    public class SC_Chat : HubBody
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 로그인 정보
    /// </summary>
    public class CS_Login
    {
        public string Id { get; set; }
    }

    public class SC_Login : HubBody
    {
        public User User { get; set; }
    }
}
