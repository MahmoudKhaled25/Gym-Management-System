namespace Gym_Management_System.Abstractions.Consts;

public static class DefaultRoles
{
    public partial class Admin
    {
        public const string Name = nameof(Admin);
        public const string Id = "019d5f90-527e-7234-8876-456086c0ef0f";
        public const string ConcurrencyStamp = "019d5f90-527e-7234-8876-4561d8a8a2db";
    }
    
    public partial class Member
    {
        public const string Name = nameof(Member);
        public const string Id = "019d5f90-527e-7234-8876-456220b0d0d1";
        public const string ConcurrencyStamp = "019d5f90-527e-7234-8876-45635f3dcaa5";
    }

    public partial class Trainer
    {
        public const string Name = nameof(Trainer);
        public const string Id = "019d5f90-527e-7234-8876-456442c7a04a";
        public const string ConcurrencyStamp = "019d5f90-527e-7234-8876-45650cededb1";
    }
}
