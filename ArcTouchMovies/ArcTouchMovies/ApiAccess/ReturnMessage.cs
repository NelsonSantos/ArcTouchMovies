namespace ArcTouchMovies.ApiAccess
{
    public class ReturnMessage
    {
        private string m_Message = "";

        public string Message
        {
            get
            {
                return m_Message.Replace("<br/>", "\r\n");
            }
            set
            {
                m_Message = value;
            }
        }
    }
}