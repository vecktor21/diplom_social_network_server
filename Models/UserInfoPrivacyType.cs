namespace server.Models
{
    public class UserInfoPrivacyType
    {
        /*
         UserInfoPrivacyTypeId:
        1 - публичная (доступ всем)
        2 - приватная (доступ никому)
        3 - только друзьям
         */
        public int UserInfoPrivacyTypeId {  get; set; }
        public string UserInfoPrivacyTypeName { get; set; }

    }
}
