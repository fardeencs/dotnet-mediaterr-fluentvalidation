namespace Logic.Interface
{
    using System;
    using System.Collections.Generic;
    using Domain;
    public interface IUserServices
    {
        //Domain.User AddUser(Domain.User model);
        //void DeleteUserById(Guid id);
         List<Domain.User> GetAll();
        //Domain.User GetUserById(Guid id);
        //Domain.User GetUserByName(string userName);
        //Domain.User UpdateUser(Domain.User model);
    }
}