namespace AuthCustom.Authorization.Definitions
{
    /// <summary>
    /// Sólo los roles que se requieran en código. Típicamente, sólo el "Admin", ya que siempre requiere un trato especial
    /// p.e., el rol admin siempre tendrá acceso a todo
    /// </summary>
    public static class Roles
    {
        public const string Admin = "Admin";
    }
}
