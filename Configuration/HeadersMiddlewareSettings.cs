namespace ASPNETCoreWithHeadersMiddleware.Configuration
{
    public class HeadersMiddlewareSettings
    {
        public List<HeaderSetting> Requests { get; set; }
        public List<HeaderSetting> Response { get; set; }
    }

    public class HeaderSetting
    {
        public string HeaderName { get; set; }
        public string HeaderValue { get; set; }
        public bool IsActive { get; set; }
    }
}
