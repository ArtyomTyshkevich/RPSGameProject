namespace Auth.BLL.DTOs.Identity
{
    public class ScopeAttribute : Attribute
    {
        public string Scope { get; set; }

        public ScopeAttribute(string scope)
        {
            Scope = scope;
        }
    }
}