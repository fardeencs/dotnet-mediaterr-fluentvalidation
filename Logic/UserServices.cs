﻿using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Logic.Interface;

namespace Logic
{
    public class UserServices : UnitOfWork, IUserServices
    {
        public UserServices(IUow uow)
        {
            Uow = uow;
        }

        public List<Domain.User> GetAll()
        {
            return Uow.Users.GetAll().ToList(); 
        }

        //public Domain.User GetUserByName(string userName)
        //{
        //    return Uow.Users.GetByFilteredNoTrack(null, user => user.UserName.ToLower().Equals(userName.ToLower()));
        //}

        //public Domain.User AddUser(Domain.User model)
        //{
        //    if (model == null) return null;
        //    model.UserId = Guid.NewGuid();
        //    model.DateCaptured = DateTime.Now;
        //    Uow.Users.Add(model);
        //    Uow.Commit();
        //    Uow.Users.Detach(model);
        //    return model;
        //}

        //public Domain.User UpdateUser(Domain.User model)
        //{
        //    if (model == null) return null;
        //    var userToUpdate = Uow.Users.GetById(model.UserId);
        //    model.DateCaptured = DateTime.Now;
        //    Uow.Users.Update(model, userToUpdate.UserId);
        //    Uow.Commit();
        //    Uow.Users.Detach(model);
        //    return model;
        //}

        //public Domain.User GetUserById(Guid id)
        //{
        //    return Uow.Users.GetById(id);
        //}

        //public void DeleteUserById(Guid id)
        //{
        //    Uow.Users.Delete(id);
        //    Uow.Commit();
        //}
    }
}