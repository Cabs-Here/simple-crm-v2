using System;

namespace Classroom.SimpleCRM
{
    public interface IUserData
    {
        CrmIdentityUser Get(Guid id);
        CrmIdentityUser GetSingle(string userName);
    }
}